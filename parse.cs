using System;
using System.Collections.Generic;
namespace LexicalAnalyzerShiftReduceParser
{
    public class parse
    {
        //list of tokens
        List<string> tokenList = new List<string>();
        //list of tokens in the format of the chart
        List<int> chartList = new List<int>();
        //list of steps to print
        List<string> steps = new List<string>();
        //list of the stack steps to print
        List<string> stackDisplay = new List<string>();
        string savedToken;
        int charPosition;
        int temp;
        string printS;
        int direction;
        Stack<string> sta = new Stack<string>();


        //the action table for use in parsing.
        // a positive number corresponds to a shift and a negative corresponds to a reduction.
        int[,] actionTable = new int[,] { {5,0,0,4,0,0},
                                            {0,6,0,0,0,100},
                                            {0,-2,7,0,-2,-2},
                                            {0,-4,-4,0,-4,-4},
                                            {5,0,0,4,0,0},
                                            {0,-6,-6,0,-6,-6},
                                            {5,0,0,4,0,0},
                                            {5,0,0,4,0,0},
                                            {0,6,0,0,11,0},
                                            {0,-1,7,0,-1,-1},
                                            {0,-3,-3,0,-3,-3},
                                            {0,-5,-5,0,-5,-5}};

        //the goto table for use in parsing
        int[,] goToTable = new int[,] { {1, 2, 3 },
                                            { 0, 0, 0},
                                            { 0, 0, 0},
                                            { 0, 0, 0},
                                            { 8, 2, 3},
                                            { 0, 0, 0},
                                            { 0, 9, 3},
                                            { 0, 0, 10},
                                            { 0, 0, 0},
                                            { 0, 0, 0},
                                            { 0, 0, 0},
                                            { 0, 0, 0}
                                                     };
        //constructor 
        public parse(List<string> list)
        {
            //adds a $ to the token list in order to denote the end
            tokenList = list;
            tokenList.Add("$");
            //converts the tokens to an integer corresponding to the correct column in the actionTable
            foreach (string token in tokenList)
            {
                if (token == "IDENT")
                    chartList.Add(0);
                else if (token == "ADD_OP")
                    chartList.Add(1);
                else if (token == "MULT_OP")
                    chartList.Add(2);
                else if (token == "RIGHT_PARAN")
                    chartList.Add(4);
                else if (token == "LEFT_PARAN")
                    chartList.Add(3);
                else if (token == "$")
                    chartList.Add(5);
            }
            //pushes 0 to the stack and print lists to ensure it is displayed
            steps.Add("0");
            stackDisplay.Add("0");
            sta.Push("0");
            //parses the list
            Parsing(tokenList);
            Print2();




        }

