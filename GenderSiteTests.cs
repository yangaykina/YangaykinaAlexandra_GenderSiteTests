using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ClassLibrary1
{
    public class GenderSiteTests
    {
        public ChromeDriver driver;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized"); //браузер раскрывается на весь экран
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //неявные ожидания
        }


        private static string expectedEmail = "test@mail.ru";
        private static string incorrectEmail = "12345"; 
        private By emailInputLocator = By.Name("email");
        private By buttonLocator = By.Id("sendMe");
        private By emailResultLocator = By.ClassName("your-email");
        private By anotherEmailLinkLocator = By.Id("anotherEmail");
        private By radiobuttonBoy = By.Id("boy");
        private By radiobuttonGirl = By.Id("girl");
        private By resultText = By.ClassName("result-text");
        private By validationEmail = By.ClassName("form-error");

        [Test]
        public void GenderSite_FillFormWithEmail_Success()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual(expectedEmail, driver.FindElement(emailResultLocator).Text, "Сделали завяку не на тот e-mail");
        }

        [Test]
        public void GenderSite_ClickAnotherEmail()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("указать другой e-mail", driver.FindElement(anotherEmailLinkLocator).Text, "Не появилась ссылка 'указать другой email'");
        }

        [Test]
        public void GenderSite_ClickAnotherEmail_EmailInputIsEmpty()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(anotherEmailLinkLocator).Click();

            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "После клика по ссылке поле не очистилось");
            Assert.IsFalse(driver.FindElements(anotherEmailLinkLocator).Count == 0, "Не исчезла ссылка для ввода другого e-mail");
        }

        [Test]
        public void GenderSite_RadiobuttonBoy_GenderIsBoy()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(radiobuttonBoy).Click();
            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Хорошо, мы пришлём имя для вашего мальчика на e-mail:", driver.FindElement(resultText).Text, "Отправилось имя для девочки вместо имени для мальчика");
        }

        [Test]
        public void GenderSite_RadiobuttonGirl_GenderIsGirl()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(radiobuttonGirl).Click();
            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Хорошо, мы пришлём имя для вашей девочки на e-mail:", driver.FindElement(resultText).Text, "Отправилось имя для мальчика вместо имени для девочки");
        }

        [Test]
        public void GenderSite_FillFormWithIncorrectEmail_Failure()
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys(incorrectEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.IsTrue(driver.FindElements(validationEmail).Count == 1, "Не появилось сообщение об ошибке");
            Assert.AreEqual("Некорректный email", driver.FindElement(validationEmail).Text, "Неправильный текст валидации");
        }
        
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }

}
