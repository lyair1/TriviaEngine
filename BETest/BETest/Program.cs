using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BETest
{
    class Program
    {
        static void Main(string[] args)
        {
            GoogleSearchClient gsc = new GoogleSearchClient();
            Console.WriteLine(gsc.SearchAndCountResults("(organs found inside skull) AND \"brain\"").Result);
            Console.ReadLine();
        }
    }
}
