namespace FaturasHandler.Crawler.Storage
{
    using System;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;

    namespace FaturasHandler.Utils
    {
        public static class JsonFileHelper
        {
            private static readonly string _outputDirectory = @"C:\src\__FaturasHandler\angular\faturasv4\faturas-app\src\db-json";

            public static void SaveJsonToFile<T>(T data, string dtoName)
            {
                try
                {
                    if (!Directory.Exists(_outputDirectory))
                    {
                        Directory.CreateDirectory(_outputDirectory);
                    }

                    string fileName = $"{dtoName}.json";
                    string filePath = Path.Combine(_outputDirectory, fileName);

                    string jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);
                    File.WriteAllText(filePath, jsonContent, Encoding.UTF8);

                    Console.WriteLine($"✅ JSON saved to {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Saving JSON failed: {ex.Message}");
                }
            }
        }
    }
}
