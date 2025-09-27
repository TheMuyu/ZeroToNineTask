using LoanApplication.Application.Persistence;
using LoanApplication.Application.Queries;
using LoanApplication.Application.QueryHandlers;
using Moq;

namespace LoanApplication.Tests
{
    public class GetLoanApplicationQueryHandlerTests
    {
        private readonly Mock<ILoanApplicationRepository> _repositoryMock;
        private readonly GetLoanApplicationQueryHandler _handler;

        public GetLoanApplicationQueryHandlerTests()
        {
            _repositoryMock = new Mock<ILoanApplicationRepository>();
            _handler = new GetLoanApplicationQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingId_ReturnsLoanApplicationDto()
        {
            // Arrange
            var loanApplication = Domain.Entities.LoanApplication.Create("John Doe", 35000, 250000, 60, "SE");
            var id = loanApplication.Id;
            _repositoryMock.Setup(r => r.GetAllApplications()).Returns(new[] { loanApplication }.AsReadOnly());
            var query = new GetLoanApplicationQuery(id);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("John Doe", result.ApplicantName);
            Assert.Equal(35000m, result.ApplicantIncome);
            Assert.Equal(250000m, result.LoanAmount);
            Assert.Equal(60, result.LoanTermMonths);
            Assert.Equal("SE", result.CountryCode);
        }

        [Fact]
        public async Task Handle_NonExistingId_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetAllApplications()).Returns(Array.Empty<Domain.Entities.LoanApplication>().AsReadOnly());
            var query = new GetLoanApplicationQuery(id);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}