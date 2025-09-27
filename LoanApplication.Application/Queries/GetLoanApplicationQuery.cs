using MediatR;

namespace LoanApplication.Application.Queries
{
    public record GetLoanApplicationQuery(Guid Id) : IRequest<LoanApplicationDto>;

    public record LoanApplicationDto
    {
        public Guid Id { get; init; }
        public string ApplicantName { get; init; } = string.Empty;
        public decimal ApplicantIncome { get; init; }
        public decimal LoanAmount { get; init; }
        public int LoanTermMonths { get; init; }
        public string CountryCode { get; init; } = string.Empty;
    }
}