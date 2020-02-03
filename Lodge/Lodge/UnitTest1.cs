using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using System;

namespace Lodge
{
    [TestClass]
    public class UnitTest1
    {

        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string Lodge = @"C:\\Program Files\\HOLMS\\Integrate LodgeIC 2\\LodgeIC2.exe";
        protected static WindowsDriver<WindowsElement> session;
        
        [TestMethod]
        public void TestMethod1()
        {
            if (session == null)
            {
                DateTime date = DateTime.Now;
                string startDate= string.Format("{0:D}", date);
                string endDate = string.Format("{0:D}", date.AddDays(1));
                AppiumOptions opt = new AppiumOptions();
                opt.AddAdditionalCapability("app", Lodge);
                opt.AddAdditionalCapability("deviceName", "WindowsPC");
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), opt);
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);
                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                session.FindElementByAccessibilityId("userIDTb").SendKeys("ashish");
                session.FindElementByAccessibilityId("passwordTb").SendKeys("password");
                session.FindElement(By.XPath("//Window/Custom[1]/Button[3]")).Click();
                session.FindElement(By.XPath("//Window/Custom[1]/Button[3]/ComboBox[1]")).Clear();
                session.FindElement(By.XPath("//Window/Custom[1]/Button[3]/ComboBox[1]")).SendKeys("holms-qa.centricconsulting.com:8080");
                session.FindElementByAccessibilityId("loginBtn").Click();
                //Verify that after login PMC QA Tenancy Text is visible
                string ExpectedValue = session.FindElementByAccessibilityId("tenancyName").Text.ToString();
                Assert.AreEqual("PMC QA Tenancy", ExpectedValue);

                //New Reservation
                session.FindElementByAccessibilityId("TabReservations").Click(); //TabReservations//NewReservationButtom
                session.FindElementByAccessibilityId("NewReservationButtom").Click();
                //Input Dates

                session.FindElementByAccessibilityId("PART_TextBoxEntryPort").Clear();
                session.FindElementByAccessibilityId("PART_TextBoxEntryPort").SendKeys(startDate);
                session.FindElementByAccessibilityId("roomTypeCb").Click();
                session.FindElement(By.Name("DBL DBL Deluxe")).Click();
                System.Threading.Thread.Sleep(3);
                //Lodging total text assertion
                string lodgeTextAssertion = session.FindElement(By.Name("LODGING SUBTOTAL")).Text.ToString();
                Assert.AreEqual("LODGING SUBTOTAL", lodgeTextAssertion);
                System.Threading.Thread.Sleep(3);
                session.FindElement(By.XPath("//Window/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Edit[5]/Text[1]")).Click();
                session.FindElement(By.XPath("//Window/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Edit[5]")).SendKeys("seh");
                session.FindElement(By.XPath("//Window/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Pane[1]/Button[3]")).Click();
                session.FindElement(By.XPath("//Window/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Button[12]")).Click();
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                //Assign Room
                session.FindElementByAccessibilityId("Button_ShowAssignableRooms").Click();
                //Room Assigned
                System.Threading.Thread.Sleep(3);
                session.FindElement(By.XPath("//Window/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Pane[1]/Pane[3]")).Click();
                //verify that room assigned
                string assignedRoom = session.FindElement(By.XPath("Window/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Pane[1]/Pane[2]/Button[1]/Text[4]")).Text.ToString();
                Assert.AreNotEqual("Unassigned", assignedRoom);
                //click to checkin
                session.FindElement(By.XPath("//Window/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Button[2]")).Click();
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                //input govt ID
                session.FindElement(By.XPath("//Window/Window[1]/Edit[1]")).SendKeys("1234");
                //input vehicle plate
                session.FindElement(By.XPath("//Window/Window[1]/Edit[2]")).SendKeys("1323");
                //click on allow checkin without authorization
                session.FindElement(By.XPath("//Window/Window[1]/CheckBox[1]")).Click();
                //click checkin
                session.FindElement(By.XPath("//Window/Window[1]/Button[6]")).Click();
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                string checkedIn = session.FindElement(By.XPath("Window/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Tab[1]/TabItem[2]/Custom[1]/Text[2]")).Text.ToString();
                Assert.AreEqual("CHECKED IN", checkedIn);
        
            }
        }
    }
}
