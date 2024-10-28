using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading;
namespace Lab_5
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
                    driver.Navigate().GoToUrl("https://ci.nsu.ru/news");

                    // Устанавливаем начальную и конечную дату в фильтрах
                    var startDateInput = driver.FindElement(By.CssSelector("input[type='date'][placeholder='Дата начала']"));
                    var endDateInput = driver.FindElement(By.CssSelector("input[type='date'][placeholder='Дата конца']"));

                    startDateInput.SendKeys("2020-10-01");
                    endDateInput.SendKeys("2024-10-01");

                    // Нажимаем кнопку "Применить"
                    var applyFilterButton = driver.FindElement(By.CssSelector("button.btn.btn-primary")); // Измените селектор на нужный
                    applyFilterButton.Click();

                    // Ждем загрузку новостей
                    Thread.Sleep(5000); // Можно заменить на WebDriverWait для лучшей практики

                    // Загружаем все новости, пока кнопка доступна
                    var loadMoreButton = driver.FindElement(By.CssSelector("button.load-more"));
                    while (loadMoreButton.Displayed)
                    {
                        loadMoreButton.Click();
                        Thread.Sleep(2000); // Ждем, пока подгрузятся новости
                    }

                    // Создаем или открываем файл для записи результата
                    using (StreamWriter file = new StreamWriter("result.txt"))
                    {
                        // Находим все карточки новостей
                        var newsCards = driver.FindElements(By.CssSelector(".news-item"));

                        foreach (var card in newsCards)
                        {
                            var date = card.FindElement(By.CssSelector(".news-date")).Text;
                            var title = card.FindElement(By.CssSelector(".news-title")).Text;
                            var link = card.FindElement(By.CssSelector("a.news-link")).GetAttribute("href");
                            var imageUrl = card.FindElement(By.CssSelector("img")).GetAttribute("src");

                            // Записываем в файл
                            file.WriteLine($"{date}, {title}, {link}, {imageUrl}");
                        }
                    }
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
