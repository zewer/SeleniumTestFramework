﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.PageObjects
{
    public static class Application
    {
        public static GooglePage GooglePage => WebDriver.AttachToPage<GooglePage>();
    }
}
