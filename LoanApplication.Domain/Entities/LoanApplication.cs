using LoanApplication.Domain.Events;
using LoanApplication.Domain.ValueObjects;

namespace LoanApplication.Domain.Entities
{
    public class LoanApplication
    {
        public Guid Id { get; private set; }
        public string ApplicantName { get; private set; }
        public Money ApplicantIncome { get; private set; }
        public Money LoanAmount { get; private set; }
        public int LoanTermMonths { get; private set; }
        public CountryCode CountryCode { get; private set; }

        private readonly List<LoanApplicationSubmittedEvent> _domainEvents = new();

        public IReadOnlyCollection<LoanApplicationSubmittedEvent> DomainEvents => _domainEvents.AsReadOnly();

        private LoanApplication() { } // Private for factory only

        public static LoanApplication Create(
            string applicantName,
            decimal applicantIncome,
            decimal loanAmount,
            int loanTermMonths,
            string countryCode)
        {
            // Enforce invariants in domain
            if (string.IsNullOrWhiteSpace(applicantName) || applicantName.Length > 100)
                throw new ArgumentException("Applicant name is required and must be under 100 characters.");

            var income = Money.FromDecimal(applicantIncome);
            var amount = Money.FromDecimal(loanAmount);

            if (amount.Amount < 10000)
                throw new ArgumentException("Loan amount must be greater than 10000.");

            if (loanTermMonths < 6 || loanTermMonths > 360)
                throw new ArgumentException("Loan term must be between 6 and 360 months.");

            var country = new CountryCode(countryCode);

            var application = new LoanApplication
            {
                Id = Guid.NewGuid(),
                ApplicantName = applicantName,
                ApplicantIncome = income,
                LoanAmount = amount,
                LoanTermMonths = loanTermMonths,
                CountryCode = country
            };

            application.AddDomainEvent(new LoanApplicationSubmittedEvent(application.Id));

            return application;
        }

        public void AddDomainEvent(LoanApplicationSubmittedEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}