using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace NZ_Travel_Mate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //1.Check the file of the defalut configuration if is exist.
        //2.Set the Currency List items.
        //3.Load the configuration of the default currencies.
        public MainWindow()
        {
            InitializeComponent();
            Boolean a = File.Exists("Default_Currency.json") ? true : false;
            if (!a)
            {
                StreamWriter WriteFile = new StreamWriter("Default_Currency.json", false);
                WriteFile.Close();
                CurrencyList obj = new CurrencyList();
                obj.Set_Default_Currency("NZD", "AUD");
            }
            string[] ListSource = {
                "NZD",
                "AUD",
                "USD",
                "GBP",
                "EUR",
                "CAD"};
            Currency_From_List.ItemsSource = ListSource;
            Currency_To_List.ItemsSource = ListSource;
            CurrencyList Default = new CurrencyList().Get_Default_Currency();
            Currency_From_List.SelectedItem = Default.DefaultCurrencyFrom;
            Currency_To_List.SelectedItem = Default.DefaultCurrencyTo;
        }

        public decimal Get_Rate(string Currency_From, string Currency_To)
        {
            //Convert a currency to another one.
            //get http response from a API to get JSON data about currency rate.
            var client = new RestClient("https://api.exchangeratesapi.io/latest?symbols="+ Currency_To +"&base=" + Currency_From);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.StatusDescription == "OK")
            {
                object rates = JsonConvert.DeserializeObject<ConvertJson>(response.Content).rates;
                string result = rates.ToString().Substring(rates.ToString().IndexOf(":") + 1, rates.ToString().Length - rates.ToString().IndexOf(":") - 5);
                if (decimal.TryParse(result, out decimal rate))
                {
                    return rate;
                }
                else
                {
                    Error_Message("Convert Error,Please check your enter");
                    return 0;
                }
            } else
            {
                Error_Message("Fail to connect service.Please Check your internet connection!");
                return 0;
            }
            
            
            
        }

        private void Convert_Button_Click(object sender, RoutedEventArgs e)
        {
            //Check if is valid the content users entered.
            //Loading selected currencies and then get rate.
            //result = rate * total
            if (decimal.TryParse(Total_Text.Text,out decimal Total))
            {
                string Currency_From = Currency_From_List.SelectedItem.ToString();
                string Currency_To = Currency_To_List.SelectedItem.ToString();
                if (decimal.TryParse(Total_Text.Text,out decimal total))
                {
                    Conversion_Result_Label.Content = Currency_To + ": " + Get_Rate(Currency_From, Currency_To) * total;
                } else
                {
                    Error_Message("Please check your Total Incl");
                }
                
            }
        }

        private void Tax_Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            Total_Text.Text = Count_Num().ToString();
        }

        private void Currency_Amount_Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            Total_Text.Text = Count_Num().ToString();
        }

        private decimal Count_Num()
        {
            if (decimal.TryParse(Currency_Amount_Text.Text, out decimal amount)) {
                if (decimal.TryParse(Tax_Text.Text, out decimal tax))
                {
                    decimal result = amount * (1 + (tax / 100));
                    return result;
                } else
                {
                    Tax_Text.Text = "0";
                    decimal result = amount;
                    return result;
                }
            } else
            {
                Currency_Amount_Text.Text = "0";
                return 0;
            }
            
        }

        private void Set_Default_From_Button(object sender, RoutedEventArgs e)
        {
            CurrencyList Default = new CurrencyList();
            Default.Set_Default_Currency(Currency_From_List.SelectedItem.ToString(),Default.Get_Default_Currency().DefaultCurrencyTo);
            Sucess_Message();

        }

        private void Set_Default_To_Button(object sender, RoutedEventArgs e)
        {
            CurrencyList Default = new CurrencyList();
            Default.Set_Default_Currency(Default.Get_Default_Currency().DefaultCurrencyFrom, Currency_To_List.SelectedItem.ToString()) ;
            Sucess_Message();

        }
        public void Error_Message(string message = "Something Wrong!")
        {
            MessageBox.Show(message,"Opps!");
        }

        public void Sucess_Message(string message = "Success!")
        {
            MessageBox.Show(message, "Congratulation!");
        }
    }
}
