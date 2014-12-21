using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseConsole_01
{
    public static class ConsoleWriting
    {
        private static int creationProgress;
        public static void WriteCreatingDB()
        {
            String printMe = " -- Creating DataBase -- ";
            Console.SetCursorPosition(Console.WindowWidth / 2 - printMe.Length / 2, 2);
            Console.Write(printMe);
            UpdateProgress(0);
        }

        public static void UpdateProgress(int addMe)
        {
            creationProgress += addMe;
            String printMe = "[";
            for (int i = 0; i < 52; i++)
            {
                printMe += creationProgress <= i ? " " : "|";
            }
            printMe += "]";
            Console.SetCursorPosition(Console.WindowWidth / 2 - printMe.Length / 2, 10);
            Console.Write(printMe);
            if (creationProgress >= 52)
            {
                printMe = " << -- DONE -- >> ";
                Console.SetCursorPosition(Console.WindowWidth / 2 - printMe.Length / 2, 10);
                Console.Write(printMe);
            }
            
            
        }
        public static void listMembers(List<Leden> members){
            String printMe = "";
            Console.Clear();
            Console.SetCursorPosition(2, 0);
            printMe = (String.Format(" {0,-15}   {1,4}   {2,1}   {3,20}   {4,4}   {5,4} ", "Naam", "Num", "S", "Adres", "Geb", "Lid"));
            Console.SetCursorPosition(Console.WindowWidth / 2 - printMe.Length / 2, Console.CursorTop);
            Console.Write(printMe);
            Console.CursorTop += 2;
            foreach (Leden member in members)
            {
                printMe = (String.Format(" {0,-15} | {1,4} | {2,1} | {3,20} | {4,4} | {5,4} ", member.naam, member.snummer, member.s, member.adres, member.gebjaar, member.jaarlid));
                Console.SetCursorPosition(Console.WindowWidth / 2 - printMe.Length / 2, Console.CursorTop);
                Console.Write(printMe);
                Console.CursorTop++;
            }
            Console.ReadKey();
        }

        public static void MainMenu()
        {
            //Console.Clear();
            Console.ReadKey();
        }
    }
}
