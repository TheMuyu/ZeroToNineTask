namespace LoanApplication.Domain.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; }

        private Money(decimal amount)
        {
            if (amount < 0)
            {
                // enforces the imvariant that money canot be negative
                throw new ArgumentException("Money amount cannot be negative.");
            }

            Amount = amount;
        }

        public static Money FromDecimal(decimal amount) => new Money(amount);
    }
}