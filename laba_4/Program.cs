using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
namespace Lab_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                try
                {
                    // Открываем указанную веб-страницу
                    driver.Navigate().GoToUrl("https://vk.com/video");
                    System.Threading.Thread.Sleep(5000);  // Даем время странице загрузиться

                    // Сбор ссылок на видео
                    var videoLinks = new List<string>();

                    // Получаем ссылки из раздела "Для вас"
                    var forYouLinks = driver.FindElements(By.CssSelector("div.video_item a"));
                    foreach (var link in forYouLinks)
                    {
                        videoLinks.Add(link.GetAttribute("href"));
                    }

                    // Получаем ссылки из раздела "Тренды"
                    var trendsLinks = driver.FindElements(By.CssSelector("div.video_item a"));
                    foreach (var link in trendsLinks)
                    {
                        videoLinks.Add(link.GetAttribute("href"));
                    }

                    // Записываем количество ссылок в файл
                    File.WriteAllText("video_links_count.txt", videoLinks.Count.ToString());

                    // Сбор информации о видео
                    using (StreamWriter sw = new StreamWriter("video_details.txt"))
                    {
                        foreach (var videoLink in videoLinks)
                        {
                            try
                            {
                                // Переход по каждой ссылке на видео
                                driver.Navigate().GoToUrl(videoLink);
                                System.Threading.Thread.Sleep(3000);  // Даем время странице загрузиться

                                // Получаем информацию о видео
                                string title = driver.FindElement(By.CssSelector("h1.video_title")).Text;
                                string views = driver.FindElement(By.CssSelector("span.views_count")).Text;
                                string likes = driver.FindElement(By.CssSelector("span.likes_count")).Text;
                                string createDate = driver.FindElement(By.CssSelector("span.date")).Text;
                                string channelName = driver.FindElement(By.CssSelector("a.channel_name")).Text;
                                string subscribersCount = driver.FindElement(By.CssSelector("span.subscribers_count")).Text;

                                // Записываем данные в файл
                                sw.WriteLine($"{title}, {views}, {likes}, {createDate}, {channelName}, {subscribersCount}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка при обработке {videoLink}: {ex.Message}");
                            }
                        }
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
        }
    }
}
