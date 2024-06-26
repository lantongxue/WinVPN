﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WinVPN.Base
{
    public class Config
    {
        private string dns1 = "";
        private string dns2 = "";

        public string Dns1
        { 
            get => dns1;
            set => dns1 = value; 
        }
        public string Dns2 
        { 
            get => dns2; 
            set => dns2 = value; 
        }

        public List<CustomDns> DnsList { get; set; }
    }

    public class CustomDns
    {
        public string Name { get; set; }

        public IPAddress Dns1 { get; set; }

        public IPAddress Dns2 { get; set; }
    }
}
