using System;
using System.Collections.Generic;

// Interface for basic banking operations
public interface IBankingOperations
{
    void Deposit(decimal amount);
    void Withdraw(decimal amount);
    void Transfer(Account targetAccount, decimal amount);
    void DisplayBalance();
}

// Account class
public class Account : IBankingOperations
{
    public string AccountNumber { get; private set; }
    public string AccountHolder { get; private set; }
    private decimal Balance;
    public List<string> TransactionHistory { get; private set; }

    public Account(string accountNumber, string accountHolder, decimal initialBalance = 0)
    {
        AccountNumber = accountNumber;
        AccountHolder = accountHolder;
        Balance = initialBalance;
        TransactionHistory = new List<string>();
        AddTransaction($"Account created with initial balance: {Balance:C}");
    }

    private void AddTransaction(string transaction)
    {
        TransactionHistory.Add($"[{DateTime.Now}] {transaction}");
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Deposit amount must be greater than zero.");
            return;
        }
        Balance += amount;
        AddTransaction($"Deposited: {amount:C}");
        Console.WriteLine($"Deposited {amount:C} successfully.");
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Withdrawal amount must be greater than zero.");
            return;
        }
        if (amount > Balance)
        {
            Console.WriteLine("Insufficient funds.");
            return;
        }
        Balance -= amount;
        AddTransaction($"Withdrew: {amount:C}");
        Console.WriteLine($"Withdrew {amount:C} successfully.");
    }

    public void Transfer(Account targetAccount, decimal amount)
    {
        if (targetAccount == null)
        {
            Console.WriteLine("Target account is invalid.");
            return;
        }
        if (amount <= 0)
        {
            Console.WriteLine("Transfer amount must be greater than zero.");
            return;
        }
        if (amount > Balance)
        {
            Console.WriteLine("Insufficient funds for transfer.");
            return;
        }
        Withdraw(amount);
        targetAccount.Deposit(amount);
        AddTransaction($"Transferred: {amount:C} to {targetAccount.AccountHolder}");
        Console.WriteLine($"Transferred {amount:C} to {targetAccount.AccountHolder} successfully.");
    }

    public void DisplayBalance()
    {
        Console.WriteLine($"Account Holder: {AccountHolder}, Account Number: {AccountNumber}, Balance: {Balance:C}");
    }

    public void DisplayTransactionHistory()
    {
        Console.WriteLine($"Transaction History for {AccountHolder} ({AccountNumber}):");
        foreach (var transaction in TransactionHistory)
        {
            Console.WriteLine(transaction);
        }
    }
}

// Main banking application
public class BankingApp
{
    private static List<Account> Accounts = new List<Account>();

    public static void Main(string[] args)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n=== Banking System ===");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Transfer");
            Console.WriteLine("5. Display Account Balance");
            Console.WriteLine("6. View Transaction History");
            Console.WriteLine("7. Exit");
            Console.Write("Select an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    CreateAccount();
                    break;
                case "2":
                    PerformDeposit();
                    break;
                case "3":
                    PerformWithdrawal();
                    break;
                case "4":
                    PerformTransfer();
                    break;
                case "5":
                    DisplayAccountBalance();
                    break;
                case "6":
                    ViewTransactionHistory();
                    break;
                case "7":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void CreateAccount()
    {
        Console.Write("Enter Account Holder Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Initial Balance: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal initialBalance))
        {
            Console.WriteLine("Invalid balance amount.");
            return;
        }

        string accountNumber = Guid.NewGuid().ToString().Substring(0, 8);
        Accounts.Add(new Account(accountNumber, name, initialBalance));
        Console.WriteLine($"Account created successfully. Account Number: {accountNumber}");
    }

    private static void PerformDeposit()
    {
        Account account = GetAccountByNumber();
        if (account == null) return;

        Console.Write("Enter deposit amount: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            Console.WriteLine("Invalid amount.");
            return;
        }
        account.Deposit(amount);
    }

    private static void PerformWithdrawal()
    {
        Account account = GetAccountByNumber();
        if (account == null) return;

        Console.Write("Enter withdrawal amount: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            Console.WriteLine("Invalid amount.");
            return;
        }
        account.Withdraw(amount);
    }

    private static void PerformTransfer()
    {
        Console.Write("Enter your account number: ");
        Account sourceAccount = GetAccountByNumber();
        if (sourceAccount == null) return;

        Console.Write("Enter target account number: ");
        Account targetAccount = GetAccountByNumber();
        if (targetAccount == null) return;

        Console.Write("Enter transfer amount: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            Console.WriteLine("Invalid amount.");
            return;
        }
        sourceAccount.Transfer(targetAccount, amount);
    }

    private static void DisplayAccountBalance()
    {
        Account account = GetAccountByNumber();
        if (account == null) return;
        account.DisplayBalance();
    }

    private static void ViewTransactionHistory()
    {
        Account account = GetAccountByNumber();
        if (account == null) return;
        account.DisplayTransactionHistory();
    }

    private static Account GetAccountByNumber()
    {
        Console.Write("Enter Account Number: ");
        string accountNumber = Console.ReadLine();

        Account account = Accounts.Find(acc => acc.AccountNumber == accountNumber);
        if (account == null)
        {
            Console.WriteLine("Account not found.");
        }
        return account;
    }
}

