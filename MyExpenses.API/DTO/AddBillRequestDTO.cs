using System.ComponentModel.DataAnnotations;

namespace MyExpenses.API.DTO
{
    public class AddBillRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage ="Must be a min of 3 chars")]
        [MaxLength(40, ErrorMessage ="Max length is 12 chars")]
        public string Name { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Category { get; set; }
    }
}
