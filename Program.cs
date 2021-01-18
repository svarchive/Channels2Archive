using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channels2Archive
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Input target directory: ");
                string target = Console.ReadLine() + @"\";
                Console.WriteLine("Input github static page directory: ");
                string gitdir = Console.ReadLine();
                if (gitdir.Length != 0) gitdir += "/";
                Dictionary<string, string> channels = new();
                List<string> categorizes = new();
                string[] html = Directory.GetFiles(target ?? throw new ArgumentException("target is not valid!"), "*.html");
                foreach (string pathName in html)
                {
                    string full = pathName.Split('/').Last();
                    string[] split = full.Split('[')[0].Split('-');
                    string category = split[1].Trim();
                    IEnumerable<string> nameList = split.Skip(2);
                    string name = string.Join('-', nameList).Replace(" ", string.Empty);
                    if (channels.ContainsKey(name)) name = $"{category} {name}";
                    File.Move(pathName, $"{target}{name}.html");
                    channels.Add(name, category);
                    if (!categorizes.Contains(category)) categorizes.Add(category);
                }

                string final = "";
                foreach (string category in categorizes)
                {
                    final += $"\n## {category}";
                    foreach ((string key, string _) in channels.Where(channel => channel.Value == category))
                    {
                        final += $"\n\n### [{key}](https://svarchive.github.io/{gitdir}{key})";
                        channels.Remove(key);
                    }
                }

                File.WriteAllText(target + "server.md", final);
                channels.Clear();
                categorizes.Clear();
            }
        }
    }
}
