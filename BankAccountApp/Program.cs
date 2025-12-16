using BankAccountApp;

var account = new BankAccount("Trung Pham", 1000m);
account.Debit(200m);
Console.WriteLine($"{account.CustomerName} balance: {account.Balance}");
