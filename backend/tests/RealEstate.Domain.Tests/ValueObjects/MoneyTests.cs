using FluentAssertions;
using RealEstate.Domain.ValueObjects;

namespace RealEstate.Domain.Tests.ValueObjects;

[TestFixture]
public class MoneyTests
{
    [Test]
    public void Constructor_WithValidAmount_ShouldCreateMoney()
    {
        // Arrange & Act
        var money = new Money(100.50m, "USD");

        // Assert
        money.Amount.Should().Be(100.50m);
        money.Currency.Should().Be("USD");
    }

    [Test]
    public void Constructor_WithoutCurrency_ShouldDefaultToUSD()
    {
        // Arrange & Act
        var money = new Money(100m);

        // Assert
        money.Amount.Should().Be(100m);
        money.Currency.Should().Be("USD");
    }

    [Test]
    public void Constructor_WithLowercaseCurrency_ShouldConvertToUppercase()
    {
        // Arrange & Act
        var money = new Money(100m, "eur");

        // Assert
        money.Currency.Should().Be("EUR");
    }

    [Test]
    public void Constructor_WithNullCurrency_ShouldThrowArgumentException()
    {
        // Arrange & Act
        Action act = () => new Money(100m, null!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Currency cannot be null or empty.*");
    }

    [Test]
    public void Constructor_WithEmptyCurrency_ShouldThrowArgumentException()
    {
        // Arrange & Act
        Action act = () => new Money(100m, "");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Currency cannot be null or empty.*");
    }

    [Test]
    public void Constructor_WithWhitespaceCurrency_ShouldThrowArgumentException()
    {
        // Arrange & Act
        Action act = () => new Money(100m, "   ");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Currency cannot be null or empty.*");
    }

    [Test]
    public void Zero_ShouldReturnZeroMoney()
    {
        // Arrange & Act
        var money = Money.Zero();

        // Assert
        money.Amount.Should().Be(0m);
        money.Currency.Should().Be("USD");
    }

    [Test]
    public void Zero_WithCurrency_ShouldReturnZeroMoneyWithSpecifiedCurrency()
    {
        // Arrange & Act
        var money = Money.Zero("EUR");

        // Assert
        money.Amount.Should().Be(0m);
        money.Currency.Should().Be("EUR");
    }

    [Test]
    public void Equals_WithSameAmountAndCurrency_ShouldReturnTrue()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(100m, "USD");

        // Act & Assert
        money1.Equals(money2).Should().BeTrue();
        (money1 == money2).Should().BeTrue();
    }

    [Test]
    public void Equals_WithDifferentAmount_ShouldReturnFalse()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(200m, "USD");

