using Microsoft.Bot.Builder.Resource;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;

namespace LowesBot.Services
{
    public static class ResourceHelper
    {
        static readonly ResourceManager _manager;

        static ResourceHelper()
        {
            _manager = new ResourceManager("LowesBot.App_GlobalResources.Strings", typeof(ResourceHelper).Assembly);
        }

        public static string GetString(string key)
        {
            try
            {
                return _manager.GetString(key, new CultureInfo("en"));
            }
            catch (Exception ex)
            {
                Debugger.Break();
                throw;
            }
        }
    }
}