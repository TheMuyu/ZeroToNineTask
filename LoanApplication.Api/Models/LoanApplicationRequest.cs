using System.ComponentModel.DataAnnotations;

namespace LoanApplication.Api.Models
{
    public class LoanApplicationRequest
    {
        [Required(ErrorMessage = "Applicant name is required.")]
        [StringLength(100, ErrorMessage = "Applicant name must be under 100 characters.")]
        public string ApplicantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Applicant income is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Income must be positive.")]
        public decimal ApplicantIncome { get; set; }

        [Required(ErrorMessage = "Loan amount is required.")]
        [Range(10000.01, double.MaxValue, ErrorMessage = "Loan amount must be greater than 10000.")]
        public decimal LoanAmount { get; set; }

        [Required(ErrorMessage = "Loan term in months is required.")]
        [Range(6, 360, ErrorMessage = "Loan term must be between 6 and 360 months.")]
        public int LoanTermMonths { get; set; }

        [Required(ErrorMessage = "Country code is required.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be exactly 2 characters.")]
        public string CountryCode { get; set; } = string.Empty;
    }
}