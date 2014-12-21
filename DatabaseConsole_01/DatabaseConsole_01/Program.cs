using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConsole_01
{
    class Program
    {
        //File directories
        private static String[] dirOne = {
@"S:\Git\databases\ledenEnBoetes.txt",
@"S:\Files\databases\databases\databases\ledenEnBoetes.txt", 
@"YourDirHere"
                                         };
        private static String[] dirTwo = {
@"S:\Git\databases\teamsEnWedstrijden.txt",
@"S:\Files\databases\databases\databases\teamsEnWedstrijden.txt",
@"YourDirHere"
                                         };

        private static String[,] gridOne;// [ column , row ]
        private static String[,] gridTwo;

        private static List<String> leden;
        private static List<String> boetes;
        private static List<String> teams;
        private static List<String> wedstrijden;

        private static ClubDbContext context;
        static void Main(string[] args)
        {
            Dictionary<Leden, string> teamKey = new Dictionary<Leden, string>();
            ConsoleWriting.WriteCreatingDB();

            context = new ClubDbContext();
            context.Database.Delete();

            leden = new List<string>();
            boetes = new List<string>();
            teams = new List<string>();
            wedstrijden = new List<string>();

            // Split text (file) to 2d String array.
            foreach (String dir in dirOne)
            {
                if (File.Exists(dir))
                {
                    gridOne = fileToGrid(File.ReadAllText(dir));
                }
            }
            foreach (String dir in dirTwo)
            {
                if (File.Exists(dir))
                {
                    gridTwo = fileToGrid(File.ReadAllText(dir));
                }
            }
            //gridOne = fileToGrid(File.ReadAllText(dirOne));
            //gridTwo = fileToGrid(File.ReadAllText(dirTwo));

            for (int i = 0; i < gridOne.GetLength(1) - 1; i++)
            {
                // Insert Memebers
                Leden lid = new Leden();
                lid.adres = getColumn("adres")[i];
                lid.gebjaar = Convert.ToInt32(getColumn("gebjaar")[i]);
                lid.naam = getColumn("naam")[i];
                lid.jaarlid = Convert.ToInt32(getColumn("jaarlid")[i]);
                lid.s = getColumn("s")[i];
                lid.snummer = Convert.ToInt32(getColumn("snummer")[i]);
                if (!teamKey.ContainsKey(lid) && getColumn("team")[i] != "")
                {
                    teamKey.Add(lid, getColumn("team")[i]);
                }


                context.Leden.Add(lid);
                ConsoleWriting.UpdateProgress(1);

                // Insert Penalties
                Boetes boete = new Boetes();
                boete.bedrag_boete = Convert.ToInt32(getColumn("bedrag_boete")[i] == "" ? "0" : getColumn("bedrag_boete")[i]);
                boete.speler = (lid);
                boete.toelichting = getColumn("toelichting")[i];

                context.Boetes.Add(boete);
                ConsoleWriting.UpdateProgress(1);
            }
            context.SaveChanges();

            for (int i = 0; i < gridTwo.GetLength(1) - 1; i++)
            {
                Zalen zaal = new Zalen();
                Training training = new Training();
                Teams team = new Teams();
                Wedstrijden wedstrijd = new Wedstrijden();

                // Insert Gyms

                zaal.zaaladres = getColumn("zaaladres")[i];
                zaal.trainzaal = getColumn("trainzaal")[i];
                zaal.zaaltel = getColumn("zaaltel")[i];

                if (!context.Zalen.Local.Any(b => b.trainzaal == zaal.trainzaal))
                {
                    context.Zalen.Add(zaal);
                    ConsoleWriting.UpdateProgress(1);
                }

                // Insert Teams

                team.teamcode = getColumn("teamcode")[i];
                team.trainer = context.Leden.Local.Single(b => b.snummer == Convert.ToInt32(getColumn("trainer")[i]));
                team.leeftijdsklasse = getColumn("leeftijdsklasse")[i];
                team.sexe = getColumn("sexe")[i];
                foreach (Leden l in from b in teamKey where b.Value == team.teamcode select b.Key)
                {
                    team.spelers.Add(l);
                }

                if (!context.Teams.Local.Any(b => b == team))
                {
                    context.Teams.Add(team);
                    ConsoleWriting.UpdateProgress(1);
                }

                // Insert Game
                try
                {
                    wedstrijd.scheids = context.Leden.Local.Single(b => b.snummer == Convert.ToInt32(getColumn("scheids")[i]));
                }
                catch (FormatException e)
                {
                    wedstrijd.scheids = null;
                }
                wedstrijd.wedstrijddatum = getColumn("wedstrijddatum")[i];
                wedstrijd.tegenstander = getColumn("tegenstander")[i];
                wedstrijd.wedstrijdzaal = context.Zalen.Local.Single(b => b.trainzaal == getColumn("wedstrijdzaal")[i]);
                wedstrijd.teamcode = team;

                // Insert Trains

                if (!team.trainingen.Any(b => b.traindag == Convert.ToInt32(getColumn("traindag")[i])))
                {
                    try
                    {
                        training.traindag = Convert.ToInt32(getColumn("traindag")[i]);
                    }
                    catch (FormatException e)
                    {
                        training.traindag = 0;
                    }
                    try
                    {
                        training.trainlengte = Convert.ToInt32(getColumn("trainlengte")[i]);
                    }
                    catch (FormatException e)
                    {
                        training.trainlengte = 0;
                    }
                    training.trainzaal = context.Zalen.Local.Single(b => b.trainzaal == getColumn("trainzaal")[i]);
                    team.trainingen.Add(training);
                }
            }
            try
            {
                context.SaveChanges();
                ConsoleWriting.UpdateProgress(1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                ConsoleWriting.listMembers(context.Leden.ToList<Leden>());
            }
        }

        private static List<String> getColumn(String name)
        {
            List<String> l = new List<string>();
            for (int i = 0; i < gridOne.GetLength(0); i++)
            {
                if (gridOne[i, 0].Trim('"') == name.Trim('"'))
                {
                    for (int j = 1; j < gridOne.GetLength(1); j++)
                    {
                        l.Add(gridOne[i, j].Trim('"'));

                    }
                }
            }
            for (int i = 0; i < gridTwo.GetLength(0); i++)
            {
                if (gridTwo[i, 0].Trim('"') == name.Trim('"'))
                {
                    for (int j = 1; j < gridTwo.GetLength(1); j++)
                    {
                        l.Add(gridTwo[i, j].Trim('"'));

                    }
                }
            }
            return l;
        }

        private static String[,] fileToGrid(String file)
        {
            String[] records = file.Split('\n');// String split on LineFeed, the last character after each record.
            int columnCount = records[0].Split(',').Count();
            String[,] grid = new String[columnCount, records.Count() - 1];
            for (int y = 0; y < records.Count() - 1; y++)// Ensures last element is skipped, which is empty.
            {   // Each row split on comma, the character between each cell.
                String[] cells = records[y].Split(',');
                for (int x = 0; x < columnCount; x++)
                {
                    grid[x, y] = cells[x];
                }
            }
            return grid;
        }
    }
}
