using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Bieren.CORE.Services
{
    public class Helper
    {
        public static string GetConnectionString()
        {
            return @"Data Source=(local)\SQLEXPRESS;Initial Catalog=praBieren;Integrated security=true;";
        }
        public static string HandleQuotes(string waarde)
        {
            return waarde.Trim().Replace("'", "''");
        }
    }
}
