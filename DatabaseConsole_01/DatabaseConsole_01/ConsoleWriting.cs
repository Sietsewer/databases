﻿using System;
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
            Console.CursorVisible = false;
            String printMe = " -- Creating DataBase -- ";
            Console.SetCursorPosition(Console.WindowWidth / 2 - printMe.Length / 2, 2);
            Console.Write(printMe);
            UpdateProgress(0);
        }

        public static void addMember()
        {
            Leden lid = new Leden();
            bool looping = true;
            String[] options = { "Cancel", "Name", "Member number", "Gender (m/v)", "Adress", "Year of birth", "Member since", "Done"};
            String[] contents = { "", "", "", "", "", "", "", "" };
            contents[2] = (Program.context.Leden.Max(x => x.snummer)+1)+"";
            contents[6] = DateTime.Now.Year + "";
            Console.Clear();
            String printMe = "- Main Menu -";
            Console.SetCursorPosition(Console.WindowWidth / 2 - printMe.Length / 2, 2);
            Console.Write(printMe);
            int index = 0;
            while (looping)
            {
                bool jump = false;
                for (int i = 0; i < options.Length; i++)
                {
                    Console.SetCursorPosition(4, 4 + i);
                    Console.Write(i == index ? " >" : "  ");
                    Console.Write(options[i]);
                    if (i != 0 && i != 7)
                    {
                        Console.Write(": " + contents[i]);
                        for (int j = 0; j < Console.WindowWidth - Console.CursorLeft; j++)
                        {
                            Console.Write(" ");
                        }
                    }
                }
                if (index != 0 && index != 7)
                {
                    Console.CursorVisible = true;
                    try
                    {
                        Console.SetCursorPosition(options[index].Length + contents[index].Length + 8, index + 4);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.Write("Now you're just making stuff up.");
                    }
                }
                else
                {
                    Console.CursorVisible = false;
                }
                ConsoleKeyInfo k = Console.ReadKey(true);
                switch (k.Key)
                {
                    case ConsoleKey.DownArrow:
                        index++;
                        index = index > options.Length - 1 ? options.Length - 1 : index;
                        break;
                    case ConsoleKey.UpArrow:
                        index--;
                        index = index < 0 ? 0 : index;
                        break;
                    case ConsoleKey.Enter:
                        jump = true;
                        break;
                    case ConsoleKey.Backspace:
                        if (contents[index] != "")
                        {
                            contents[index] = contents[index].Remove(contents[index].Length - 1);
                        }
                        break;
                    default:
                        if (Char.IsLetterOrDigit(k.KeyChar))
                        {
                            contents[index] += k.KeyChar;
                        }
                        break;
                }
                if (jump)
                {
                    switch (index)
                    {
                        case 0:
                            looping = false;
                            break;
                        case 7:
                            //String[] options = { "Cancel", "Name", "Member number", "Gender (m/v)", "Adress", "Year of birth", "Member since", "Done"};
                            bool error = false;
                            lid.naam = contents[1];
                            try
                            {
                                lid.snummer = Convert.ToInt32(contents[2]);
                            }
                                catch (Exception e)
                            {
                                error = true;
                            }
                            if (Program.context.Leden.Any(b => b.snummer == lid.snummer))
                            {
                                error = true;
                            }
                            lid.s = contents[3];
                            lid.adres = contents[4];
                            try
                            {
                                lid.gebjaar = Convert.ToInt32(contents[5]);
                            }
                            catch (Exception e)
                            {
                                error = true;
                            }
                            try
                            {
                                lid.jaarlid = Convert.ToInt32(contents[6]);
                            }
                            catch (Exception e)
                            {
                                error = true;
                            }
                            looping = error;
                            if (!error)
                            {
                                Program.context.Leden.Add(lid);
                                Program.context.SaveChanges();
                            }
                            break;
                        default:
                            index++;
                            break;
                    }
                }

            }
            ConsoleWriting.MainMenu();

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

            printMe = " -- Press any key to continue -- ";
            Console.SetCursorPosition(Console.WindowWidth / 2 - printMe.Length / 2, Console.WindowHeight - 2);
            Console.Write(printMe);

            Console.ReadKey(true);

            ConsoleWriting.MainMenu();
        }

        public static void GoodBye()
        {
            Console.Clear();
            String printMe = "-- Goodbye --";
            Console.SetCursorPosition(Console.WindowWidth / 2 - printMe.Length / 2, Console.WindowHeight/2);
            Console.Write(printMe);
        }

        public static void MainMenu()
        {
            bool looping = true;
            String[] options = { "List members", "Add member", "Yet another one", "last one I swear", "Exit" };
            Console.Clear();
            String printMe = "- Main Menu -";
            Console.SetCursorPosition(Console.WindowWidth / 2 - printMe.Length / 2, 2);
            Console.Write(printMe);
            int index = 0;
            while (looping)
            {
                bool jump = false;
                for (int i = 0; i < options.Length; i++)
                {
                    Console.SetCursorPosition(4, 4 + i);
                    Console.Write(i == index ? " >" : "  ");
                    Console.Write((i + 1) + " ");
                    Console.Write(options[i]);
                }
                ConsoleKeyInfo k = Console.ReadKey(true);
                switch (k.Key)
                {
                    case ConsoleKey.DownArrow :
                        index++;
                        index = index > options.Length - 1 ? options.Length - 1 : index;
                        break;
                    case ConsoleKey.UpArrow :
                        index--;
                        index = index < 0 ? 0 : index;
                        break;
                    case ConsoleKey.Enter :
                        jump = true;
                        break;
                    default:
                        if (Char.IsDigit(k.KeyChar))
                        {
                            int d = Convert.ToInt32(k.KeyChar.ToString()) - 1;
                            if (d < options.Length)
                            {
                                jump = true;
                                index = d;
                            }
                        }
                        break;
                }
                if (jump)
                {
                    switch (index)
                    {
                        case 0:
                            listMembers(Program.context.Leden.ToList<Leden>());
                            looping = false;
                            break;
                        case 1:
                            addMember();
                            looping = false;
                            break;
                        case 4:
                            looping = false;
                            ConsoleWriting.GoodBye();
                            break;
                        default:
                            break;
                    }
                }

            }
        }
    }
}
