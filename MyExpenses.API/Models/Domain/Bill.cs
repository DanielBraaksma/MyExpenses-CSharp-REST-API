using System.ComponentModel;
using System.Runtime.Serialization;

namespace MyExpenses.API.Models.Domain
{
    public class Bill
    {
        public Guid Id { get; set; }
        // Foreign key to link the bill to the user who owns it
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public string Category { get; set; }

    }
}
