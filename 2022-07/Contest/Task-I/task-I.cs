using System;
using System.Collections.Generic;
using System.IO;

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
        string input = reader.ReadLine();
        string[] values = input.Split(' ');

        int procCount = int.Parse(values[0]);
        int taskCount = int.Parse(values[1]);

        Proc[] procs = new Proc[procCount];

        input = reader.ReadLine();
        values = input.Split(' ');

        for (int i = 0; i < values.Length; i++)
        {
            procs[i] = new Proc { Number = i + 1, Energy = int.Parse(values[i]) };
        }

        var free = new PriorityQueue<Proc, int>(procs.Select(x => (x, x.Energy)));
        var used = new PriorityQueue<Proc, int>();

        ulong result = 0;
        for (int i = 0; i < taskCount; i++)
        {
            input = reader.ReadLine();
            values = input.Split(' ');

            int taskTick = int.Parse(values[0]);
            int taskWork = int.Parse(values[1]);

            while ((used.Count != 0) && (used.Peek().Free <= taskTick))
            {
                Proc freed = used.Dequeue();
                free.Enqueue(freed, freed.Energy);
            }

            if (free.Count == 0)
                continue;

            Proc proc = free.Dequeue();
            proc.Free = taskTick + taskWork;
            used.Enqueue(proc, proc.Free);

            result += (ulong)taskWork * (ulong)proc.Energy;
        }

        writer.WriteLine(result);
    }

    class Proc
    {
        public int Number { get; set; }
        public int Energy { get; set; }
        public int Free { get; set; }
    }
}
