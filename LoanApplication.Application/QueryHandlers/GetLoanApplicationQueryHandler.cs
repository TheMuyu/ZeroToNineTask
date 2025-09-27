using LoanApplication.Application.Persistence;
using LoanApplication.Application.Queries;
using MediatR;

namespace LoanApplication.Application.QueryHandlers
{
    public class GetLoanApplicationQueryHandler : IRequestHandler<GetLoanApplicationQuery, LoanApplicationDto>
    {
        private readonly ILoanApplicationRepository _repository;

        public GetLoanApplicationQueryHandler(ILoanApplicationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<LoanApplicationDto> Handle(GetLoanApplicationQuery request, CancellationToken cancellationToken)
        {
            // Simulate retrieval from in-memory List
            var loanApplications = await Task.FromResult(_repository.GetAllApplications());
            var loanApplication = loanApplications.FirstOrDefault(a => a.Id == request.Id);

            if (loanApplication == null)
            {
                return null; // Controller will handle 404
            }

            return new LoanApplicationDto
            {
                Id = loanApplication.Id,
                ApplicantName = loanApplication.ApplicantName,
                ApplicantIncome = loanApplication.ApplicantIncome.Amount,
                LoanAmount = loanApplication.LoanAmount.Amount,
                LoanTermMonths = loanApplication.LoanTermMonths,
                CountryCode = loanApplication.CountryCode.Value
            };
        }
    }
}