        void Parsing(List<string> l)
        {
            //loop runs endlessly until an error is found or an accept is reached
            int r = 1;
            while (r > 0)
            {
                // parses the top item in the stack and saves it as a temp value 
                temp = Int32.Parse(sta.Peek());
                //direction is equal to the point at action table between the temp (top item of the stack) and the integer corresponding to the item not yet pushed.
                direction = actionTable[temp, chartList[charPosition]];
                //if direction is negative, it corresponds to a reduce 
                if (direction < 0)
                {
                    // calls step calc to print the step, pops the top of the stack and saves the new top as a variable, top, for comparison
                    StepCalc(direction);
                    sta.Pop();
                    string top = sta.Peek();

                    //if the top of the stack is an id pop it, read the integer from the top of the stack, push F since it is the corresponding item in the grammar table, then push the correct number from the goto table in order to find the next direction
                    if (top == "id")
                    {
                        sta.Pop();
                        temp = Int32.Parse(sta.Peek());
                        sta.Push("F");
                        sta.Push(goToTable[temp, 2].ToString());


                    }
                    //if the token has already been converted to an F, according to the grammar table, we need to check if it is T*F or just F 
                    else if (top == "F")
                    {
                        //pops the top and checks to see if the stack is greater than 1, if so save the top of the stack before poping it since it is uncertain what type of T this will reduce to (F or T*F).
                        sta.Pop();
                        if (sta.Count > 1)
                            savedToken = sta.Pop();
                        top = sta.Peek();
                        //check to see if there is a *
                        if (top == "*")
                        {
                            sta.Pop();
                            PrintStack();
                            continue;
                        }
                        //when there is no * at the top, then it can be reduced
                        else
                        {
                            //push the item saved earlier, reduce to T and push the new item from the goto table
                            if (sta.Count > 1)
                                sta.Push(savedToken);
                            temp = Int32.Parse(sta.Peek());
                            sta.Push("T");
                            sta.Push(goToTable[temp, 1].ToString());

                        }
                    }
                    //checks to see if the item containing ( also contains a ) if it doesn't then there is an error.
                    else if (top == "(")
                    {
                        if (!chartList.Contains(4))
                        {
                            StepCalc(0);
                            return;
                        }

                    }
                    //looks for the ( if a ) is at the top of the stack 
                    else if (top == ")")
                    {
                        sta.Pop();
                        string current = sta.Peek();
                        while (current != "(")
                        {
                            sta.Pop();
                            current = sta.Peek();

                        }
                        //if they both are there, reduce
                        sta.Pop();
                        temp = Int32.Parse(sta.Peek());
                        sta.Push("F");
                        sta.Push(goToTable[temp, 2].ToString());
                    }
                    //similar to F above but with addition 
                    else if (top == "T")
                    {
                        sta.Pop();
                        if (sta.Count > 1)
                            savedToken = sta.Pop();
                        top = sta.Peek();
                        if (top == "+")
                        {
                            sta.Pop();
                            PrintStack();
                            continue;
                        }
                        else
                        {
                            if (sta.Count > 1)
                                sta.Push(savedToken);
                            temp = Int32.Parse(sta.Peek());
                            sta.Push("E");
                            sta.Push(goToTable[temp, 0].ToString());

                        }
                    }
                }
                //calculates a shift 
                else if (direction > 0 && direction != 100)
                {
                    //prints the steps, pushes the next number from the actiontable and the direction, as well as incrementing the position in the list of numerical tokens
                    StepCalc(direction);
                    sta.Push(DisplayHelper(chartList[charPosition]));
                    sta.Push(direction.ToString());
                    charPosition++;
                }
                //if accept
                else if (direction == 100)
                {
                    StepCalc(100);
                    return;
                }
                //if error
                else if (direction == 0)
                {
                    StepCalc(0);
                    return;
                }
                PrintStack();

            }





        }

        //method to populate the step list for later printing
        void StepCalc(int num)
        {
            if (num < 0)
                steps.Add("R" + (num * (-1)));
            else if (num > 0 && num != 100)
                steps.Add("S" + num);
            else if (num == 0)
                steps.Add("ERROR");
            else if (num == 100)
                steps.Add("ACCEPT");


        }
        //method to help with printing the stack
        string DisplayHelper(int i)
        {
            int helper = i;
            if (helper == 0)
                return "id";
            else if (helper == 1)
                return "+";
            else if (helper == 2)
                return "*";
            else if (helper == 3)
                return "(";
            else if (helper == 4)
                return ")";
            else if (helper == 5)
                return "$";
            else
                return "";
        }
        //the print method 
        void Print2()
        {
            Console.WriteLine("\nSteps:");
            for (int i = 0; i < steps.Count; i++)
                Console.WriteLine(steps[i]);
            Console.WriteLine("\n Stack:");
            for (int j = 0; j < stackDisplay.Count; j++)
                Console.WriteLine(stackDisplay[j]);
        }
        //populates the stackdisplay with items
        void PrintStack()
        {
            printS = "";
            foreach (string str in sta)
            {
                printS = str + printS;
            }
            stackDisplay.Add(printS);
        }
    }

}



