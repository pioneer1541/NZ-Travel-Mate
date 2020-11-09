using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NZ_Travel_Mate
{
    public class CurrencyList
    {
        public string DefaultCurrencyFrom { get; set; }
        public string DefaultCurrencyTo { get; set; }
        public void Set_Default_Currency(string FValue,string TValue)
        {
            this.DefaultCurrencyFrom = FValue;
            this.DefaultCurrencyTo = TValue;
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            StreamWriter WriteFile = new StreamWriter("Default_Currency.json", false);
            WriteFile.AutoFlush = true;
            WriteFile.Write(json);
            WriteFile.Close();
        }
        public CurrencyList Get_Default_Currency()
        {
            StreamReader ReadFile = new StreamReader("Default_Currency.json");
            var Default_Value = ReadFile.ReadToEnd();
            CurrencyList result = JsonConvert.DeserializeObject<CurrencyList>(Default_Value);
            ReadFile.Close();
            return result;
        }
    }
}
