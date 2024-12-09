using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--disable-blink-features=AutomationControlled");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--ignore-ssl-errors");
        options.AddArgument("--allow-insecure-localhost");
        options.AddArgument("--disable-web-security");
        options.AddArgument("--allow-running-insecure-content");
        options.AddArgument("--ignore-certificate-errors");
        options.AcceptInsecureCertificates = true;

        IWebDriver driver = new ChromeDriver(options);

        try
        {
            // Шаг 1: Открыть три вкладки
            // Открыть вкладки с Википедией
            driver.Navigate().GoToUrl("https://ru.wikipedia.org");
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open('https://en.wikipedia.org', '_blank');");
            // Открыть третью вкладку с конвертером Base64
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open('https://www.base64encode.org', '_blank');");


            // Получение списка вкладок
            var tabs = driver.WindowHandles;

            // Шаг 2: Переход на русскую Википедию и открытие случайных статей
            driver.SwitchTo().Window(tabs[1]);
            Console.WriteLine("Открыта русская Википедия");
            List<string> russianTitles = OpenRandomArticles(driver, "li#n-randompage > a", 5);


            // Шаг 3: Переход на английскую Википедию и открытие случайных статей
            driver.SwitchTo().Window(tabs[2]);
            Console.WriteLine("Открыта английская Википедия");
            List<string> englishTitles = OpenRandomArticles(driver, "li#n-randompage > a", 5);

            // Объединяем заголовки
            List<string> allTitles = new List<string>();
            allTitles.AddRange(russianTitles);
            allTitles.AddRange(englishTitles);

            // Шаг 4: Переключение на вкладку с конвертером
            driver.SwitchTo().Window(tabs[0]);
            Console.WriteLine("Переключение на вкладку с конвертером Base64");

            // Шаг 5: Конвертация заголовков в Base64 и вывод в консоль
            foreach (var title in allTitles)
            {
                string encodedTitle = ConvertToBase64(driver, title);
                Console.WriteLine($"Заголовок: {title}");
                Console.WriteLine($"Base64: {encodedTitle}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            driver.Quit();
        }
    }

    static List<string> OpenRandomArticles(IWebDriver driver, string linkSelector, int count)
    {
        List<string> titles = new List<string>();

        for (int i = 0; i < count; i++)
        {
            try
            {
                // Ожидание полной загрузки страницы
                WebDriverWait waitPageLoad = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                waitPageLoad.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                // Ожидание элемента "Случайная статья"
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement? randomLink = wait.Until(drv =>
                {
                    var element = drv.FindElement(By.CssSelector(linkSelector));
                    return element.Displayed && element.Enabled ? element : null;
                });

                if (randomLink == null)
                {
                    Console.WriteLine("Ссылка randomLink не найдена.");
                    continue;
                }

                // Клик по ссылке
                randomLink.Click();

                // Ожидание полной загрузки новой страницы
                waitPageLoad.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                // Сохранение заголовка статьи
                titles.Add(driver.Title);
                Console.WriteLine($"Открыта статья: {driver.Title}");

                // Возврат назад
                driver.Navigate().Back();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при открытии случайной статьи: {ex.Message}");
            }
        }

        return titles;
    }


    static string ConvertToBase64(IWebDriver driver, string text)
    {
        try
        {
            // Ожидание появления текстового поля
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var inputField = wait.Until(drv => drv.FindElement(By.CssSelector("textarea#input")));

            // Очистка и ввод текста
            inputField.Clear();
            inputField.SendKeys(text);

            // Ожидание кнопки и нажатие
            var encodeButton = wait.Until(drv => drv.FindElement(By.CssSelector("button#submit_text")));
            encodeButton.Click();

            // Ожидание и получение результата
            var outputField = wait.Until(drv => drv.FindElement(By.CssSelector("textarea#output")));
            return outputField.Text;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при кодировании в Base64: {ex.Message}");
            return string.Empty;
        }
    }
}
