using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaaSAdmin.ServiceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool blIsSuccess = new Service.Utility.Utility().UploadHealth();
            Console.WriteLine(blIsSuccess);

            Console.ReadKey();
        }
    }
}
