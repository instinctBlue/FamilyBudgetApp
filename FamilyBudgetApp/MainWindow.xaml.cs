using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FamilyBudgetApp.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace FamilyBudgetApp
{
    public partial class MainWindow : Window
    {
        private List<Transaction> transactions = new List<Transaction>();
        private List<Category> categories = new List<Category>();

        private int nextId = 1;

        private readonly string jsonFilePath = "transactions.json";
        private readonly string categoriesFilePath = "categories.json";

        private List<Category> LoadCategories()
        {
            if (!File.Exists(categoriesFilePath))
                return new List<Category>
        {
            new Category { Name = "Food" },
            new Category { Name = "Utilities" },
            new Category { Name = "Salary" },
            new Category { Name = "Entertainment" },
            new Category { Name = "Other" }
        };

            string json = File.ReadAllText(categoriesFilePath);
            return JsonSerializer.Deserialize<List<Category>>(json) ?? new List<Category>();
        }
        private void SaveCategories(List<Category> categories)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(categories, options);
            File.WriteAllText(categoriesFilePath, json);
        }

        private void SaveTransactions(List<Transaction> transactions)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() } // if you use enums, optional here
            };

            string json = JsonSerializer.Serialize(transactions, options);
            File.WriteAllText(jsonFilePath, json);
        }

        private List<Transaction> LoadTransactions()
        {
            if (!File.Exists(jsonFilePath))
                return new List<Transaction>();

            string json = File.ReadAllText(jsonFilePath);
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() } // match Save settings if needed
            };

            return JsonSerializer.Deserialize<List<Transaction>>(json, options);
        }

        
        public MainWindow()
        {
            InitializeComponent();
            transactions = LoadTransactions();
            categories = LoadCategories();
            RefreshUI();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddTransactionWindow(categories);
            if (addWindow.ShowDialog() == true)
            {
                var newTransaction = addWindow.Transaction;
                transactions.Add(newTransaction);
                SaveTransactions(transactions);
                RefreshUI();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionGrid.SelectedItem is Transaction selected)
            {
                transactions.Remove(selected);
                SaveTransactions(transactions);
                RefreshUI();
            }
            else
            {
                MessageBox.Show("Please select a transaction to delete.");
            }
        }

        private void RefreshUI()
        {
            TransactionGrid.ItemsSource = null;
            TransactionGrid.ItemsSource = transactions;

            decimal income = transactions.Where(t => t.IsIncome).Sum(t => t.Amount);
            decimal expense = transactions.Where(t => !t.IsIncome).Sum(t => t.Amount);
            decimal balance = income - expense;

            SummaryText.Text = $"Income: {income:C}   |   Expense: {expense:C}   |   Balance: {balance:C}";
        }

        private void ManageCategories_Click(object sender, RoutedEventArgs e)
        {
            var categoryWindow = new CategoryWindow(categories, transactions);
            categoryWindow.ShowDialog();

            categories = categoryWindow.UpdatedCategories;
            SaveCategories(categories);
        }



    }
}
