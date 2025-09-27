using LoanApplication.Application.CommandHandlers;
using LoanApplication.Application.Commands;
using LoanApplication.Application.Notifications;
using LoanApplication.Application.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace LoanApplication.Tests
{
    public class SubmitLoanApplicationCommandHandlerTests
    {
        private readonly Mock<ILoanApplicationRepository> _repositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<SubmitLoanApplicationCommandHandler>> _loggerMock;
        private readonly SubmitLoanApplicationCommandHandler _handler;

        public SubmitLoanApplicationCommandHandlerTests()
        {
            _repositoryMock = new Mock<ILoanApplicationRepository>();
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<SubmitLoanApplicationCommandHandler>>();
            _handler = new SubmitLoanApplicationCommandHandler(_repositoryMock.Object, _mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsLoanApplicationId()
        {
            // Arrange
            var command = new SubmitLoanApplicationCommand
            {
                ApplicantName = "John Doe",
                ApplicantIncome = 35000,
                LoanAmount = 250000,
                LoanTermMonths = 60,
                CountryCode = "SE"
            };
            var loanApplication = Domain.Entities.LoanApplication.Create(command.ApplicantName, command.ApplicantIncome, command.LoanAmount, command.LoanTermMonths, command.CountryCode);
            var expectedId = loanApplication.Id;
            _repositoryMock.Setup(r => r.AddAsync(It.Is<Domain.Entities.LoanApplication>(la =>
                la.ApplicantName == command.ApplicantName &&
                la.ApplicantIncome.Amount == command.ApplicantIncome &&
                la.LoanAmount.Amount == command.LoanAmount &&
                la.LoanTermMonths == command.LoanTermMonths &&
                la.CountryCode.Value == command.CountryCode), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mediatorMock.Setup(m => m.Publish(It.IsAny<LoanApplicationSubmittedNotification>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result); // Ensure a valid GUID is returned
            _repositoryMock.Verify(r => r.AddAsync(It.Is<Domain.Entities.LoanApplication>(la =>
                la.ApplicantName == command.ApplicantName &&
                la.ApplicantIncome.Amount == command.ApplicantIncome &&
                la.LoanAmount.Amount == command.LoanAmount &&
                la.LoanTermMonths == command.LoanTermMonths &&
                la.CountryCode.Value == command.CountryCode), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Publish(It.IsAny<LoanApplicationSubmittedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsArgumentException()
        {
            // Arrange
            var command = new SubmitLoanApplicationCommand
            {
                ApplicantName = "", // Invalid: empty name
                ApplicantIncome = 35000,
                LoanAmount = 2500, // Invalid: below 10000
                LoanTermMonths = 60,
                CountryCode = "SE"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}