namespace MyExpenses.API.DTO
{
    public class BillDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public string Category { get; set; }
    }
}
