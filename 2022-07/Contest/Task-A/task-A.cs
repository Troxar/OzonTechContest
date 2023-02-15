using System;
using System.IO;

public class Program
{
    static TextWriter writer;

    public static void Main()
    {
        writer = Console.Out;
        
        SolveTheTask();
    }

    private static void SolveTheTask()
    {
        writer.WriteLine("I am sure that I will fill out the form by 11:00 am on July 4, 2022.");
    }
}
