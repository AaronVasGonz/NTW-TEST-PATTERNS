using FAQBot.FAQBot.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTW_TEST_PATTERNSV.FAQBot.FAQBot
{
    public interface IFAQService
    {
        string GetAnswer(string userQuestion);
    }

    public class FAQService : IFAQService
    {
        private readonly List<FAQ> faqs;

        public FAQService()
        {
            string basePath = Directory.GetCurrentDirectory();

            // Sube un nivel
            string parentDirectory = Path.Combine(basePath, "..");

            // Obtén el directorio absoluto (sin esto, la ruta será relativa)
            string fullParentDirectory = Path.GetFullPath(parentDirectory);

            // Ahora navega hacia la carpeta FAQBot, que está en el nivel superior
            string jsonFilePath = Path.Combine(fullParentDirectory, "NTW-TEST-PATTERNSV.FAQBot", "FAQBot", "faq.json");

            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException("The json file for faqs wasn't not found.", jsonFilePath);
            }
            faqs = LoadFaqsFromJson(jsonFilePath);
        }

        public string GetAnswer(string userQuestion)
        {
            var matchingFAQ = faqs.FirstOrDefault(faq => userQuestion.Contains(faq.Question, StringComparison.OrdinalIgnoreCase));

            if (matchingFAQ == null)
            {
                Console.WriteLine("No matching FAQ found for: " + userQuestion);
            }

            return matchingFAQ?.Answer ?? "Sorry, I don't have an answer for that.";
        }

        private List<FAQ> LoadFaqsFromJson(string jsonFilePath)
        {
            string json = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<List<FAQ>>(json);
        }
    }
}
