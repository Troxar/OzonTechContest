using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        int n = int.Parse(values[0]);
        int m = int.Parse(values[1]);

        Dictionary<int, User> users = new();
        for (int i = 0; i < m; i++)
        {
            input = reader.ReadLine();
            values = input.Split(' ');

            int id1 = int.Parse(values[0]);
            int id2 = int.Parse(values[1]);

            User user1 = users.GetValueOrDefault(id1);
            if (user1 == null)
            {
                user1 = new User { Number = id1 };
                users.Add(id1, user1);
            }

            User user2 = users.GetValueOrDefault(id2);
            if (user2 == null)
            {
                user2 = new User { Number = id2 };
                users.Add(id2, user2);
            }

            user1.Friends.Add(user2);
            user2.Friends.Add(user1);
        }

        for (int i = 1; i <= n; i++)
        {
            User user = users.GetValueOrDefault(i);
            if (user == null)
            {
                writer.WriteLine("0");
                continue;
            }

            Dictionary<int, int> ids = new();
            int max = 1;

            foreach (User friend in user.Friends)
            {
                foreach (User ff in friend.Friends)
                {
                    if ((ff != user) && (!user.Friends.Contains(ff)))
                    {
                        if (ids.ContainsKey(ff.Number))
                        {
                            ids[ff.Number]++;
                            max = Math.Max(max, ids[ff.Number]);
                        }
                        else
                        {
                            ids.Add(ff.Number, 1);
                        }
                    }
                }
            }

            if (ids.Count == 0)
            {
                writer.WriteLine("0");
            }
            else
            {
                writer.WriteLine(String.Join(' ', ids.Where(x => x.Value == max)
                                                     .OrderBy(x => x.Key)
                                                     .Select(x => x.Key)));
            }
        }
    }

    class User
    {
        public int Number { get; set; }
        public List<User> Friends { get; set; } = new();
    }
}
