using SGBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Models;
using System.IO;

namespace SGBank.Data
{
    public class FileAccountRepository : IAccountRepository
    {
        private const string FilePath = @"C:\Users\Ashym\Documents\repos\SGBank\Accounts.txt";

        public List<Account> GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();

            var reader = File.ReadAllLines(FilePath);

            for (int i = 1; i < reader.Length; i++)
            {
                var columns = reader[i].Split(',');

                var account = new Account();

                account.AccountNumber = columns[0];
                account.Name = columns[1];
                account.Balance = decimal.Parse(columns[2]);
                switch (columns[3])
                {
                    case "F":
                        account.Type = AccountType.Free;
                        break;
                    case "B":
                        account.Type = AccountType.Basic;
                        break;
                    case "P":
                        account.Type = AccountType.Premium;
                        break;

                }

                accounts.Add(account);
            }

            return accounts;
        }

        public Account LoadAccount(string AccountNumber)
        {
            List<Account> accounts = GetAllAccounts();
            return accounts.FirstOrDefault(a => a.AccountNumber == AccountNumber);
        }

        public void SaveAccount(Account account)
        {
            var accounts = GetAllAccounts();

            var existingAccount = accounts.First(a => a.AccountNumber == account.AccountNumber);
            existingAccount.Name = account.Name;
            existingAccount.Balance = account.Balance;
            existingAccount.Type = account.Type;

            OverwriteFile(accounts);
        }

        private void OverwriteFile(List<Account> accounts)
        {
            File.Delete(FilePath);

            using (var writer = File.CreateText(FilePath))
            {
                writer.WriteLine("AccountNumber,Name,Balance,Type");

                foreach (var account in accounts)
                {
                    switch (account.Type)
                    {
                        //repeated code 
                        case AccountType.Free:
                            writer.WriteLine("{0},{1},{2},{3}", account.AccountNumber, account.Name, account.Balance, "F");
                            break;
                        case AccountType.Basic:
                            writer.WriteLine("{0},{1},{2},{3}", account.AccountNumber, account.Name, account.Balance, "B");
                            break;
                        case AccountType.Premium:
                            writer.WriteLine("{0},{1},{2},{3}", account.AccountNumber, account.Name, account.Balance, "P");
                            break;
                    }



                }
            }
        }
    }
}