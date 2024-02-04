using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var filters = ProcessCase();

            _writer.WriteLine(filters.Sum(filter => filter.Excess));
            _writer.WriteLine(filters.Length);

            foreach (var filter in filters.OrderBy(f => f.IpAddress.Segments[2]))
                _writer.WriteLine(filter);
        }

        IPFilter[] ProcessCase()
        {
            var ints = _reader.ReadInts();
            int ipCount = ints[0];
            int filterCount = ints[1];
            var ips = new IPAddress[ipCount];

            for (int i = 0; i < ipCount; i++)
                ips[i] = new IPAddress(_reader.ReadLine());

            if (ips.Length <= filterCount)
            {
                return ips.Select(ip => new IPFilter(ip, IPFilterType.Specific, 1))
                    .ToArray();
            }

            var groups = ips.GroupBy(ip => ip.Segments[2]);
            if (groups.Count() > filterCount)
            {
                var ip = new IPAddress(ips[0].Segments[0], ips[0].Segments[1], 0, 0);
                return new IPFilter[] { new IPFilter(ip, IPFilterType.All, ips.Length) };
            }

            var list = new List<IPFilter>();
            var order = groups.OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key);

            int toBlock = ips.Length;
            foreach (var group in order)
            {
                if (filterCount >= toBlock)
                {
                    foreach (var ip in group)
                        list.Add(new IPFilter(ip, IPFilterType.Specific, 1));
                }
                else
                {
                    var count = group.Count();
                    var ip = group.First();
                    var filterIp = new IPAddress(ip.Segments[0], ip.Segments[1], ip.Segments[2], 0);
                    list.Add(new IPFilter(filterIp, IPFilterType.Subnet, count));
                    toBlock -= count;
                    filterCount--;
                }
            }

            return list.ToArray();
        }
    }

    class IPAddress
    {
        public readonly int[] Segments;

        public IPAddress(string line)
        {
            Segments = line.Split('.')
                .Select(x => int.Parse(x))
                .ToArray();

            if (Segments.Length != 4)
                throw new FormatException(nameof(line));
        }

        public IPAddress(params int[] segments)
        {
            if (segments.Length != 4)
                throw new FormatException(nameof(segments));

            Segments = segments;
        }

        public override string ToString()
        {
            return string.Join('.', Segments);
        }
    }

    class IPFilter
    {
        public readonly IPAddress IpAddress;
        public readonly IPFilterType Type;
        public readonly int Blocked;

        public IPFilter(IPAddress ipAddress, IPFilterType type, int blocked)
        {
            IpAddress = ipAddress;
            Type = type;
            Blocked = blocked;
        }

        public int Excess => Type switch
        {
            IPFilterType.Specific => 0,
            IPFilterType.Subnet => Constants.BlockedBySubnetCount - Blocked,
            IPFilterType.All => Constants.BlockedAllCount - Blocked,
            _ => throw new ArgumentException(nameof(Type))
        };

        public override string ToString()
        {
            return IpAddress.ToString()
                + (Type == 0
                    ? string.Empty
                    : "/" + ((int)Type).ToString());
        }
    }

    enum IPFilterType
    {
        Specific = 0,
        Subnet = 24,
        All = 16
    }

    static class Constants
    {
        public const int BlockedBySubnetCount = 256;
        public const int BlockedAllCount = 65536;
    }

    internal static class Extensions
    {
        internal static int[] ReadInts(this TextReader reader)
        {
            return reader.ReadLine()
                .Split(' ')
                .Select(x => int.Parse(x))
                .ToArray();
        }
    }
}
