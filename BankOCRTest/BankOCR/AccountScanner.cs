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

        public bool ChecksumCheck(string accountNumber)
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
                if (ChecksumCheck(accountNumber))
                {
                    return accountNumber;
                }
                else
                {
                    return accountNumber + " ERR";
                }
            }
        }

        public string CaseFourScan(string accountString)
        {
            // Convert the OCR into a string
            string[] lines = accountString.Split(Environment.NewLine);
            string accountNumber = "";
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
                    accountNumber += GuessTheChar(charToDecipher);
                }
            }

            if(!(accountNumber.Contains("?")))
            {
                if(ChecksumCheck(accountNumber))
                {
                    // No incorrect characters detected and we pass the checksum so we are good!
                    return accountNumber;
                }
                //We need to get the correct checksum possibilities by changing one char at a time. 
                List<string> potentialAccountNumbers = GeneratePotentialAccounts(accountNumber);
                List<string> actualAccountNumbers = new List<string>();
                foreach(string pAccountNumber in potentialAccountNumbers)
                {
                    if(ChecksumCheck(pAccountNumber))
                    {
                        actualAccountNumbers.Add(pAccountNumber);
                    }
                }
                if (actualAccountNumbers.Count > 1)
                {
                    string listOfAccounts = "[";
                    foreach(string s in actualAccountNumbers)
                    {
                        listOfAccounts += "'" + s + "'" + ", ";
                    }
                    listOfAccounts = listOfAccounts.Remove(listOfAccounts.LastIndexOf(", "), 2);
                    listOfAccounts += "]";
                    return accountNumber + " AMB " + listOfAccounts;
                }
                else if (actualAccountNumbers.Count == 1)
                {
                    return actualAccountNumbers.ToArray()[0];
                }
                else
                {
                    return accountNumber + " ERR";
                }
            }
            else
            {
                // We can't match the char. Return with question marks
                return accountNumber + " ILL";
            }
        }

        private List<string> GeneratePotentialAccounts(string accountNumber)
        {
            //Rules I am following - 
            //0 can be 8
            //1 can be 7
            //2 can be 3
            //3 can be 9
            //4 can only be 4
            //5 can be 6 or 9
            //6 can be 5 or 8
            //7 can be 1
            //8 can be 9 or 0 or 6
            //9 can be 8 or 3 or 5
            List<string> potentialAccountNumbers = new List<string>();

            for(int i = 0; i < accountNumber.Length; i++)
            {
                switch (accountNumber.Substring(i, 1))
                {
                    case "1":
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "7"));
                        break;
                    case "2":
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "3"));
                        break;
                    case "3":
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "9"));
                        break;
                    case "4":
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "1"));
                        break;
                    case "5":
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "6"));
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "9"));
                        break;
                    case "6":
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "5"));
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "8"));
                        break;
                    case "7":
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "1"));
                        break;
                    case "8":
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "9"));
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "0"));
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "6"));
                        break;
                    case "9":
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "8"));
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "3"));
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "5"));
                        break;
                    case "0":
                        potentialAccountNumbers.Add(accountNumber.Remove(i, 1).Insert(i, "8"));
                        break;
                    default:
                        break;
                }
            }
            potentialAccountNumbers.Sort();
            return potentialAccountNumbers;
        }

        /// <summary>
        /// I am aware this is a brute force method to get a matching char
        /// </summary>
        /// <param name="charToDecipher"></param>
        /// <returns></returns>
        public string GuessTheChar(string charToDecipher)
        {
            string newChar = "";
            for (int i = 0; i < charToDecipher.Length; i++)
            {
                if(numberCodes.ContainsKey(charToDecipher.Remove(i, 1).Insert(i, " ")))
                {
                    newChar = numberCodes[charToDecipher.Remove(i, 1).Insert(i, " ")];
                }
                if (numberCodes.ContainsKey(charToDecipher.Remove(i, 1).Insert(i, "_")))
                {
                    newChar = numberCodes[charToDecipher.Remove(i, 1).Insert(i, "_")];
                }
                if (numberCodes.ContainsKey(charToDecipher.Remove(i, 1).Insert(i, "|")))
                {
                    newChar = numberCodes[charToDecipher.Remove(i, 1).Insert(i, "|")];
                }
                if(!string.IsNullOrEmpty(newChar))
                {
                    break;
                }
            }
            if(string.IsNullOrEmpty(newChar))
            {
                return "?";
            }
            return newChar;
        }
    }
}
