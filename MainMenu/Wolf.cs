using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Wolf
    {
        public int lvl = 1;
        public int exp = 0;
        public int strange = 5;
        public int agility = 3;
        public int wisdom = 1;
        public int health = 75;
        public int stamina = 30;
        public int hunger = 100;
        public int territory = 0;

        public string GetParams()
        {
            string stats = ("level - " + lvl + "\nexperience - " + exp + "/" + (System.Math.Pow(4, lvl)) + "\nstrange - " + strange + "\nagility - " + agility + "\nwisdom - " + wisdom + "\nhealth - " + health + "/" + (strange * 15) + "\nstamina - " + stamina + "/" + (agility * 10) + "\nhunger - " + hunger + "/100" + "\nexplored - " + (territory / 10) + "%\n");
            return stats;
        }
        public string GetParamsToSave()
        {
            string stats = (lvl + "\n" + exp + "\n" + strange + "\n" + agility + "\n" + wisdom + "\n" + health + "\n" + stamina + "\n" + hunger + "\n" + territory);
            return stats;
        }
        private int CheckLevelUp(int lvl, int exp)
        {
            int check = Convert.ToInt32(System.Math.Pow(4, lvl));
            if (exp >= check)
            {
                strange += 4;
                agility += 3;
                wisdom += 2;
                lvl++;
                lvl = CheckLevelUp(lvl, exp);
            }
            return lvl;
        }
        public bool CheckStats(int health, int stamina, int hunger)
        {
            if (health > strange * 15) this.health = strange * 15;
            if (stamina > agility * 10) this.stamina = agility * 10;
            if (hunger > 100) this.hunger = 100;
            if (hunger < 0) this.hunger = 0;
            if (stamina < 0) this.stamina = 0;
            if (health <= 0)
            {
                return false;
            }
            return true;
        }
        public bool CheckTerritory()
        {
            if (territory >= 1000) return false;
            return true;
        }
        public bool DoSleep()
        {
            var rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                hunger -= rnd.Next(6);
                if (hunger <= 0)
                {
                    health -= strange;
                    stamina -= agility;
                }
                stamina += agility * 1;
                health += strange / 3;
            }
            exp += wisdom * lvl * (rnd.Next(10) + 1);
            lvl = CheckLevelUp(lvl, exp);
            return CheckStats(health, stamina, hunger);
        }
        public bool DoRest()
        {
            hunger -= 4;
            if (hunger < 0)
            {
                health -= strange / 2;
                stamina -= agility;
            }
            health += strange / 5;
            stamina += agility * 3;
            return CheckStats(health, stamina, hunger);
        }
        public bool DoExploration()
        {
            if (stamina < lvl * 5)
            {
                Console.WriteLine("The wolf is too tired\nGo to sleep or get rest");
                return true;
            }
            var rnd = new Random();
            for (int i = 0; i < 3; i++)
            {
                health -= rnd.Next(strange) / 3;
                stamina -= rnd.Next(lvl * 5);
                hunger -= rnd.Next(4);
                exp += (wisdom) * rnd.Next(100);
                territory += (wisdom) * rnd.Next(5);
            }
            switch (rnd.Next(10))
            {
                case 0:
                    {
                        Console.WriteLine("You found some herbs");
                        health += (wisdom / 10) * rnd.Next(10) * strange;
                        break;
                    }
                case 1:
                    {
                        Console.WriteLine("You found a good place to rest");
                        stamina += (wisdom / 10) * rnd.Next(10) * agility;
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("You found some food");
                        hunger += (wisdom / 10) * rnd.Next(20);
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("You got stronger");
                        exp += wisdom * rnd.Next(100);
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("You explored much more lands");
                        territory += wisdom * rnd.Next(10);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("You didn't find any interesting");
                        break;
                    }
            }
            lvl = CheckLevelUp(lvl, exp);
            return CheckStats(health, stamina, hunger);
        }
        public bool DoHunt()
        {
            if (stamina < 2 * agility)
            {
                Console.WriteLine("The wolf is too tired\nGo to sleep or get rest");
                return true;
            }
            // get random animal
            var rnd = new Random();
            int rndAnimalLevel = rnd.Next(lvl) + 1;
            int rndAnimalStr = 0;
            int rndAnimalAgi = 0;
            int rndAnimalWis = 0;
            for (int i = 0; i < rndAnimalLevel * 3; i++)
            {
                switch (rnd.Next(3))
                {
                    case 0:
                        {
                            rndAnimalStr++;
                            break;
                        }
                    case 1:
                        {
                            rndAnimalAgi++;
                            break;
                        }
                    case 2:
                        {
                            rndAnimalWis++;
                            break;
                        }
                    default:
                        break;
                }
            }
            int rndAnimalHP = rndAnimalStr * 30;
            int rndAnimalSTM = rndAnimalAgi * 30;
            //^^^
            health -= rndAnimalHP;
            stamina -= rndAnimalSTM;
            hunger += rndAnimalLevel * 10;
            exp += rndAnimalWis * 100;
            lvl = CheckLevelUp(lvl, exp);
            return CheckStats(health, stamina, hunger);
        }
    }
}
