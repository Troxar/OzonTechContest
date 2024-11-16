using System;
using System.IO;
using var input = new StreamReader(Console.OpenStandardInput());
using var output = new StreamWriter(Console.OpenStandardOutput());

var testCount = int.Parse(input.ReadLine());

for (var testIndex = 0; testIndex < testCount; testIndex++)
{
    var str = input.ReadLine();
    var result = GetResult(str);
    output.WriteLine(result);
}

string GetResult(string str)
{
    if (str.Length <= 1)
        return "0";
    
    
    for (int i = 0; i < str.Length - 1; i++)
    {
        if (str[i + 1] > str[i])
            return str.Remove(i, 1);
    }
    
    return str.Remove(str.Length - 1, 1);
}
