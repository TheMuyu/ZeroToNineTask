using LoanApplication.Application.Persistence;
using Microsoft.Extensions.Logging;

namespace LoanApplication.Infrastructure.Repositories
{
    public class LoanApplicationRepository : ILoanApplicationRepository
    {
        private static readonly List<Domain.Entities.LoanApplication> _loanApplications = new(); // Static list for simulation
        private readonly ILogger<LoanApplicationRepository> _logger;

        public LoanApplicationRepository(ILogger<LoanApplicationRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync(Domain.Entities.LoanApplication loanApplication, CancellationToken cancellationToken = default)
        {
            if (loanApplication == null) throw new ArgumentNullException(nameof(loanApplication));

            _loanApplications.Add(loanApplication);
            _logger.LogInformation("Added loan application with ID: {Id}", loanApplication.Id);
            await Task.CompletedTask; // Simulate async work
        }

        public IReadOnlyCollection<Domain.Entities.LoanApplication> GetAllApplications()
        {
            _logger.LogInformation("Returning {Count} loan applications", _loanApplications.Count);
            return _loanApplications.AsReadOnly();
        }
    }
}