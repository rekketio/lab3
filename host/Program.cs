using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(core.kamenService)))
            {
                host.Open();
                Console.WriteLine("Start");
                Console.ReadKey();
            }
        }
    }
}
