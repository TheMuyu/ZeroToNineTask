using MediatR;

namespace LoanApplication.Application.Commands
{
    public class SubmitLoanApplicationCommand : IRequest<Guid>
    {
        public string ApplicantName { get; set; } = string.Empty;
        public decimal ApplicantIncome { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanTermMonths { get; set; }
        public string CountryCode { get; set; } = string.Empty;
    }
}