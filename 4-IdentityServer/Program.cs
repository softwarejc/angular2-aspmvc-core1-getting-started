using System;
using Microsoft.Owin.Hosting;


namespace _4_IdentityServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("https://localhost:44300"))
            {
                Console.ReadLine();
            }
        }
    }
}
