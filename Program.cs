﻿using System;
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
            Dictionary<int, Tuple<string, string>> channels = new Dictionary<int, Tuple<string, string>>();
            int num = 0;
            List<string> catogories = new List<string>();
            Console.WriteLine("Input target directory: ");
            string target = Console.ReadLine();
            string[] html = Directory.GetFiles(target , "*.html");
            foreach (string pathName in html)
            {
                string full = pathName.Split('/').Last();
                string[] split = full.Split('[')[0].Split('-');
                var catogory = split[1].Trim();
                var nameList = split.Skip(2);
                string name = string.Join('-', nameList).Replace(" ", string.Empty);
                channels.Add(num, Tuple.Create(name, catogory));
                num += 1;
                if (!catogories.Contains(catogory)) catogories.Add(catogory);
                File.Move(pathName, $"{target}{name}.html");
                Console.WriteLine(name + " " + catogory);
            }

            string final = "";
            foreach (string catogory in catogories)
            {
                final += $"\n## {catogory}";
                foreach (var channel in channels)
                {
                    if (channel.Value.Item2 == catogory)
                    {
                        final += $"\n\n### [{channel.Value.Item1}](svarchive.github.io{target.Split("svarchive.github.io")[1].Replace(@"\","/")}{channel.Value.Item1})";
                        channels.Remove(channel.Key);
                    }
                }
            }

            File.WriteAllText(target + "server.md", final);
        }
    }
}
