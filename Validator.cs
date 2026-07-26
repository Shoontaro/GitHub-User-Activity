using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub_User_Activity
{
    public class Validator
    {
        public static void NameValidator(string? name) 
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Error: No username provided.");
                Console.WriteLine("Usage: github-activity <username>");
                Environment.Exit(1);
            }
        }
    }
}
