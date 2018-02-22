using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace flightapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //var url = "https://www.expedia.com/Flights?langid=1033&semcid=us.multilob.google.gt-c-en.flight&kword=ZzZz.4850000000144.ee1da01b-3d8e-4bd7-9b32-ead93528c0c9&semdtl=a1355852835.b125535175395.d1221925365712.e1c.f11t2.g1kwd-20847100.h1e.i1.j19012552.k1.l1g.m1.n1&gclid=CjwKCAiAk4XUBRB5EiwAHBLUMaQukzSc-jtDURjz7uUKxUC_59pAEs7tQ2iR9fha0JD4h1amrQtgFxoC5_IQAvD_BwE";
            //var web = new HtmlWeb();
            //var doc = web.Load(url);
            //var parsed = doc.ParsedText;




            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.cheapoair.com/flights/booknow/cheap-flights?fpaffiliate=google-cf&fpsub=gt-cheap-flights-e&gclid=CjwKCAiAk4XUBRB5EiwAHBLUMS5Cbb0ijD7Esd_ZRyC3hsq0jeSxzlJPqcDV4KKeVYLbj6_HA9gsMhoCAHcQAvD_BwE");
            //Declare a implicit wait for synchronisation


            string[] codes = new string[] { "AAB", "AAA","AAC", "AAD", "AAE", "AAF", "AAG", "AAH", "AAI", "AAJ", "AAK", "AAL", "AAM", "AAN", "AAO", "AAQ","AAR",
                "AAS","AAT","AAU","AAV","AAX","AAY","AAZ","ABA","ABB","ABC","ABD","ABE","ABF","ABG","ABH","ABI" };
           
            List<Dictionary<string, double>> topFlights = new List<Dictionary<string, double>>();
            Dictionary<string, double> flight = new Dictionary<string, double>();

        
            foreach (var code in codes)
            {
               
                
                driver.SwitchTo().ParentFrame();
                IWebElement destination = driver.FindElement(By.CssSelector("#ember545"));
                System.Threading.Thread.Sleep(1000);
                destination.Click();
                System.Threading.Thread.Sleep(1000);
                destination.SendKeys(Keys.Backspace);
                //destination.SendKeys(Keys.Clear);
                System.Threading.Thread.Sleep(2000);
                //destination.SendKeys("ATL - Atlanta All Airports, Georgia, United States");
                destination.SendKeys(code);
                System.Threading.Thread.Sleep(2000);
                destination.SendKeys(Keys.Tab);
                Actions actions = new Actions(driver);
                IWebElement departuredate = driver.FindElement(By.Id("departCalendar_0"));
              


                actions.MoveToElement(departuredate).Click().Perform();
                IWebElement clickondepartdate = driver.FindElement(By.CssSelector("#calendarCompId > section > div > div:nth-child(1) > ol > div:nth-child(29) > li"));
                actions.MoveToElement(clickondepartdate).Click().Perform();
                System.Threading.Thread.Sleep(2000);
                IWebElement departSelect = driver.FindElement(By.CssSelector("#ember514 > section > form > fieldset.search__single-trip > fieldset.search__trip-date.form-horizontal > div:nth-child(3) > label"));
                actions.MoveToElement(departSelect).Click().Perform();
                IWebElement clickoncomebackdate = driver.FindElement(By.CssSelector("#calendarCompId > section > div > div:nth-child(1) > ol > div:nth-child(32) > li"));
                actions.MoveToElement(clickoncomebackdate).Click().Perform();
                System.Threading.Thread.Sleep(2000);

                IWebElement people = driver.FindElement(By.CssSelector("#ember565"));
                actions.MoveToElement(people).Click().Perform();
                IWebElement selectFlights = driver.FindElement(By.CssSelector("#ember514 > section > form > input"));
                actions.MoveToElement(selectFlights).Click().Perform();

                System.Threading.Thread.Sleep(17000);
                IJavaScriptExecutor javascriptDriver = (IJavaScriptExecutor)driver;
                try
                {
                    IWebElement selectbestflight = driver.FindElement(By.CssSelector("#MSFL-2 > span.currency"));
                    Dictionary<string, object> attributes = javascriptDriver.ExecuteScript("var items = {}; for (index = 0; index < arguments[0].attributes.length; ++index) { items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; return items;", selectbestflight) as Dictionary<string, object>;
                    // actions.MoveToElement(selectbestflight).Click().Perform();
                    
                    flight.Add(code, Convert.ToDouble(attributes.ElementAt(1).Value.ToString()));
                    
                }
                catch
                {

                }


                System.Threading.Thread.Sleep(5000);
                driver.Navigate().Back();
                System.Threading.Thread.Sleep(2000);
            }
            var max = flight.OrderBy(dict => dict.Value);



            using (var file = File.CreateText(@"C:\Users\dilli\OneDrive\csv.txt"))
            {
                foreach (var arr in max)
                {
                    var flightdestination = arr.Key;
                    var cost = arr.Value; 
                    file.WriteLine(string.Join(",", flightdestination+" " + cost));
                }
            }
            
       
        }
      
    }
}
