using LoanApplication.Application.Commands;
using LoanApplication.Application.Notifications;
using LoanApplication.Application.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanApplication.Application.CommandHandlers
{
    public class SubmitLoanApplicationCommandHandler : IRequestHandler<SubmitLoanApplicationCommand, Guid>
    {
        private readonly ILoanApplicationRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<SubmitLoanApplicationCommandHandler> _logger;

        public SubmitLoanApplicationCommandHandler(
                    ILoanApplicationRepository repository,
                    IMediator mediator,
                    ILogger<SubmitLoanApplicationCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        public async Task<Guid> Handle(SubmitLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing loan application command for {ApplicantName}", request.ApplicantName);

            // Create domain entity with validation
            var loanApplication = Domain.Entities.LoanApplication.Create(
                request.ApplicantName,
                request.ApplicantIncome,
                request.LoanAmount,
                request.LoanTermMonths,
                request.CountryCode);

            // Simulate persistence
            await _repository.AddAsync(loanApplication, cancellationToken);

            // Publish domain event by converting to MediatR notification
            // This could be done in a more generic way with a DomainEventDispatcher
            var domainEvent = loanApplication.DomainEvents.FirstOrDefault();
            if (domainEvent != null)
            {
                var notification = new LoanApplicationSubmittedNotification(domainEvent.LoanApplicationId);
                await _mediator.Publish(notification, cancellationToken);
                _logger.LogInformation("Published domain event for loan application {Id}", domainEvent.LoanApplicationId);
                loanApplication.ClearDomainEvents(); // Clean up after publishing
            }

            _logger.LogInformation("Loan application {Id} created successfully", loanApplication.Id);

            // Return the ID (201 Created will use this in API)
            return loanApplication.Id;
        }
    }
}