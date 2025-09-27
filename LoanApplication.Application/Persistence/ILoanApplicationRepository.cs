namespace LoanApplication.Application.Persistence
{
    public interface ILoanApplicationRepository
    {
        Task AddAsync(Domain.Entities.LoanApplication loanApplication, CancellationToken cancellationToken = default);
        IReadOnlyCollection<Domain.Entities.LoanApplication> GetAllApplications();
    }
}