        // Act & Assert
        money1.Equals(money2).Should().BeFalse();
        (money1 != money2).Should().BeTrue();
    }

    [Test]
    public void Equals_WithDifferentCurrency_ShouldReturnFalse()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(100m, "EUR");

        // Act & Assert
        money1.Equals(money2).Should().BeFalse();
        (money1 != money2).Should().BeTrue();
    }

    [Test]
    public void GetHashCode_WithSameAmountAndCurrency_ShouldReturnSameHashCode()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(100m, "USD");

        // Act & Assert
        money1.GetHashCode().Should().Be(money2.GetHashCode());
    }

    [Test]
    public void Addition_WithSameCurrency_ShouldReturnSum()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(50m, "USD");

        // Act
        var result = money1 + money2;

        // Assert
        result.Amount.Should().Be(150m);
        result.Currency.Should().Be("USD");
    }

    [Test]
    public void Addition_WithDifferentCurrencies_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(50m, "EUR");

        // Act
        Action act = () => { var result = money1 + money2; };

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot add money values with different currencies: USD vs EUR");
    }

    [Test]
    public void Subtraction_WithSameCurrency_ShouldReturnDifference()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(30m, "USD");

        // Act
        var result = money1 - money2;

        // Assert
        result.Amount.Should().Be(70m);
        result.Currency.Should().Be("USD");
    }

    [Test]
    public void Subtraction_WithDifferentCurrencies_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(30m, "EUR");

        // Act
        Action act = () => { var result = money1 - money2; };

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot subtract money values with different currencies: USD vs EUR");
    }

    [Test]
    public void Multiplication_WithPositiveMultiplier_ShouldReturnProduct()
    {
        // Arrange
        var money = new Money(100m, "USD");

        // Act
        var result = money * 2.5m;

        // Assert
        result.Amount.Should().Be(250m);
        result.Currency.Should().Be("USD");
    }

    [Test]
    public void Multiplication_WithZero_ShouldReturnZero()
    {
        // Arrange
        var money = new Money(100m, "USD");

        // Act
        var result = money * 0m;

        // Assert
        result.Amount.Should().Be(0m);
        result.Currency.Should().Be("USD");
    }

    [Test]
    public void Multiplication_WithNegativeMultiplier_ShouldReturnNegativeAmount()
    {
        // Arrange
        var money = new Money(100m, "USD");

        // Act
        var result = money * -2m;

        // Assert
        result.Amount.Should().Be(-200m);
        result.Currency.Should().Be("USD");
    }

    [Test]
    public void Division_WithPositiveDivisor_ShouldReturnQuotient()
    {
        // Arrange
        var money = new Money(100m, "USD");

        // Act
        var result = money / 4m;

        // Assert
        result.Amount.Should().Be(25m);
        result.Currency.Should().Be("USD");
    }

    [Test]
    public void Division_WithZero_ShouldThrowDivideByZeroException()
    {
        // Arrange
        var money = new Money(100m, "USD");

        // Act
        Action act = () => { var result = money / 0m; };

        // Assert
        act.Should().Throw<DivideByZeroException>()
            .WithMessage("Cannot divide money by zero.");
    }

    [Test]
    public void CompareTo_WithSmallerAmount_ShouldReturnPositive()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(50m, "USD");

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        result.Should().BePositive();
        (money1 > money2).Should().BeTrue();
        (money1 >= money2).Should().BeTrue();
    }

    [Test]
    public void CompareTo_WithLargerAmount_ShouldReturnNegative()
    {
        // Arrange
        var money1 = new Money(50m, "USD");
        var money2 = new Money(100m, "USD");

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        result.Should().BeNegative();
        (money1 < money2).Should().BeTrue();
        (money1 <= money2).Should().BeTrue();
    }

    [Test]
    public void CompareTo_WithEqualAmount_ShouldReturnZero()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(100m, "USD");

        // Act
        var result = money1.CompareTo(money2);

        // Assert
        result.Should().Be(0);
        (money1 <= money2).Should().BeTrue();
        (money1 >= money2).Should().BeTrue();
    }

    [Test]
    public void CompareTo_WithDifferentCurrencies_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(50m, "EUR");

        // Act
        Action act = () => money1.CompareTo(money2);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot compare money values with different currencies: USD vs EUR");
    }

    [Test]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var money = new Money(1234.56m, "USD");

        // Act
        var result = money.ToString();

        // Assert - Check that it contains the amount and currency (format may vary by culture)
        result.Should().Contain("1").And.Contain("234").And.Contain("56").And.Contain("USD");
        result.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void ToString_WithZero_ShouldReturnFormattedZero()
    {
        // Arrange
        var money = Money.Zero("EUR");

        // Act
        var result = money.ToString();

        // Assert - Check that it contains zero and currency (format may vary by culture)
        result.Should().Contain("0").And.Contain("EUR");
        result.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void Money_ShouldBeImmutable()
    {
        // Arrange
        var money = new Money(100m, "USD");
        var originalAmount = money.Amount;
        var originalCurrency = money.Currency;

        // Act - Perform operations that create new instances
        var _ = money + new Money(50m, "USD");
        var __ = money * 2;

        // Assert - Original should remain unchanged
        money.Amount.Should().Be(originalAmount);
        money.Currency.Should().Be(originalCurrency);
    }

    [Test]
    public void Money_WithNegativeAmount_ShouldBeAllowed()
    {
        // Arrange & Act
        var money = new Money(-100m, "USD");

        // Assert
        money.Amount.Should().Be(-100m);
        money.Currency.Should().Be("USD");
    }

    [Test]
    public void Money_ComparisonOperators_ShouldWorkCorrectly()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(100m, "USD");
        var money3 = new Money(50m, "USD");
        var money4 = new Money(150m, "USD");

        // Assert
        (money1 == money2).Should().BeTrue();
        (money1 != money3).Should().BeTrue();
        (money1 > money3).Should().BeTrue();
        (money1 >= money2).Should().BeTrue();
        (money3 < money1).Should().BeTrue();
        (money3 <= money1).Should().BeTrue();
        (money4 > money1).Should().BeTrue();
    }
}
