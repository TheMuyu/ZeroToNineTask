namespace LoanApplication.Api.Models
{
    public record LoanApplicationCreatedResponse
    {
        public Guid loanApplicationId { get; init; }
    }
}