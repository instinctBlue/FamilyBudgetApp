using System;

namespace FamilyBudgetApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public bool IsIncome { get; set; }

        public string Type => IsIncome ? "Income" : "Expense";
    }
}
