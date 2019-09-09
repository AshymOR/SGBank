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
    public class FreeAccountWithdrawRule : IWithdraw
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
            if (account.Type != AccountType.Free)
            {
                return FailedWithError(response, "Error: a non-free account hit the Free Withdraw Rule. Contact IT");
            }
            if (amount >= 0)
            {
                return FailedWithError(response, "Withdrawal amounts must be negative.");
            }
            if (amount < -100)
            {
                return FailedWithError(response, "Free accounts cannot withdraw over $100.");
            }
            if (account.Balance + amount < 0)
            {
                return FailedWithError(response, "Free accounts cannot overdraft.");
            }
            response.OldBalance = account.Balance;
            account.Balance += amount;
            response.Account = account;
            response.Amount = amount;
            response.Success = true;
            return response;
        }
    }
}