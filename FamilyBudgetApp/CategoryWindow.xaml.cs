using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FamilyBudgetApp.Models;

namespace FamilyBudgetApp
{
    public partial class CategoryWindow : Window
    {
        public List<Category> Categories { get; private set; }
        public List<Category> UpdatedCategories => Categories;
        private List<Transaction> transactions;
        public CategoryWindow(List<Category> categories, List<Transaction> transactions)
        {
            InitializeComponent();
            Categories = categories;
            this.transactions = transactions;

            CategoryList.ItemsSource = Categories;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newCat = NewCategoryBox.Text.Trim();
            if (!string.IsNullOrEmpty(newCat) && !Categories.Exists(c => c.Name.Equals(newCat, System.StringComparison.OrdinalIgnoreCase)))
            {
                Categories.Add(new Category { Name = newCat });
                CategoryList.Items.Refresh();
                NewCategoryBox.Clear();
            }
            else
            {
                MessageBox.Show("Category already exists or is empty.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryList.SelectedItem is Category selected)
            {
                bool isUsed = transactions.Any(t => t.Category == selected.Name);
                if (isUsed)
                {
                    MessageBox.Show($"Cannot delete category '{selected.Name}' because it is used in one or more transactions.", "In Use", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Delete category '{selected.Name}'?", "Confirm", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Categories.Remove(selected);
                    CategoryList.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select a category to delete.");
            }
        }


    }
}
