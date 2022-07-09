using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    static TextReader reader;
    static TextWriter writer;

    const string SUCCESS = "SUCCESS";
    const string FAIL = "FAIL";

    public static void Main()
    {
        reader = Console.In;
        writer = Console.Out;

        int n = int.Parse(reader.ReadLine());
        for (int i = 0; i < n; i++)
        {
            SolveTheTask();
        }
    }

    private static void SolveTheTask()
    {
        var carriage = new HashSet<int>();

        _ = reader.ReadLine();
        string input = reader.ReadLine();
        string[] values = input.Split(' ');

        int coupes = int.Parse(values[0]);
        int requests = int.Parse(values[1]);

        bool[] reservedCoupes = new bool[coupes];

        for (int i = 0; i < requests; i++)
        {
            input = reader.ReadLine();
            values = input.Split(' ');
            RequestType requestType = (RequestType)int.Parse(values[0]);

            if (requestType == RequestType.ReservePlace)
            {
                ReservePlace(carriage, int.Parse(values[1]), reservedCoupes);
            }
            else if (requestType == RequestType.ReleasePlace)
            {
                ReleasePlace(carriage, int.Parse(values[1]), reservedCoupes);
            }
            else if (requestType == RequestType.ReserveCoupe)
            {
                ReserveCoupe(carriage, reservedCoupes);
            }
        }

        writer.WriteLine();
    }

    enum RequestType
    {
        ReservePlace = 1,
        ReleasePlace = 2,
        ReserveCoupe = 3
    }

    private static void ReservePlace(HashSet<int> carriage, int place, bool[] reservedCoupes)
    {
        if (carriage.Contains(place))
        {
            writer.WriteLine(FAIL);
        }
        else
        {
            carriage.Add(place);
            reservedCoupes[GetCoupeIndexByPlace(place)] = true;

            writer.WriteLine(SUCCESS);
        }
    }

    private static void ReleasePlace(HashSet<int> carriage, int place, bool[] reservedCoupes)
    {
        if (carriage.Contains(place))
        {
            carriage.Remove(place);
            if (place % 2 == 0)
            {
                if (!carriage.Contains(place - 1))
                {
                    reservedCoupes[GetCoupeIndexByPlace(place - 1)] = false;
                }
            }
            else
            {
                if (!carriage.Contains(place + 1))
                {
                    reservedCoupes[GetCoupeIndexByPlace(place + 1)] = false;
                }
            }

            writer.WriteLine(SUCCESS);
        }
        else
        {
            writer.WriteLine(FAIL);
        }
    }

    private static void ReserveCoupe(HashSet<int> carriage, bool[] reservedCoupes)
    {
        int i = Array.IndexOf(reservedCoupes, false);

        if (i == -1)
        {
            writer.WriteLine(FAIL);
            return;
        }

        int place1 = i * 2 + 1;
        int place2 = place1 + 1;

        carriage.Add(place1);
        carriage.Add(place2);
        reservedCoupes[i] = true;

        writer.WriteLine($"{SUCCESS} {place1}-{place2}");
    }

    private static int GetCoupeIndexByPlace(int place)
    {
        return (place - 1) / 2;
    }
}
