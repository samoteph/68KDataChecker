This tool addresses one of the shortcomings of the 68000 assembler.

When you declare a variable of byte type (.b) in your code, for example, `toto: dc.b 1` or `titi: ds.b 2`, you then access it in 99.9% of cases with the same type for example, `move.b #3,toto`. 
Unfortunately, sometimes the type is used incorrectly, and you can write `move.w #3,toto` when `toto` is declared as `.b`. 

In this case, the value will be written to a different memory location, which can cause, at best, a crash, and at worst, nothing but occasional inconsistent behavior.
These inconsistencies are often bugs that are difficult to reproduce and are best avoided.

To overcome this problem, I wrote a very simple tool that scans all the assembly language files in my project, retrieves the declared data along with its type, and then analyzes the instructions that use this data and compares their types. If the types are different, it's likely a potential bug. The tool isn't very powerful in its parsing, but it allowed me to quickly find eight potential bugs that would have been very complex to fix.

The tool is written in C# .Net Core 8.0.
You can download the tool here => https://github.com/samoteph/68KDataChecker/blob/master/bin/Release/net8.0/68KDataChecker.exe

<img width="1482" height="762" alt="image" src="https://github.com/user-attachments/assets/708b2ef7-0421-4caa-8b99-ee609094bdcb" />
