using BusinessLogicLayer.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Models
{
    public class TaxPayer : ITaxPayer
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+(?:\s[a-zA-Z]+)+$", ErrorMessage = "The Full Name is invalid")]
        public string FullName { get; set; }

        [Required]
        [Range(typeof(ulong), "1", "9999999999")]
        public ulong SSN { get; set; }

        [Required]
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessage = "Gross Income is not a valid positive number or decimal")]
        public decimal GrossIncome { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [RegularExpression(@"^((\n|\s*|\r$)|([0-99]\d{0,60}(\.\d{1,60})?%?$))$", ErrorMessage = "Charity Spent is not a valid number or decimal")]
        public decimal? CharitySpent { get; set; }
    }
}
