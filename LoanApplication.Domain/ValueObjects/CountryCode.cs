namespace LoanApplication.Domain.ValueObjects
{
    public record CountryCode
    {
        public string Value { get; }

        public CountryCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 2)
            {
                // Enforce the rule that country codes must be exactly 2 characters.
                throw new ArgumentException("Country code must be exactly 2 characters.");
            }

            Value = value.ToUpperInvariant();
        }
    }
}