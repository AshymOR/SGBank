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
    public class BasicAccountTests
    {
        [TestCase("33333", "Basic Account", 100, AccountType.Free, 250, false)] //Wrong account type
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, -100, false)] // Negative number deposited
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, 250, true)] // Success
        public void BasicAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
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


        [TestCase("33333", "Basic Account", 1500, AccountType.Basic, -1000, 1500, false)] // Too much withdrawn
        [TestCase("33333", "Basic Account", 100, AccountType.Free, -100, 100, false)] // Wrong account type
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, 100, 100, false)] // Positive amount withdrawn
        [TestCase("33333", "Basic Account", -50, AccountType.Basic, -60, -50, false)] // Overdraft too far
        [TestCase("33333", "Basic Account", 150, AccountType.Basic, -50, 100, true)] //Success, no overdraft
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, -150, -60, true)] //Success, $10 overdraft fee
        public void BasicAccountWithdrawRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, decimal newBalance, bool expectedResult)
        {
            IWithdraw withdraw = new BasicAccountWithdrawRule();
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