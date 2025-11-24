using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System;
using System.Threading;

namespace lab5_kpi
{
    public class Lab5Tests : BaseTest
    {
        private const string BaseUrl = "https://the-internet.herokuapp.com";

        /// <summary>
        /// Тест №1: Перевірка роботи чекбоксів.
        /// Сценарій: Перевіряє початковий стан чекбоксів та зміну стану першого чекбокса після кліку.
        /// </summary>
        [Test]
        public void Test1_Checkboxes()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/checkboxes");
            IList<IWebElement> checkboxes = driver.FindElements(By.CssSelector("input[type='checkbox']"));

            Assert.That(checkboxes[0].Selected, Is.False, "Помилка: Чекбокс 1 має бути порожнім за замовчуванням");
            Assert.That(checkboxes[1].Selected, Is.True, "Помилка: Чекбокс 2 має бути обраним за замовчуванням");

            checkboxes[0].Click();
            Assert.That(checkboxes[0].Selected, Is.True, "Помилка: Чекбокс 1 не активувався після кліку");
        }

        /// <summary>
        /// Тест №2: Додавання та видалення елементів.
        /// Сценарій: Додає елемент, перевіряє його наявність, видаляє його та перевіряє відсутність.
        /// </summary>
        [Test]
        public void Test2_AddRemoveElements()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/add_remove_elements/");
            driver.FindElement(By.CssSelector("button[onclick='addElement()']")).Click();

            IWebElement deleteButton = driver.FindElement(By.ClassName("added-manually"));
            Assert.That(deleteButton.Displayed, Is.True, "Помилка: Кнопка Delete не з'явилася після додавання");

            deleteButton.Click();
            var deletedButtons = driver.FindElements(By.ClassName("added-manually"));
            
            Assert.That(deletedButtons.Count, Is.EqualTo(0), "Помилка: Кнопка Delete не зникла після натискання");
        }

        /// <summary>
        /// Тест №3: Робота з випадаючим списком (Dropdown).
        /// Сценарій: Вибирає опцію 'Option 1' за текстом та перевіряє, що вибір застосовано.
        /// </summary>
        [Test]
        public void Test3_Dropdown()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/dropdown");
            IWebElement dropdownElement = driver.FindElement(By.Id("dropdown"));
            SelectElement select = new SelectElement(dropdownElement);

            select.SelectByText("Option 1");
            Assert.That(select.SelectedOption.Text, Is.EqualTo("Option 1"));
        }

        /// <summary>
        /// Тест №4: Введення даних у поле Input.
        /// Сценарій: Вводить числове значення '123' у поле та перевіряє атрибут 'value'.
        /// </summary>
        [Test]
        public void Test4_Inputs()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/inputs");
            IWebElement inputField = driver.FindElement(By.CssSelector("input[type='number']"));

            inputField.SendKeys("123");
            
            string? value = inputField.GetAttribute("value");
            Assert.That(value, Is.EqualTo("123"));
        }

        /// <summary>
        /// Тест №5: Перевірка кодів відповіді HTTP.
        /// Сценарій: Переходить за посиланням '200' та перевіряє URL і наявність тексту статусу на сторінці.
        /// </summary>
        [Test]
        public void Test5_StatusCodes()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/status_codes");
            driver.FindElement(By.LinkText("200")).Click();

            Assert.That(driver.Url, Does.Contain("200"));
            
            string pageText = driver.FindElement(By.TagName("body")).Text;
            Assert.That(pageText, Does.Contain("200 status code"));
        }

        /// <summary>
        /// Тест №6: Робота з вкладеними фреймами (Nested Frames).
        /// Сценарій: Перемикається на верхній, потім на середній фрейм і перевіряє текст 'MIDDLE'.
        /// </summary>
        [Test]
        public void Test6_NestedFrames()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/nested_frames");

            // Перемикання контексту драйвера на фрейми
            driver.SwitchTo().Frame("frame-top");
            driver.SwitchTo().Frame("frame-middle");

            IWebElement body = driver.FindElement(By.Id("content"));
            Assert.That(body.Text, Is.EqualTo("MIDDLE"));

            // Повернення на основну сторінку
            driver.SwitchTo().DefaultContent();
        }

        /// <summary>
        /// Тест №7: Автентифікація через форму.
        /// Сценарій: Вводить логін/пароль, виконує вхід та перевіряє повідомлення про успіх і кнопку Logout.
        /// </summary>
        [Test]
        public void Test7_FormAuthentication()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/login");

            driver.FindElement(By.Id("username")).SendKeys("tomsmith");
            driver.FindElement(By.Id("password")).SendKeys("SuperSecretPassword!");
            driver.FindElement(By.CssSelector("button.radius")).Click();

            IWebElement flashMessage = driver.FindElement(By.Id("flash"));
            Assert.That(flashMessage.Text, Does.Contain("You logged into a secure area!"));

            bool isLogoutButtonDisplayed = driver.FindElement(By.CssSelector("a.button.secondary")).Displayed;
            Assert.That(isLogoutButtonDisplayed, Is.True);
        }

        /// <summary>
        /// Тест №8: Перевірка динамічного контенту.
        /// Сценарій: Оновлює сторінку та перевіряє, що текст у блоці змінився.
        /// </summary>
        [Test]
        public void Test8_DynamicContent()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/dynamic_content");
            string xpath = "(//div[@class='large-10 columns'])[3]";
            string textBeforeRefresh = driver.FindElement(By.XPath(xpath)).Text;

            driver.Navigate().Refresh();
            string textAfterRefresh = driver.FindElement(By.XPath(xpath)).Text;

            Assert.That(textBeforeRefresh, Is.Not.EqualTo(textAfterRefresh), "Помилка: Текст мав змінитися після оновлення сторінки");
        }

        /// <summary>
        /// Тест №9: Зникаючі елементи меню.
        /// Сценарій: Перевіряє наявність стабільної кнопки 'Home' та загальну кількість елементів меню.
        /// </summary>
        [Test]
        public void Test9_DisappearingElements()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/disappearing_elements");

            IWebElement homeButton = driver.FindElement(By.LinkText("Home"));
            Assert.That(homeButton.Displayed, Is.True);

            IList<IWebElement> menuItems = driver.FindElements(By.TagName("li"));
            Assert.That(menuItems.Count, Is.GreaterThanOrEqualTo(4));
        }

        /// <summary>
        /// Тест №10: Взаємодія при наведенні курсору (Hover).
        /// Сценарій: Наводить курсор на аватар, чекає появи прихованого тексту та перевіряє ім'я користувача.
        /// </summary>
        [Test]
        public void Test10_Hovers()
        {
            driver.Navigate().GoToUrl(BaseUrl + "/hovers");

            IWebElement avatar = driver.FindElement(By.CssSelector(".figure:nth-of-type(1) img"));
            
            // Використання Actions для емуляції наведення миші
            OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(driver);
            action.MoveToElement(avatar).Perform();

            // Явне очікування появи елемента
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            IWebElement hiddenText = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".figure:nth-of-type(1) .figcaption h5")));
            
            Assert.That(hiddenText.Displayed, Is.True, "Помилка: Текст не з'явився при наведенні");
            Assert.That(hiddenText.Text, Is.EqualTo("name: user1"));
        }
    }
}