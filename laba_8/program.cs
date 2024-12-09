using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        ChromeOptions options = new ChromeOptions();
        using IWebDriver driver = new ChromeDriver(options);

        try
        {
            string baseUrl = "https://www.culture.ru/literature/poems/author-aleksandr-pushkin";
            driver.Navigate().GoToUrl(baseUrl);

            // Явное ожидание для загрузки страницы
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Найти блоки с ссылками на отдельные стихи
            var poemBlocks = wait.Until(d => d.FindElements(By.CssSelector("div.Iu6ke a.ICocV")));
            List<string> poemLinks = new List<string>();

            foreach (var block in poemBlocks)
            {
                string link = block.GetAttribute("href");
                if (!string.IsNullOrEmpty(link))
                {
                    poemLinks.Add(link);
                }
            }

            int maxPoems = Math.Min(8, poemLinks.Count);
            List<(string Title, string Text, string Translation)> poems = new List<(string Title, string Text, string Translation)>();

            for (int i = 0; i < maxPoems; i++)
            {
                driver.Navigate().GoToUrl(poemLinks[i]);

                // Извлечение заголовка
                var titleElement = wait.Until(d => d.FindElement(By.CssSelector("div.rrWFt")));
                string title = titleElement.Text.Trim();

                // Извлечение текста стихотворения
                var textElement = wait.Until(d => d.FindElement(By.CssSelector("div.lsom6")));
                string text = textElement.Text.Trim();

                // Переход на страницу переводчика
                driver.SwitchTo().NewWindow(WindowType.Tab);
                driver.Navigate().GoToUrl("https://translate.yandex.ru/");

                // Ввод текста для перевода
                var inputElement = wait.Until(d => d.FindElement(By.CssSelector("div#fakeArea")));
                inputElement.SendKeys(text);

                // Ожидание появления перевода
                var translationElement = wait.Until(d =>
                {
                    var element = d.FindElement(By.CssSelector("div#dstTextField .nI3G8IFy_0MnBmqtxi8Z"));
                    return !string.IsNullOrEmpty(element.Text) ? element : null;
                });

                string translation = translationElement.Text.Trim();

                // Сохранение данных
                poems.Add((title, text, translation));

                // Закрыть вкладку переводчика и вернуться к исходной
                driver.Close();
                driver.SwitchTo().Window(driver.WindowHandles[0]);
            }

            // Сохранение данных в файл
            string filePath = "poems.txt";
            using StreamWriter writer = new StreamWriter(filePath);

            int counter = 1;
            foreach (var poem in poems)
            {
                writer.WriteLine($"Стихотворение №{counter}");
                writer.WriteLine($"Название: {poem.Title}");
                writer.WriteLine("Оригинал:");
                writer.WriteLine(poem.Text);
                writer.WriteLine("Перевод:");
                writer.WriteLine(poem.Translation);
                writer.WriteLine(new string('-', 10));
                counter++;
            }

            Console.WriteLine($"Данные успешно сохранены в файл: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
        finally
        {
            driver.Quit();
        }
    }
}
