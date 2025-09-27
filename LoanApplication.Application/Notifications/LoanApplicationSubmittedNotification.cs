using MediatR;

namespace LoanApplication.Application.Notifications
{
    public record LoanApplicationSubmittedNotification(Guid LoanApplicationId) : INotification;
}