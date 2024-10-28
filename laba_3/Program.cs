using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
namespace Lab_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Установка параметров для браузера
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            // Создание экземпляра ChromeDriver
            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    // Открываем веб-страницу
                    driver.Navigate().GoToUrl("https://example.com"); // Замените на нужный URL

                    // Ждем, пока страница загрузится
                    Thread.Sleep(2000); // Используйте WebDriverWait для более стабильного кода

                    // Поиск элемента с помощью XPath
                    var element = driver.FindElement(By.XPath("//h1")); // Замените на нужный XPath

                    // Вывод текста найденного элемента
                    Console.WriteLine("Найденный элемент: " + element.Text);
                }
                catch (NoSuchElementException e)
                {
                    Console.WriteLine("Элемент не найден: " + e.Message);
                }
                finally
                {
                    // Закрываем браузер
                    driver.Quit();
                }
            }
        }
    }
}
