using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
namespace Lab2_Test

{
    public class Program
    {
        static void Main(string[] args)
        {
            // 1. Запуск браузера и открытие сайта Python
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.python.org");

            // 2. Найдите элемент кнопки Downloads и кликните
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement downloadButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.LinkText("Downloads")));
                downloadButton.Click();
                Console.WriteLine("Кнопка 'Downloads' нажата.");
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Кнопка 'Downloads' не найдена.");
            }

            // 3. Найдите элемент текстового поля Search и кнопки GO
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement searchBox = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("q")));
                IWebElement goButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("submit")));

                // 4. Введите в поле текст и нажмите кнопку
                string searchText = "Python";
                searchBox.SendKeys(searchText);
                goButton.Click();

                // Убедитесь, что текст был успешно вставлен в поле
                if (searchBox.GetAttribute("value").Equals(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Текст успешно вставлен в поле поиска.");
                }
                else
                {
                    Console.WriteLine("Текст не был вставлен в поле поиска.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Элемент(ы) не найден(ы).");
            }

            // 5. Закройте браузер
            driver.Quit();
        }
    }
}
