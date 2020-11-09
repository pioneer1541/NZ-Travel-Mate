using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NZ_Travel_Mate
{
    public class ConvertJson
    {
        public object rates { get; set; }
        public string base_currency { get; set; }
        public string date { get; set; }
    }
}
