using System;
using System.Collections.Generic;
namespace LexicalAnalyzerShiftReduceParser
{
    public class lex
    {
        //private variables for use in the class.
        string input;
        string lexeme = "";
        char[] inputChar;
        int position = 0;

        //lists to hold the lexemes and tokens to print and for use in the parser
        List<string> lexemes = new List<string>();
        public List<string> tokens = new List<string>();


        //Passes the user input into this class, sets the private variable equal to it, and calls the method that does the lexical analysis, and finally prints!
        public lex(String userString)
        {
            input = userString;
            inputChar = userString.ToCharArray();
            LexAnalyzer(inputChar);
            Print();

        }
        //method that analyzes the inputted phrase
        public void LexAnalyzer(char[] inputC)

        {
            //runs until the end of the string (denoted by $)
            while (inputC[position] != '$')
            {
                //if the character is a digit continue until the end of the digits
                if (Char.IsDigit(inputC[position]))
                {
                    //while the character is a digit add the digit to the individual lexeme string (to add to the total list of lexemes when complete)
                    //increment position in order to move across the input array
                    while (Char.IsDigit(inputC[position]))
                    {
                        lexeme = lexeme + inputC[position];
                        position++;
                    }
                    // add the ident code to the token list, the list of digits to the lexemes list and clear the lexeme string.
                    tokens.Add("IDENT");
                    lexemes.Add(lexeme);
                    lexeme = "";
                }
                //if the character is a letter this overall statement checks until a non letter is reached.
                if (Char.IsLetter(inputC[position]))
                {
                    //while the character is a letter add the letter to the individual lexeme string (to add to the total list of lexemes when complete)
                    //increment position in order to move across the input array
                    while (Char.IsLetter(inputC[position]))
                    {
                        lexeme = lexeme + inputC[position];
                        position++;
                    }
                    // add the ident code to the token list, the list of letters to the lexemes list and clear the lexeme string.
                    tokens.Add("IDENT");
                    lexemes.Add(lexeme);
                    lexeme = "";
                }

                //all of these if statements account for cases where each input is meant to be seperate (i.e. "++" is 2 tokens, not 1)
                if (inputC[position] == '+')
                {

                    tokens.Add("ADD_OP");
                    lexemes.Add("+");

                    position++;
                }
                if (inputC[position] == '*')
                {
                    lexemes.Add("*");
                    tokens.Add("MULT_OP");


                    position++;
                }
                if (inputC[position] == '(')
                {
                    lexemes.Add("(");
                    tokens.Add("LEFT_PARAN");
                    position++;
                }
                if (inputC[position] == ')')
                {
                    lexemes.Add(")");
                    tokens.Add("RIGHT_PARAN");
                    position++;
                }
                //ensures that whitespace is not added as a token 
                if (Char.IsWhiteSpace(inputC[position]))
                    position++;
            }

        }

        //prints the chart of tokens and lexemes
        public void Print()
        {
            Console.WriteLine("lexeme    |    token");
            Console.WriteLine("-----------------------");
            Console.WriteLine();
            for (int i = 0; i < lexemes.Count; i++)
            {
                Console.WriteLine("{0, -9} | {1, -9}", lexemes[i], tokens[i]);
            }

        }

    }
}
