using System.Collections;

namespace GameServer
{
    public class KeywordDescription(string keyword, string description)
    {
        public string Keyword { get; set; } = keyword;
        public string Description { get; set; } = description;
    }

    public class Database
    {
        public ArrayList KeywordDescriptions { get; private set; }

        public Database(string filePath)
        {
            KeywordDescriptions = [];
            ParseFile(filePath);
        }

        private void ParseFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            int n = int.Parse(lines[0]);

            for (int i = 1; i <= n * 2; i += 2)
            {
                var keywordDescription = new KeywordDescription(lines[i], lines[i + 1]);

                KeywordDescriptions.Add(keywordDescription);
            }
        }

        public KeywordDescription GetRandomKeyword()
        {
            var random = new Random();
            var index = random.Next(KeywordDescriptions.Count);

            return (KeywordDescription)KeywordDescriptions[index]!;
        }
    }
}