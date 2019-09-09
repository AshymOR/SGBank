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
    public class PremiumAccountWithdrawRule : IWithdraw
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
            if (account.Type != AccountType.Premium)
            {
                return FailedWithError(response, "Error: a non-premium account hit the Premium Withdraw Rule. Contact IT");
            }
            if (amount >= 0)
            {
                return FailedWithError(response, "Withdrawal amounts must be negative!");
            }
            if (account.Balance + amount < -500)
            {
                return FailedWithError(response, "This amount will overdraft more than your $500 limit!");
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