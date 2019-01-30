using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace MainMenu
{
    class Menu
    {
        public static bool ActionMenu(string userName)
        {
            // verify
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            var wolf = new ClassLibrary.Wolf();
            string[] menu = new string[5] { "save & back to main menu\n__________", "go to sleep", "get rest", "explore lands", "go on a hunt" };
            using (StreamReader sr = File.OpenText(Program.GetPath("save",userName)))
            {
                string[] stats = new string[9];
                for (int i = 0; i < 9; i++)
                {
                    stats[i] = sr.ReadLine();
                }
                try
                {
                    LoadStats(wolf, stats);
                }
                catch (Exception)
                {
                    Console.WriteLine("This save file is corrupted");
                    return true;
                }
            }
            //^^^
            return GetChoice(userName, ref cki, wolf, menu);
        }

        private static void LoadStats(ClassLibrary.Wolf wolf, string[] stats)
        {
            wolf.lvl = Convert.ToInt32(stats[0]);
            wolf.exp = Convert.ToInt32(stats[1]);
            wolf.strange = Convert.ToInt32(stats[2]);
            wolf.agility = Convert.ToInt32(stats[3]);
            wolf.wisdom = Convert.ToInt32(stats[4]);
            wolf.health = Convert.ToInt32(stats[5]);
            wolf.stamina = Convert.ToInt32(stats[6]);
            wolf.hunger = Convert.ToInt32(stats[7]);
            wolf.territory = Convert.ToInt32(stats[8]);
        }

        private static bool GetChoice(string userName, ref ConsoleKeyInfo cki, ClassLibrary.Wolf wolf, string[] menu)
        {
            do
            {
                // writing Action menu
                cki = GetMenu(userName, cki, wolf, menu);
                //^^^
                // processing coise
                switch (cki.Key)
                {
                    // save & back to main menu
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        {
                            string stats = wolf.GetParamsToSave();
                            MainMenu.Program.FileWolf(Program.GetPath("save", userName), stats, userName, 1);
                            break;
                        }
                    //^^^
                    // go to sleep
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            if (!wolf.DoSleep())
                            {
                                ShowEndGame(userName, 0);
                                return false;
                            }
                            break;
                        }
                    //^^^
                    // get rest
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        {
                            if (!wolf.DoRest())
                            {
                                ShowEndGame(userName, 1);
                                return false;
                            }
                            break;
                        }
                    //^^^
                    // explore lands
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        {
                            if (!wolf.DoExploration())
                            {
                                ShowEndGame(userName, 2);
                                return false;
                            }
                            if (!wolf.CheckTerritory())
                            {
                                ShowEndGame(userName, 4);
                                return false;
                            }
                            break;
                        }
                    //^^^
                    // go to hunt
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        {
                            if (!wolf.DoHunt())
                            {
                                ShowEndGame(userName, 3);
                                return false;
                            }
                            break;
                        }
                    //^^^
                    default:
                        {
                            Console.WriteLine("Invalid comand");
                            break;
                        }
                }
            } while (cki.Key != ConsoleKey.D0 && cki.Key != ConsoleKey.NumPad0);
            return true;
        }

        private static ConsoleKeyInfo GetMenu(string userName, ConsoleKeyInfo cki, ClassLibrary.Wolf wolf, string[] menu)
        {
            MainMenu.Program.ClearConsole();
            Console.WriteLine("                          Welcome to action menu, " + userName + "!");
            Console.WriteLine(wolf.GetParams());

            for (int i = 0; i < 5; i++)
            {
                Console.Write("Press <{0}> to ", i);
                Console.WriteLine(menu[i]);
            }
            Console.Write("your choice - ");
            cki = Console.ReadKey();
            Console.WriteLine();
            return cki;
        }
        /// <summary>
        /// ShowEndGame
        /// </summary>
        /// <param name="live">what to check</param>
        /// <param name="userName">username</param>
        /// <param name="kindOfDeath">0-sleep;1-rest;2-explore;3-hunt;4-win</param>
        public static void ShowEndGame(string userName, int kindOfDeath)
        {
                MainMenu.Program.ClearConsole();
                switch (kindOfDeath)
                {
                    case 0:
                        {
                            Console.WriteLine("\n\n\n\n\n                          Your wolf died in his dreams\n                              Rest in peace . . .");
                            break;
                        }
                    case 1:
                        {
                            Console.WriteLine("\n\n\n\n\n              Your wolf lay down to rest but was unable to get up\n                              Rest in peace . . .");
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("\n\n\n\n\n                        Your wolf died in unknown lands\n                              Rest in peace . . .\n\n\n");
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("\n\n\n\n\n                      Your wolf died in the deadly battle\n                              Rest in peace . . .\n\n\n");
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("\n\n\n\n\n                                Congratulations!\n                           You explored all the land\n\n\n");
                            break;
                        }
                    default:
                        break;
                }
                MainMenu.Program.FileWolf(MainMenu.Program.GetPath("save",userName), "", userName, 2);
                Thread.Sleep(10000);
        }
    }
}
