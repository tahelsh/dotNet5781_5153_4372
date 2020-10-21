//Hi I am Tahel
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00_5153_4372
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome5153();
            Welcome4372();
            Console.ReadKey();
        }

        static partial void Welcome4372();

        private static void Welcome5153()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
//Hello World
