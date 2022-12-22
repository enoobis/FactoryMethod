using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BankSystem
{
    // Abstract base class for all accounts
    abstract class Account
    {
        public object Customer { get; set; }
        public decimal Balance { get; set; }
        public decimal InterestRate { get; set; }

        public abstract decimal CalculateInterest(int months);
    }

    // Subclass for deposit accounts
    class DepositAccount : Account
    {
        public override decimal CalculateInterest(int months)
        {
            if (Balance > 0 && Balance < 1000)
            {
                return 0;
            }

            return months * InterestRate;
        }
    }

    // Subclass for loan accounts
    class LoanAccount : Account
    {
        public override decimal CalculateInterest(int months)
        {
            if (Customer is Individual)
            {
                if (months <= 3)
                {
                    return 0;
                }
            }
            else if (Customer is Company)
            {
                if (months <= 2)
                {
                    return 0;
                }
            }

            return months * InterestRate;
        }
    }

    // Subclass for mortgage accounts
    class MortgageAccount : Account
    {
        public override decimal CalculateInterest(int months)
        {
            if (Customer is Individual)
            {
                if (months <= 6)
                {
                    return 0;
                }
                months -= 6;
            }
            else if (Customer is Company)
            {
                if (months <= 12)
                {
                    return months * InterestRate / 2;
                }
                months -= 12;
            }

            return months * InterestRate;
        }
    }

    // Class for the bank
    class Bank
    {
        private List<Account> accounts;

        public Bank()
        {
            accounts = new List<Account>();
        }

        public void AddAccount(Account account)
        {
            accounts.Add(account);
        }

        public void RemoveAccount(Account account)
        {
            accounts.Remove(account);
        }

        public decimal CalculateTotalInterest(int months)
        {
            decimal totalInterest = 0;
            foreach (var account in accounts)
            {
                totalInterest += account.CalculateInterest(months);
            }
            return totalInterest;
        }
    }

    // Customer class (base class for individuals and companies)
    abstract class Customer
    {
        public string Name { get; set; }
    }

    // Subclass for individual customers
    class Individual : Customer
    {
    }

    // Subclass for company customers
    class Company : Customer
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a bank instance
            var bank = new Bank();

            // Create some customers
            var individual1 = new Individual { Name = "John Doe" };
            var individual2 = new Individual { Name = "Jane Doe" };
            var company1 = new Company { Name = "ACME Corp" };
            var company2 = new Company { Name = "XYZ Inc" };

            // Create some accounts and add them to the bank
            var depositAccount1 = new DepositAccount
            {
                Customer = individual1,
                Balance = 500,
                InterestRate = 0.01m
            };
            bank.AddAccount(depositAccount1);

            var depositAccount2 = new DepositAccount
            {
                Customer = company1,
                Balance = 1000,
                InterestRate = 0.02m
            };
            bank.AddAccount(depositAccount2);

            var loanAccount1 = new LoanAccount
            {
                Customer = individual2,
                Balance = 2000,
                InterestRate = 0.03m
            };
            bank.AddAccount(loanAccount1);

            var loanAccount2 = new LoanAccount
            {
                Customer = company2,
                Balance = 3000,
                InterestRate = 0.04m
            };
            bank.AddAccount(loanAccount2);

            var mortgageAccount1 = new MortgageAccount
            {
                Customer = individual1,
                Balance = 4000,
                InterestRate = 0.05m
            };
            bank.AddAccount(mortgageAccount1);

            var mortgageAccount2 = new MortgageAccount
            {
                Customer = company1,
                Balance = 5000,
                InterestRate = 0.06m
            };
            bank.AddAccount(mortgageAccount2);

            // Calculate and print the total interest for all accounts for 6 months
            decimal totalInterest = bank.CalculateTotalInterest(6);
            Console.WriteLine($"Total interest for all accounts: {totalInterest:C}");
        }
    }
}