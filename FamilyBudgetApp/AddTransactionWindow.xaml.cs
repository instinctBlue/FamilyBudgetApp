using FamilyBudgetApp.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FamilyBudgetApp
{
    public partial class AddTransactionWindow : Window
    {
        public Transaction Transaction { get; private set; }

        private List<Category> categories;

        public AddTransactionWindow(List<Category> categories)
        {
            InitializeComponent();
            this.categories = categories;
            CategoryBox.ItemsSource = categories;
            if (categories.Count > 0)
                CategoryBox.SelectedIndex = 0;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionBox.Text) ||
                string.IsNullOrWhiteSpace(AmountBox.Text) ||
                !decimal.TryParse(AmountBox.Text, out decimal amount))
            {
                MessageBox.Show("Please fill all fields with valid values.");
                return;
            }

            Transaction = new Transaction
            {
                Description = DescriptionBox.Text,
                Amount = Math.Abs(amount), // Ensure it's always positive
                Category = ((Category)CategoryBox.SelectedItem).Name,
                IsIncome = IsIncomeBox.IsChecked == true
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
