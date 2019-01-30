using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace MainMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            // using
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            bool accaunt = false;
            bool login = false;
            string userName = "guest";
            string log = "";
            var wolf = new ClassLibrary.Wolf();
            //^^^
            Intro();
            do
            {
                CheckLogin(ref accaunt, login, ref userName, ref log);
			    // writing Action menu
                cki = GetMainMenu(cki, userName, log);
			    //^^^
			    // processing choice
                GetChoice(ref cki, ref accaunt, ref login, ref userName, wolf);
            } while (cki.Key != ConsoleKey.D0 && cki.Key != ConsoleKey.NumPad0);
        }

        private static void GetChoice(ref ConsoleKeyInfo cki, ref bool accaunt, ref bool login, ref string userName, ClassLibrary.Wolf wolf)
        {
            switch (cki.Key)
            {
                // action 0 (exit)
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    {
                        break;
                    }
                //^^^
                // action 1 (log)
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    {
                        if (login == true)
                        {
                            login = false;
                            break;
                        }

                        ClearConsole();
                        UserList(0, userName); // show userlist
                        userName = LogIn();
                        login = true;
                        accaunt = CheckPath("save", userName);
                        break;
                    }
                // Create new accaunt
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    {
                        if (login == false)
                        {
                            Console.WriteLine("You must to log in first");
                            break;
                        }
                        if (CheckPath("save", userName) == true)
                        {
                            Console.WriteLine("You alredy have the wolf\nIf you want to create new wolf you should to delete this one");
                        }
                        else
                        {
                            string stats = wolf.GetParamsToSave();
                            FileWolf(GetPath("save", userName), stats, userName, 0);
                            accaunt = true;
                        }
                        break;
                    }
                //^^^
                // deleting
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    {
                        if (accaunt == true)
                        {
                            FileWolf(GetPath("save", userName), "", userName, 2);
                            accaunt = false;
                        }
                        else Console.WriteLine("You have not character to delete");
                        break;
                    }
                //^^^
                // Go to task menu
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    {
                        if (accaunt == true)
                            accaunt = (Menu.ActionMenu(userName));
                        else Console.WriteLine("You need to create a character!");
                        break;
                    }
                //^^^
                // Autors & about
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    {
                        ShowAbout();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid comand");
                        break;
                    }
            }
            ClearConsole();
        }

        private static ConsoleKeyInfo GetMainMenu(ConsoleKeyInfo cki, string userName, string log)
        {
            string[] menu = new string[6] { "exit", log, "create new character", "delete your character", "open action menu", "show autors & about" };
            Console.WriteLine("                    Welcome to menu, " + userName + "!");
            for (int i = 0; i < 6; i++)
            {
                Console.Write("Press <{0}> to ", i);
                Console.WriteLine(menu[i]);
            }
            Console.Write("your choice - ");
            cki = Console.ReadKey();
            Console.WriteLine();
            return cki;
        }

        private static void CheckLogin(ref bool accaunt, bool login, ref string userName, ref string log)
        {
            if (login == false || userName == "guest")
            {
                accaunt = false;
                log = "log in";
                userName = "guest";
            }
            else log = "log out";
        }
        public static void Intro()
        {
            Console.WriteLine("\n\n\n                        Adventures of the wandering wolf");
            Thread.Sleep(3000);
            ClearConsole();
        }
        public static void ClearConsole()
        {
            string[] chars = new string[7] { "\n                                 L ", "O ", "A ", "D ", "I ", "N ", "G " };

            for (int i = 0; i < 7; i++)
            {
                Console.Write(chars[i]);
                Thread.Sleep(500);
            }
            Console.Clear();
        }
        public static void ShowAbout()
        {            
            ClearConsole();
            Console.Write("\n\n\n                        Adventures of the wandering wolf\n\n                        Project manager - Dmitriy Frolov\n                      Idea - Dmitriy Frolov & Diana Ratova");
            Console.Write("\n\n                 The task of this game is explore all the lands\n    When you're sleaping, hunting or explore lands you're geting experience");
            Console.Write("\n               Also you must to look after you health & hunger...\n                      If you helth fall to zero you'll die");
            Console.Write("\n          If you want to support me you can buy for me some cookies :3\n                       Special thanks to rainbow squirrel");
            Console.Write("\n\n                                   Have Fun!\n\n\n\n                          Press any key to continue");
            Console.ReadKey();
        }
        /// <summary>
        /// Return full file path (string)
        /// </summary>
        /// <param name="prefix">save;list;</param>
        /// <param name="userName">username</param>
        /// <returns></returns>
        public static string GetPath(string prefix, string userName)
        {
            return (Path.GetFullPath(prefix) + userName + ".txt");
        }
        /// <summary>
        /// Checking file path (bool)
        /// </summary>
        /// <param name="prefix">save;list;</param>
        /// <param name="userName">username</param>
        /// <returns></returns>
        public static bool CheckPath(string prefix, string userName)
        {            
            string path = (Path.GetFullPath(prefix) + userName + ".txt");
            if (File.Exists(path))
                return true;
            else return false;
        }
        /// <summary>
        /// Userlist actions
        /// </summary>
        /// <param name="action"> 0 - show; 1 - add; 2 - remove </param>
        /// <param name="wolfName"> character name </param>
        public static void UserList(int action, string userName)
        {
            string pathList = @"listOfUsers.txt";
            pathList = (Path.GetFullPath(pathList));
            if (!File.Exists(pathList))
            {
                using (StreamWriter sw = File.CreateText(pathList))
                {
                    sw.Write("");
                }
            }

            var userList = new List<string>();
            string buffer = "";
            using (StreamReader sr = File.OpenText(pathList))
                while ((buffer = sr.ReadLine()) != null)
                {
                    userList.Add(buffer);
                }
            buffer = "";

            switch (action)
            {
                // Show userlist
                case 0:
                    {
                        Console.WriteLine("User list:\n");
                        foreach (var name in userList)
                        {
                            Console.WriteLine(name);
                        }
                        Console.WriteLine();
                        break;
                    }
                //^^^
                // add username to userlist
                case 1:
                    {
                        userList.Add(userName);
                        foreach (var name in userList)
                        {
                            buffer += name + "\n";
                        }
                        using (StreamWriter sw = File.CreateText(pathList)) sw.Write(buffer);
                        break;
                    }
                //^^^
                // remove name from userlist
                case 2:
                    {
                        userList.RemoveAll(x => x.Contains(userName));
                        foreach (var name in userList)
                        {
                            buffer += name + "\n";
                        }
                        using (StreamWriter sw = File.CreateText(pathList)) sw.Write(buffer);

                        break;
                    }
                //^^^
                default:
                    break;
            }
        }
        /// <summary>
        /// working with save files
        /// </summary>
        /// <param name="path">path to file</param>
        /// <param name="stats">string of stats</param>
        /// <param name="wolfName">accaunt name</param>
        /// <param name="action">0 - create; 1 - save; 2 - delete</param>
        public static void FileWolf(string path, string stats, string wolfName, int action)
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                if (action == 0)
                {
                    sw.Write(stats);
                    Console.WriteLine("\n                                  Wolf created");
                    UserList(1, wolfName);
                }
                if (action == 1)
                {
                    sw.Write(stats);
                    Console.WriteLine("\n                                   Wolf saved");
                }
            }
            if (action == 2)
            {
                File.Delete(path);
                UserList(2, wolfName);
                Console.WriteLine("\n                             The character deleted");
            }
        }
        public static string LogIn()
        {
            bool checkName = false;
            string userName;
            do
            {
                Console.Write("Please enter your name: ");
                userName = Console.ReadLine();
                for (int i = 0; i < userName.Length; i++)
                {
                    if (userName == "guest")
                    {
                        Console.WriteLine("You can't use this name");
                        checkName = false;
                        break;
                    }
                    else if ((userName[i] >= 65 && userName[i] <= 90) || (userName[i] >= 97 && userName[i] <= 122) || (userName[i] >= 48 && userName[i] <= 57))
                    {
                        checkName = true;
                    }
                    else
                    {
                        Console.WriteLine("You can use only Latin alphabet and numbers");
                        checkName = false;
                        break;
                    }
                }
            } while (!checkName);
            return userName;
        }
    }
}
