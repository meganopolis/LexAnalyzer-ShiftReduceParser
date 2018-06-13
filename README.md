# LexAnalyzer-ShiftReduceParser
This is a C# console application. Compile using a C# compiler, type the terms and hit enter to display the lexical analysis and parsing info.

This will analyze any statement set in the following grammar
E → E + T | T
T → T * F | F
F → (E) | id

For example id+id*id
