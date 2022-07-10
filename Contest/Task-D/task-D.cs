using System;
using System.IO;
using System.Text.RegularExpressions;

public class Program
{
    static TextReader reader;
    static TextWriter writer;

    public static void Main()
    {
        reader = Console.In;
        writer = Console.Out;
        
        SolveTheTask();
    }

    private static void SolveTheTask()
    {
        Regex regUpper = new Regex("[A-Z]");
        Regex regLower = new Regex("[a-z]");
        Regex regVowel = new Regex("[euioayEUIOAY]");
        Regex regConsonant = new Regex("[^euioayEUIOAY\\d]");
        Regex regDigit = new Regex("\\d");

        string input = reader.ReadLine();
        int n = int.Parse(input);

        for (int i = 0; i < n; i++)
        {
            input = reader.ReadLine();

            bool hasUpper = regUpper.IsMatch(input);
            bool hasLower = regLower.IsMatch(input);
            bool hasVowel = regVowel.IsMatch(input);
            bool hasConsonant = regConsonant.IsMatch(input);
            bool hasDigit = regDigit.IsMatch(input);

            if (!hasVowel)
            {
                if (!hasUpper)
                {
                    input += "A";
                    hasUpper = true;
                }
                else
                {
                    input += "a";
                    hasLower = true;
                }
            }

            if (!hasConsonant)
            {
                if (!hasUpper)
                {
                    input += "B";
                    hasUpper = true;
                }
                else
                {
                    input += "b";
                    hasLower = true;
                }
            }

            if (!hasUpper) input += "A";
            if (!hasLower) input += "a";
            if (!hasDigit) input += "1";

            writer.WriteLine(input);
        }
    }
}
