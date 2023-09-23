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

        int n = int.Parse(reader.ReadLine());
        for (int i = 0; i < n; i++)
        {
            SolveTheTask();
        }
    }

    private static void SolveTheTask()
    {
        Dictionary<string, Module> dict = new();

        _ = reader.ReadLine();
        int n = int.Parse(reader.ReadLine());
        for (int i = 0; i < n; i++)
        {
            ReadModule(dict);
        }

        int q = int.Parse(reader.ReadLine());
        for (int i = 0; i < q; i++)
        {
            string input = reader.ReadLine();
            Module module = dict[input];

            List<Module> loaded = module.LoadDependencies();
            if (loaded.Count != 0)
            {
                writer.WriteLine($"{loaded.Count} {String.Join(' ', loaded)}");
            }
            else
            {
                writer.WriteLine("0");
            }
        }

        writer.WriteLine();
    }

    class Module
    {
        public string Name { get; }
        public List<Module> Dependencies { get; } = new List<Module>();
        public bool IsLoaded;

        public Module(string name)
        {
            Name = name;
        }

        public List<Module> LoadDependencies()
        {
            List<Module> loadedDependencies = new();

            if (IsLoaded)
                return loadedDependencies;

            if (Dependencies.Count == 0)
            {
                IsLoaded = true;
                loadedDependencies.Add(this);
                return loadedDependencies;
            }

            foreach (Module module in Dependencies)
            {
                if (module.IsLoaded)
                    continue;

                List<Module> nestedDependencies = module.LoadDependencies();

                foreach (Module nestedDependency in nestedDependencies)
                {
                    loadedDependencies.Add(nestedDependency);
                }
            }

            IsLoaded = true;
            loadedDependencies.Add(this);

            return loadedDependencies;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    static private void ReadModule(Dictionary<string, Module> dict)
    {
        string input = reader.ReadLine();
        string[] values = input.Split(' ');

        string name = values[0].Replace(":", "");
        Module module = dict.GetValueOrDefault(name);

        if (module == null)
        {
            module = new Module(name);
            dict.Add(name, module);
        }

        for (int i = 1; i < values.Length; i++)
        {
            string nestedName = values[i];
            Module nestedModule = dict.GetValueOrDefault(nestedName);

            if (nestedModule == null)
            {
                nestedModule = new Module(nestedName);
                dict.Add(nestedName, nestedModule);
            }

            module.Dependencies.Add(nestedModule);
        }
    }
}
