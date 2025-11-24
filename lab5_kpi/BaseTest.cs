using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace lab5_kpi 
{
    /// <summary>
    /// Базовий клас для всіх автоматизованих тестів.
    /// Відповідає за керування життєвим циклом браузера (відкриття/закриття),
    /// забезпечуючи ізольованість кожного тесту та уникнення дублювання коду.
    /// </summary>
    public class BaseTest
    {
        /// <summary>
        /// Екземпляр веб-драйвера Selenium, який керує браузером.
        /// Оголошений як 'protected', щоб бути доступним у класах-спадкоємцях.
        /// </summary>
        protected IWebDriver driver;

        /// <summary>
        /// Метод налаштування (SetUp), що автоматично запускається перед кожним тестом.
        /// Ініціалізує ChromeDriver, розгортає вікно на весь екран та налаштовує неявні очікування.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            
            // Встановлюємо неявне очікування 5 секунд для пошуку елементів
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        /// <summary>
        /// Метод завершення (TearDown), що автоматично запускається після кожного тесту.
        /// Закриває браузер (Quit) та звільняє ресурси пам'яті (Dispose), навіть якщо тест впав з помилкою.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}