using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContestConsoleApp
{
    internal class Program
    {
        static void Main()
        {
            var reader = Console.In;
            var writer = Console.Out;
            var executor = new Executor(reader, writer);
            executor.Execute();
        }
    }

    public class Executor
    {
        TextReader _reader;
        TextWriter _writer;

        public Executor(TextReader reader, TextWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public void Execute()
        {
            int caseCount = _reader.ReadInt();

            var result = new CategoryProcessed[caseCount];
            for (int i = 0; i < caseCount; i++)
                result[i] = ProcessCase();

            string json = JsonSerializer.Serialize<CategoryProcessed[]>(result, new JsonSerializerOptions { MaxDepth = 4000 });
            _writer.WriteLine(json);
        }

        CategoryProcessed ProcessCase()
        {
            int count = _reader.ReadInt();
            var sb = new StringBuilder();

            for (int i = 0; i < count; i++)
                sb.AppendLine(_reader.ReadLine());

            var categories = JsonSerializer.Deserialize<CategoryOriginal[]>(sb.ToString(), new JsonSerializerOptions { MaxDepth = 4000 });
            var dict = categories.ToDictionary(c => c.Id);

            var node = ProcessCategory(categories.Single(c => c.ParentId == -1), dict);

            return node;
        }

        CategoryProcessed ProcessCategory(CategoryOriginal category, Dictionary<int, CategoryOriginal> dict)
        {
            var node = new CategoryProcessed { Id = category.Id, Name = category.Name };

            foreach (var cat in dict.Values.Where(x => x.ParentId == category.Id))
            {
                node.Children.Add(ProcessCategory(cat, dict));
            }

            return node;
        }
    }

    class CategoryOriginal
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("parent")]
        public int ParentId { get; init; } = -1;

        [JsonIgnore]
        public CategoryOriginal Parent { get; set; }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is CategoryOriginal other && other.Id == Id;
        }
    }

    class CategoryProcessed
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("next")]
        public List<CategoryProcessed> Children { get; init; } = new();
    }

    internal static class Extensions
    {
        internal static int ReadInt(this TextReader reader)
        {
            return int.Parse(reader.ReadLine());
        }
    }
}
