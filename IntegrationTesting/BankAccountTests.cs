using System;
using DevOpsDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTesting
{
    [TestClass]
    public class BankAccountTests
    {
        [TestMethod]
        public void Credit_Debit_WithValidAmount_UpdatesBalance()
        {
            // arrange  
            double beginningBalance = 11.99;
            double creditAmount = 4.01;
            double debitAmount = 4.00;
            double expected = 12.00;
            BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

            // act  
            account.Credit(creditAmount);
            account.Debit(debitAmount);

            // assert  
            double actual = account.Balance;
            Assert.AreEqual(expected, actual, 0.001, "Account not credited/debited correctly");
        }
    }
}
