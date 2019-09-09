
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.BLL.WithdrawRules
{
    public class BasicAccountWithdrawRule : IWithdraw
    {
        private static AccountWithdrawResponse FailedWithError(AccountWithdrawResponse r, string message)
        {
            r.Success = false;
            r.Message = message;
            return r;
        }

        public AccountWithdrawResponse Withdraw(Account account, decimal amount)
        {
            AccountWithdrawResponse response = new AccountWithdrawResponse();
            if (account.Type != AccountType.Basic)
            {
                return FailedWithError(response, "Error: a non-basic account hit the Basic Withdraw Rule. Contact IT");
            }
            if (amount >= 0)
            {
                return FailedWithError(response, "Withdrawal amounts must be negative.");
            }
            if (amount < -500)
            {
                return FailedWithError(response, "Basic accounts cannot withdraw more than $500");
            }
            if (account.Balance + amount < -100)
            {
                return FailedWithError(response, "This goes over your overdraft limit");
            }
            response.OldBalance = account.Balance;
            account.Balance += amount;
            if (account.Balance < 0)
            {
                account.Balance -= 10;
            }
            response.Account = account;
            response.Amount = amount;
            response.Success = true;
            return response;
        }
    }
}