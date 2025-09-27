namespace LoanApplication.Domain.Events
{
    public record LoanApplicationSubmittedEvent(Guid LoanApplicationId);
}