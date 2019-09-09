using NUnit.Framework;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.Tests
{
    [TestFixture]
    public class PremiumAccountTests
    {
        [TestCase("66666", "Premium Account", 100, AccountType.Free, 250, false)] //Wrong account type
        [TestCase("66666", "Premium Account", 100, AccountType.Premium, -100, false)] // Negative number deposited
        [TestCase("66666", "Premium Account", 100, AccountType.Premium, 250, true)] // Success
        public void PremiumAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IDeposit deposit = new NoLimitDepositRule();
            Account account = new Account()
            {
                AccountNumber = accountNumber,
                Name = name,
                Balance = balance,
                Type = accountType
            };
            AccountDepositResponse response = deposit.Deposit(account, amount);
            Assert.AreEqual(expectedResult, response.Success);
        }



        [TestCase("66666", "Premium Account", 100, AccountType.Basic, -100, 100, false)] // Wrong account type
        [TestCase("66666", "Premium Account", 100, AccountType.Premium, 100, 100, false)] // Positive amount withdrawn
        [TestCase("66666", "Premium Account", 100, AccountType.Premium, -650, 100, false)] // Overdrafted too far
        [TestCase("66666", "Premium Account", 150, AccountType.Premium, -50, 100, true)] //Success, no overdraft
        [TestCase("66666", "Premium Account", 100, AccountType.Premium, -350, -250, true)] //Success, no fee for overdraft
        public void PremiumAccountWithdrawRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, decimal newBalance, bool expectedResult)
        {
            IWithdraw withdraw = new PremiumAccountWithdrawRule();
            Account account = new Account()
            {
                AccountNumber = accountNumber,
                Name = name,
                Balance = balance,
                Type = accountType,
            };
            AccountWithdrawResponse response = withdraw.Withdraw(account, amount);
            Assert.AreEqual(expectedResult, response.Success);
            Assert.AreEqual(newBalance, account.Balance);
        }



    }
}