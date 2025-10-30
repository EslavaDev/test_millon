namespace RealEstate.Domain.ValueObjects;

/// <summary>
/// Represents a money value with currency support.
/// This is an immutable value object following DDD principles.
/// </summary>
public readonly struct Money : IEquatable<Money>, IComparable<Money>
{
    /// <summary>
    /// The monetary amount.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// The currency code (e.g., "USD", "EUR", "COP").
    /// </summary>
    public string Currency { get; }

    /// <summary>
    /// Creates a new Money instance.
    /// </summary>
    /// <param name="amount">The monetary amount.</param>
    /// <param name="currency">The currency code. Defaults to "USD".</param>
    public Money(decimal amount, string currency = "USD")
    {
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot be null or empty.", nameof(currency));
        }

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    /// <summary>
    /// Creates a zero money value.
    /// </summary>
    public static Money Zero(string currency = "USD") => new(0, currency);

    /// <summary>
    /// Checks if two Money values are equal.
    /// </summary>
    public bool Equals(Money other)
    {
        return Amount == other.Amount && Currency == other.Currency;
    }

    /// <summary>
    /// Checks if this Money value equals another object.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Money other && Equals(other);
    }

    /// <summary>
    /// Gets the hash code for this Money value.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    /// <summary>
    /// Compares this Money value with another.
    /// </summary>
    public int CompareTo(Money other)
    {
        if (Currency != other.Currency)
        {
            throw new InvalidOperationException($"Cannot compare money values with different currencies: {Currency} vs {other.Currency}");
        }

        return Amount.CompareTo(other.Amount);
    }

    /// <summary>
    /// Returns a string representation of this Money value.
    /// </summary>
    public override string ToString()
    {
        return $"{Amount:N2} {Currency}";
    }

    // Arithmetic operators
    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException($"Cannot add money values with different currencies: {left.Currency} vs {right.Currency}");
        }

        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException($"Cannot subtract money values with different currencies: {left.Currency} vs {right.Currency}");
        }

        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public static Money operator *(Money money, decimal multiplier)
    {
        return new Money(money.Amount * multiplier, money.Currency);
    }

    public static Money operator /(Money money, decimal divisor)
    {
        if (divisor == 0)
        {
            throw new DivideByZeroException("Cannot divide money by zero.");
        }

        return new Money(money.Amount / divisor, money.Currency);
    }

    // Comparison operators
    public static bool operator ==(Money left, Money right) => left.Equals(right);
    public static bool operator !=(Money left, Money right) => !left.Equals(right);
    public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;
    public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;
    public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;
    public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;
}
