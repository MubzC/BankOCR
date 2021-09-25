using System;
using System.Collections.Generic;

namespace BankOCR
{
    // https://codingdojo.org/kata/BankOCR/
    
    public class Program
    {
        private static readonly Dictionary<string, string> numberCodes = new Dictionary<string, string>
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

        public static void Main(string[] args)
        {
            string accountString = @"
                           
  |  |  |  |  |  |  |  |  |
  |  |  |  |  |  |  |  |  |";

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
            Console.WriteLine(accountNumber);
        }
    }
}
