namespace BankAccountApp;

public class BankAccount
{
    public string CustomerName { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string customerName, decimal initialBalance)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new ArgumentException("Customer name is required.", nameof(customerName));
        }

        CustomerName = customerName;
        Balance = initialBalance;
    }

    public void Debit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Debit amount must be positive.");
        }

        if (amount > Balance)
        {
            throw new InvalidOperationException("Insufficient funds");
        }

        Balance -= amount;
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Credit amount must be positive.");
        }

        Balance += amount;
    }

    public void Withdraw(decimal amount) => Debit(amount);
}

