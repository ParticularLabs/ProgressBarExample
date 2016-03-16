using System;
using Microsoft.Owin.Hosting;

namespace ProgressBarWebAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8576"))
            {
                Console.WriteLine("Status Store Server is running.");
                Console.WriteLine("Press enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
