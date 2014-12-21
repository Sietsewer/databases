using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportDB
{
    class Program
    {
        static WPFSportDbContext _dbContext;
        static void Main(string[] args)
        {
            Dictionary<int, string> playerTeam = new Dictionary<int, string>();
            Console.WriteLine("<<<<CREATE DATABASE>>>>");
            _dbContext = new WPFSportDbContext();
            _dbContext.Database.Delete();
            string[] lines = File.ReadAllLines(@"S:\Files\databases\databases\databases\ledenEnBoetes.txt");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] splitted = lines[i].Split(new string[] { "," }, StringSplitOptions.None);
                //"gebjaar","s","jaarlid","snummer","adres","naam","bnummer","team","bedrag_boete","boete_wedstrijd","toelichting"
                //1970,     "m",1990,1,"spoorstraat 24","Pietersen",23,"s5",,,""
                List<string> values = new List<string>(splitted);
                for (int w = 0; w < values.Count; w++)
                {
                    values[w] = values[w].Replace("\"", "");
                }
                Lid lid = new Lid();
                if (string.IsNullOrEmpty(values[6]) == false)
                {
                    lid = new WedstrijdtSpeler();
                    ((WedstrijdtSpeler)lid).Bondsnummer = int.Parse(values[6]);
                }
               
                lid.SpelersNummer = int.Parse(values[3]);
                if (string.IsNullOrEmpty(values[7]) == false)
                {
                    if (playerTeam.ContainsKey(lid.SpelersNummer) == false)
                    {
                        playerTeam.Add(lid.SpelersNummer, values[7]);
                    }
                }
                if (!Lidadded(lid.SpelersNummer))
                {
                    lid.GeboorteJaar = int.Parse(values[0]);
                    lid.Geslacht = values[1];
                    lid.ToetredingsJaar = int.Parse(values[2]);
                    lid.Adres = values[4];
                    lid.Naam = values[5];

                    if (string.IsNullOrEmpty(values[6]) == false)
                    {
                        _dbContext.WedstrijdtSpelers.Add((WedstrijdtSpeler)lid);
                    }
                    
                    _dbContext.Leden.Add(lid);

                }
                //"bedrag_boete","boete_wedstrijd","toelichting"
                   if (string.IsNullOrEmpty(values[8]) == false)
                  {
                      Boete b = new Boete();
                      b.Bedrag = int.Parse(values[8]);
                      b.Wedstrijdt = getWedstrijd(int.Parse(values[9]));
                      b.Speler = (lid);
                      if (string.IsNullOrEmpty(values[10]) == false)
                      {
                          b.Toelichting = values[10];
                      }
                     _dbContext.Boetes.Add(b);
                  }



              }
            _dbContext.SaveChanges();



                //read file 2
                /*
    "teamcode"
     ,"trainer"
    ,"leeftijdsklasse","sexe","4traindag","5traintijd","6trainlengte","7trainzaal","8zaaladres","9zaaltel","10wedstrijdnummer","11scheids","12wedstrijddatum","13wedstrijdtijd","14tegenstander","15wedstrijdzaal"
    "g1",3,"gemegnd","o",2,"",2,"fondsbad","neerslag 5","058-9887765",1,24,"2015-01-30","","zuid 12","fondsbad"*/


            lines = File.ReadAllLines(@"S:\Files\databases\databases\databases\teamsEnWedstrijden.txt");
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(new string[] { "," }, StringSplitOptions.None);
                    List<string> values = new List<string>(parts);
                    for (int w = 0; w < values.Count; w++)
                    {
                        values[w] = values[w].Replace("\"", "");
                    }
                    //"7trainzaal","8zaaladres","9zaaltel"
                    if (string.IsNullOrWhiteSpace(values[9]) == false)
                    {
                        Zaal z = new Zaal();
                        z.TelefoonNR = values[9];
                        z.Adres = values[8];
                        z.Naam = values[7];
                        if (_dbContext.zalen.Local.Any(b => b.Naam == z.Naam) == false)
                        {
                            _dbContext.zalen.Add(z);
                        }
                    }
                    /*"teamcode"
 ,"trainer"
,"leeftijdsklasse","sexe"*/
                    Team t = getTeam(values[0]);
                    t.TeamCode = values[0];
                    t.Trainer = _dbContext.Leden.Local.Single(b => b.SpelersNummer == int.Parse(values[1]));
                    t.LeeftijdsKlasse = values[2];
                    t.Sexe = values[3];

                    t.Spelers.AddRange(playerTeam.Where(b => b.Value == t.TeamCode).Select(c => _dbContext.Leden.Local.Where(s => s.SpelersNummer == c.Key).First()).ToList());
                    if (_dbContext.Teams.Local.Any(b => b == t) == false)
                    {
                        _dbContext.Teams.Add(t);
                    }
                    /*"8zaaladres","9zaaltel",
                     * "10wedstrijdnummer","11scheids",
                     * "12wedstrijddatum","13wedstrijdtijd",
                     * "14tegenstander","15wedstrijdzaal"*/
                     if (string.IsNullOrWhiteSpace(values[10]) == false)
                      {
                          Wedstrijd w =  getWedstrijd(int.Parse(values[10]));
                          w.Scheidsrechter = _dbContext.Leden.Local.Single(b=>b.SpelersNummer == int.Parse(values[11]));
                          w.WedstrijdtDatum = values[12];
                          w.GespeeldTegen = values[14];
                          w.GespeeldIn = _dbContext.zalen.Local.Single(b=> b.Naam== values[15]);
                          w.Team = t;
                      }
                      //"0teamcode","4traindag","5traintijd","6trainlengte","7trainzaal"
                      if (string.IsNullOrWhiteSpace(values[4]) == false)
                      {
                          if (t.Trainingen.Any(b => b.dagVanWeek == int.Parse(values[4])) == false)
                          {
                              Training train = new Training();
                              train.dagVanWeek = int.Parse(values[4]);
                              train.Duur = int.Parse(values[6]);
                              train.zaal = _dbContext.zalen.Local.Single(b=> b.Naam == values[7]);
                              t.Trainingen.Add(train);
                          }
                      }
                }
            

               for(int i=0;i< _dbContext.Leden.Local.Count;i++)
               {
                   if(_dbContext.Leden.Local[i].Naam == null)
                   {
                       _dbContext.Leden.Local.RemoveAt(i);
                       i--;
                   }
               }
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                    }

                    Console.WriteLine("<<<<DATABASE CREATED>>>>");
                    ShowMenu();
            
        }

        static void ShowMenu()
        {
            Console.WriteLine("<<<<MENU>>>>");
            Console.WriteLine("1. ADD MEMBER");
            Console.WriteLine("2. EDIT MEMBER");
            Console.WriteLine("3. REMOVE MEMBER");
            Console.WriteLine("4. REFEREE AT NHL");
            Console.WriteLine("5. RECREATION TEAMS");
            Console.WriteLine("6. PLAYER MOST FINES");
            Console.WriteLine("7. SHOW MEMBERS");
            Console.WriteLine("<<<<GIVE OPTION>>>");
            int chose;
            if (int.TryParse(Console.ReadLine(), out chose))
            {
                if(chose>=1 && chose<=7)
                {
                    switch(chose)
                    {
                        case 1:
                            addMember();
                            Console.Clear();
                            ShowMenu();
                            break;
                        case 2:
                            editMember();
                            Console.Clear();
                            ShowMenu();
                            break;
                        case 3:
                            removeMember();
                            Console.Clear();
                            ShowMenu();
                            break;
                        case 4:
                            Console.Clear();
                            refreeAtNHL();
                            Console.ReadLine();
                            Console.Clear();
                            ShowMenu();
                            break;
                        case 5:
                            Console.Clear();
                            recreationTeams();
                            Console.ReadLine();
                            Console.Clear();
                            ShowMenu();
                            break;

                        case 6:
                            Console.Clear();
                            mostFine();
                            Console.ReadLine();
                            Console.Clear();
                            ShowMenu();
                            break;


                        case 7:
                            Console.Clear();
                            showMembers();
                            Console.ReadLine();
                            Console.Clear();
                            ShowMenu();
                            break;
                        default:
                            Console.Clear();
                            ShowMenu();
                            break;

                    }
                }
                else
                {
                    Console.Clear();
                    ShowMenu();
                }
            }
            else
            {
                Console.Clear();
                ShowMenu();
            }


        }

        private static void recreationTeams()
        {
          var recteams =  _dbContext.Teams.Where(b => b.Spelers.All(s => s.ToetredingsJaar - s.GeboorteJaar >= 30) && b.Spelers.Count !=0);
            foreach(var team in recteams)
            {
                Console.WriteLine("<<Team: " + team.TeamCode + ">>");
                Console.WriteLine("Sexe:"+team.Sexe);
                Console.WriteLine("LeeftijdsKlasse:" + team.LeeftijdsKlasse);
                Console.WriteLine("LeeftijdsKlasse:" + team.Trainer);
                Console.WriteLine("<<TRAINER:" + team.Trainer.LidId + ">>");
                Console.WriteLine("Naam:" + team.Trainer.Naam);
                Console.WriteLine("Adres:" + team.Trainer.Adres);
                Console.WriteLine("Geboortejaar:" + team.Trainer.GeboorteJaar);
                Console.WriteLine("Geslacht:" + team.Trainer.Geslacht);
                Console.WriteLine("Spelersnummer:" + team.Trainer.SpelersNummer);
                Console.WriteLine("Toetredings jaar:" + team.Trainer.ToetredingsJaar);
                Console.WriteLine("<<>>");
                foreach(var lid in team.Spelers)
                {
                    Console.WriteLine("<<" + lid.LidId + ">>");
                    Console.WriteLine("Naam:" + lid.Naam);
                    Console.WriteLine("Adres:" + lid.Adres);
                    Console.WriteLine("Geboortejaar:" + lid.GeboorteJaar);
                    Console.WriteLine("Geslacht:" + lid.Geslacht);
                    Console.WriteLine("Spelersnummer:" + lid.SpelersNummer);
                    Console.WriteLine("Toetredings jaar:" + lid.ToetredingsJaar);
                    Console.WriteLine("<<>>");
                }
                Console.WriteLine("<<>>");
            }

        }

        private static void mostFine()
        {
            var query = from t in _dbContext.Boetes
                        group t by new { a = t.Speler, b = t.Bedrag} into g
                        select g;
            var max = query.First(c => c.Key.b == query.Max(b => b.Key.b));
            var lid = max.Key.a;
            Console.WriteLine("Boete:" + max.Key.b);
            Console.WriteLine("<<" + lid.LidId + ">>");
            Console.WriteLine("Naam:" + lid.Naam);
            Console.WriteLine("Adres:" + lid.Adres);
            Console.WriteLine("Geboortejaar:" + lid.GeboorteJaar);
            Console.WriteLine("Geslacht:" + lid.Geslacht);
            Console.WriteLine("Spelersnummer:" + lid.SpelersNummer);
            Console.WriteLine("Toetredings jaar:" + lid.ToetredingsJaar);
            Console.WriteLine("<<>>");


        }

        private static void refreeAtNHL()
        {
            var refrees = _dbContext.Wedstrijden.Where(b => b.GespeeldIn.Naam == "NHL").Select(c => c.Scheidsrechter).ToList();
            foreach(var lid  in refrees)
            {
                Console.WriteLine("<<" + lid.LidId + ">>");
                Console.WriteLine("Naam:" + lid.Naam);
                Console.WriteLine("Adres:" + lid.Adres);
                Console.WriteLine("Geboortejaar:" + lid.GeboorteJaar);
                Console.WriteLine("Geslacht:" + lid.Geslacht);
                Console.WriteLine("Spelersnummer:" + lid.SpelersNummer);
                Console.WriteLine("Toetredings jaar:" + lid.ToetredingsJaar);
                Console.WriteLine("<<>>");
            }
        }

        private static void removeMember()
        {

            Console.WriteLine("GIVE ID OF MEMBER:");
            int memberID = GetInt();
            while (_dbContext.Leden.Where(b => b.LidId == memberID).Count() == 0)
            {
                Console.WriteLine("MEMBER NOT EXISTING, TRY AGAIN");
                memberID = GetInt();
            }
            _dbContext.Leden.Remove(_dbContext.Leden.Single(b => b.LidId == memberID));
            _dbContext.SaveChanges();
        }

        private static void editMember()
        {
            Console.WriteLine("GIVE ID OF MEMBER:");
            int memberID = GetInt();
            while (_dbContext.Leden.Where(b => b.LidId == memberID).Count() == 0)
            {
                Console.WriteLine("MEMBER NOT EXISTING, TRY AGAIN");
                memberID = GetInt();
            }
            Lid lid = _dbContext.Leden.Single(b => b.LidId == memberID);
            Console.WriteLine("<<" + lid.LidId + ">>");
            Console.WriteLine("Naam:" + lid.Naam);
            Console.WriteLine("Change naam:");
            lid.Naam = Console.ReadLine();
            Console.WriteLine("Adres:" + lid.Adres);
            Console.WriteLine("Change adres:");
            lid.Adres = Console.ReadLine();

            Console.WriteLine("Geboortejaar:" + lid.GeboorteJaar);
            Console.WriteLine("Change geboortejaar:");
            lid.GeboorteJaar = GetInt();

            Console.WriteLine("Geslacht:" + lid.Geslacht);
            Console.WriteLine("Change geslacht:");
            lid.Geslacht = Console.ReadLine();


            Console.WriteLine("Spelersnummer:" + lid.SpelersNummer);
            Console.WriteLine("Change spelersnummer:");
            lid.SpelersNummer = GetInt();


            Console.WriteLine("Toetredings jaar:" + lid.ToetredingsJaar);
            Console.WriteLine("Change toetredings jaar:");
            lid.ToetredingsJaar = GetInt();

            Console.WriteLine("<<>>");
            _dbContext.SaveChanges();

        }

        private static void showMembers()
        {
            foreach (var lid in _dbContext.Leden)
            {
                Console.WriteLine("<<"+lid.LidId+">>");
                Console.WriteLine("Naam:" + lid.Naam );
                Console.WriteLine("Adres:" + lid.Adres);
                Console.WriteLine("Geboortejaar:" + lid.GeboorteJaar);
                Console.WriteLine("Geslacht:" + lid.Geslacht);
                Console.WriteLine("Spelersnummer:" + lid.SpelersNummer);
                Console.WriteLine("Toetredings jaar:" + lid.ToetredingsJaar);
                Console.WriteLine("<<>>");
            }
        }

        private static void addMember()
        {
            Lid l = new Lid();
            Console.Clear();
            Console.WriteLine("Naam:");
            l.Naam = Console.ReadLine();
               Console.WriteLine("Geslacht:");
               l.Geslacht = Console.ReadLine();
               Console.WriteLine("Adres:");
               l.Adres = Console.ReadLine();

               Console.WriteLine("Geboortejaar:");
               l.GeboorteJaar = GetInt();
               Console.WriteLine("Spelersnummer:");
               l.SpelersNummer = GetInt();
               Console.WriteLine("Toetredings jaar:");
               l.ToetredingsJaar = GetInt();

              _dbContext.Leden.Add(l);
              _dbContext.SaveChanges();

        }
        static int GetInt()
        {
            int number;
            while(!int.TryParse(Console.ReadLine() , out number))
            {
                Console.WriteLine("Not a number try again:");
            }
            return number;
        }

        private static Wedstrijd getWedstrijd(int p)
        {
            if (_dbContext.Wedstrijden.Local.Any(c => c.WedstrijdtNummer == p))
            {
                return _dbContext.Wedstrijden.Local.Single(b => b.WedstrijdtNummer == p);
            }
            else
            {
                Wedstrijd w = new Wedstrijd() { WedstrijdtNummer = p };
                _dbContext.Wedstrijden.Add(w);
                return w;
            }
        }

        private static Team getTeam(string p)
        {
            if (_dbContext.Teams.Local.Any(c => c.TeamCode == p))
            {
                return _dbContext.Teams.Local.Single(b => b.TeamCode == p);
            }
            else
            {
                return new Team() { TeamCode = p };
            }
        }

        private static bool Lidadded(int spelersNummer)
        {
            return _dbContext.Leden.Local.Any(b => b.SpelersNummer == spelersNummer);
        }
    }
    class Pair<T, X>
    {
        public T Key { get; set; }
        public X Value { get; set; }
    }
}
