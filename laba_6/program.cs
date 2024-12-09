using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.IO;

namespace Lab6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    // Часть 1: Работа с контекстным меню
                    Console.WriteLine("Часть 1: Работа с контекстным меню");
                    driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/context_menu");

                    // Находим панель
                    var panel = driver.FindElement(By.Id("hot-spot"));

                    // Имитируем правый клик с помощью Actions
                    Actions actions = new Actions(driver);
                    actions.ContextClick(panel).Perform();

                    // Обработка окна подтверждения
                    var alert = driver.SwitchTo().Alert();
                    Console.WriteLine("Текст всплывающего окна: " + alert.Text);

                    // Закрываем всплывающее окно
                    alert.Accept();

                    // Делаем скриншот после закрытия окна подтверждения
                    TakeScreenshot(driver, "context_menu_alert.png");

                    // Часть 2: Загрузка файла
                    Console.WriteLine("Часть 2: Загрузка файла");
                    driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/upload");

                    // Создаем пустой текстовый файл
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "empty_file.txt");
                    File.WriteAllText(filePath, string.Empty);

                    // Находим элемент загрузки файла и загружаем файл
                    var fileInput = driver.FindElement(By.Id("file-upload"));
                    fileInput.SendKeys(filePath);

                    // Нажимаем на кнопку "Upload"
                    var uploadButton = driver.FindElement(By.Id("file-submit"));
                    uploadButton.Click();

                    // Делаем скриншот результата
                    TakeScreenshot(driver, "file_upload_result.png");

                    // Проверяем текст на странице
                    var uploadedFileName = driver.FindElement(By.Id("uploaded-files")).Text;
                    Console.WriteLine("Загруженный файл: " + uploadedFileName);

                    Console.WriteLine("Все задачи выполнены успешно.");
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
static void TakeScreenshot(IWebDriver driver, string fileName)
{
    try
    {
        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        screenshot.SaveAsFile(filePath); // Указание формата файла через расширение имени
        Console.WriteLine($"Скриншот сохранён: {filePath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при сохранении скриншота: {ex.Message}");
    }
}
    }
}
