using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class ConsoleNavigation
    {
        public void WaitQuit()
        {
            Console.WriteLine();
            Console.WriteLine("Для выхода нажмите ENTER");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }
    }
}
