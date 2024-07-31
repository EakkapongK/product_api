using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi
{
    public class Env
    {
        
        public static string DbHost()
        {
            var result = Environment.GetEnvironmentVariable("DB_HOST");
            if (result == null)
            {
                result = "localhost";
            }
            return result;
        }
        public static string DbUser()
        {
            var result = Environment.GetEnvironmentVariable("DB_USER");
            if (result == null)
            {
                result = "sa";
            }
            return result;
        }
        public static string DbPassword()
        {
            var result = Environment.GetEnvironmentVariable("DB_PASSWORD");
            if (result == null)
            {
                result = "Admin123";
            }
            return result;
        }
        public static string DbName()
        {
            var result = Environment.GetEnvironmentVariable("DB_NAME");
            if (result == null)
            {
                result = "TestDB";
            }
            return result;
        }
        
        public static string AppToken()
        {
            var result = Environment.GetEnvironmentVariable("APP_TOKEN");
            if (result == null)
            {
                result = "4849ff7bc9e5bc6d9ca6c257e765ce52ef8454efa09cd8e7fd437a7c10ae78ff54b6357b6f6b74e17c6d48b3afe1436fd6d81bdcae18ff36a14b9ccba81c6c06";
            }
            return result;
        }
    }
}