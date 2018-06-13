using System;
using System.Text.RegularExpressions;
namespace LexicalAnalyzerShiftReduceParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //tells the user to enter a phrase and saves the input as a string
            Console.WriteLine("Enter a phrase:");

            string input = Console.ReadLine();
            //checks for various impossible but reasonble cases
            while (input.Contains("$") || input.Contains("-") || input.Contains("=") || input.Contains("/") || input.Contains(">") || input.Contains("<") || input.Contains(";"))
            {
                Console.WriteLine("you may not use $ in the program, re-enter");
                input = Console.ReadLine();
            }

            input = input + "$";
            //calls the lexical analysis class to analyze the input text
            lex icalAnalysis = new lex(input);
            //calls the parse class, passing the token list created during the lexical analysis to create the parse tree 
            parse tree = new parse(icalAnalysis.tokens);

        }
    }
}
