using System;
using System.IO;

namespace JsonRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (File.Exists(args[i]))
                {
                    if (!args[i].EndsWith(".json")) continue;
                    FixData(args[i]);
                }
            }
        }

        /// <summary>
        /// The main function. Reads the <see cref="File"/> at the specified <paramref name="path"/> and applies edits to it.
        /// </summary>
        /// 
        /// <param name="path">The path to to the desired file.</param>
        public static void FixData(string path)
        {
            Console.WriteLine("Enter new id for " + Path.GetFileName(path));
            string id = Console.ReadLine();
            string content = File.ReadAllText(path);

            // ↓ this part can be edited ↓
            content = FindAndReplace(content, "id", id);
            content = FindAndReplace(content, "localizationId", id);
            content = FindAndReplace(content, "prefabAddress", id);
            content = FindAndReplace(content, "iconAddress", id + ".Icon");
            // ↑ this part can be edited ↑

            File.WriteAllText(path, content);
            RenameFile(path, id);
        }


        /// <summary>
        /// Renames the <see cref="File"/> at the specified <paramref name="path"/> and renames it according to the file's <paramref name="id"/>.
        /// </summary>
        /// 
        /// <param name="path">The path to to the desired file.</param><br/>
        /// <param name="id">The id of the file.</param>
        /// 
        /// <exception cref="IOException">
        /// Thrown if the file could not be found, was inaccessible (for example due to being opened) or was otherwise not movable
        /// </exception>
        public static void RenameFile(string path, string id)
        {
            try
            {
                string newPath = Path.Combine(Path.GetDirectoryName(path), id + ".json");
                File.Move(path, newPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Finds a substring in the specified <see cref="string"/> and replaces it, then returns the new <see cref="string"/>
        /// </summary>
        /// 
        /// <param name="content">The string which should be edited.</param><br/>
        /// <param name="key">The substring which should be replaced.</param><br/>
        /// <param name="newValue">The substring which should replace the <paramref name="key"/> value.</param>
        /// 
        /// <returns>a copy of <paramref name="content"/> in which the <paramref name="key"/> value was replaced with the <paramref name="newValue"/></returns>
        public static string FindAndReplace(string content, string key, string newValue)
        {
            if (content.Contains(key))
            {
                int startIndex = content.IndexOf(key) + key.Length + 4;
                int endIndex = content.IndexOf('"', startIndex);
                string part1 = content.Substring(0, startIndex);
                string part2 = content.Substring(endIndex);
                content = part1 + newValue + part2;
            }
            return content;
        }
    }
}