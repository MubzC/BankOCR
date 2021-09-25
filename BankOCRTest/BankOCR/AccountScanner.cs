using System;
using System.Collections.Generic;

namespace BankOCR
{
    // https://codingdojo.org/kata/BankOCR/
    
    public class AccountScanner
    {
        private readonly Dictionary<string, string> numberCodes = new Dictionary<string, string>
        {
            { " _ | ||_|", "0" },
            { "     |  |", "1" },
            { " _  _||_ ", "2" },
            { " _  _| _|", "3" },
            { "   |_|  |", "4" },
            { " _ |_  _|", "5" },
            { " _ |_ |_|", "6" },
            { " _   |  |", "7" },
            { " _ |_||_|", "8" },
            { " _ |_| _|", "9" },
        };

        public string CaseOneScan(string accountString)
        {
            string[] lines = accountString.Split(Environment.NewLine);
            string accountNumber = "";
            for (int i = 0; i < 9; i++)
            {
                string charToDecipher = "";
                charToDecipher += lines[1].Substring((i * 3), 3);
                charToDecipher += lines[2].Substring((i * 3), 3);
                charToDecipher += lines[3].Substring((i * 3), 3);
                accountNumber += numberCodes[charToDecipher];
            }
            return accountNumber;
        }

        public bool CaseTwoScan(string accountNumber)
        {
            char[] charArray = accountNumber.ToCharArray();
            Array.Reverse(charArray);
            string reverseAccountNumber = new string (charArray);
            int checksumTotal = 0;
            for(int i = 0; i< 9; i++)
            {
                checksumTotal += (int.Parse(reverseAccountNumber.Substring(i, 1)) * (i+1));
            }
            return (checksumTotal % 11 == 0);
        }

        public string CaseThreeScan(string accountString)
        {
            string[] lines = accountString.Split(Environment.NewLine);
            string accountNumber = "";
            bool incorrectCharDetected = false;
            for (int i = 0; i < 9; i++)
            {
                string charToDecipher = "";
                charToDecipher += lines[1].Substring((i * 3), 3);
                charToDecipher += lines[2].Substring((i * 3), 3);
                charToDecipher += lines[3].Substring((i * 3), 3);
                if (numberCodes.ContainsKey(charToDecipher))
                {
                    accountNumber += numberCodes[charToDecipher];
                }
                else
                {
                    incorrectCharDetected = true;
                    accountNumber += "?";
                }
            }
            if(incorrectCharDetected)
            {
                return accountNumber + " ILL";
            }
            else
            {
                if (CaseTwoScan(accountNumber))
                {
                    return accountNumber;
                }
                else
                {
                    return accountNumber + " ERR";
                }
            }
        }
    }
}
