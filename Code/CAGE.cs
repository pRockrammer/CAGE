using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CAG
{
    class Program
    {
        private static void Main(string[] args)
        {
            //EXAMPLE
            Player player = new Player(/*HP = */400, /*ARMOR = */10,/*DAMAGE = */ 10,/*ICON = */ "(O!");
            Console.WriteLine(@" ////    //\\     ////  //// ");
            Console.WriteLine(@"||      //  \\   ||    ||== ");
            Console.WriteLine(@"||     //====\\  || \\ ||==  ");
            Console.WriteLine(@" \\\\ //      \\  \\\\  \\\\  ");
            Console.WriteLine("      the console   engine       ");
            Console.WriteLine("Press smth....");
            ConsoleKey c = Console.ReadKey(true).Key;
            //EXAMPLE:
            GAME(new[] { "!@#", "!$)", "!O!" }, new[] { new[] { 12, 3, 45 }, new[] { 15, 5, 56 }, new[] { 14, 2, 40 } }, player, 20, new[] { "Enemy: Hello!", "You: I will kill you!", "Enemy: HA!","You: LOL!" });
        }
        static void GAME(string[] enemys, int[][] stats, Player player, int hardlevel, string[] dialog)
            //запускает битву с мобами
            //enemys - new[]{"!@#", "!$)"...}(пример)
            //stats - статы врагов перечислeнных в enemys соответсвенно new[]{new[]{12,3,45},new[]{15,5,56}..} - {{hp,armor,damage}...}
            //dialog - диалог перед битвой, пример - new[]{"Enemy: Hello!", "You: I will kill you!"...} диалоги врагов - чётные позиции, диалог игрока нечётные позиции ВСЕГДА!
        {
            ConsoleKey c = ConsoleKey.C;
            if (player.hp <= 0)
                return;
            for (int i = 0; i < dialog.Length; i++)
            {
                Console.Clear();
                if (i % 2 == 0)
                {
                    Console.WriteLine(dialog[i]);
                    Console.WriteLine();
                    foreach (string en in enemys)
                        Console.Write(en + " ");
                    Console.WriteLine();
                    if (enemys.Length % 2 == 0)
                        for (int j = 0; j < (enemys.Length * 3 + enemys.Length) / 2; j++)
                            Console.Write(" ");
                    else
                        for (int j = 0; j < (enemys.Length * 3 + enemys.Length) / 2 - 2; j++)
                            Console.Write(" ");
                    Console.WriteLine(player.icon);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    foreach (string en in enemys)
                        Console.Write(en + " ");
                    Console.WriteLine();
                    if (enemys.Length % 2 == 0)
                        for (int j = 0; j < (enemys.Length * 3 + enemys.Length) / 2; j++)
                            Console.Write(" ");
                    else
                        for (int j = 0; j < (enemys.Length * 3 + enemys.Length) / 2 - 2; j++)
                            Console.Write(" ");
                    Console.WriteLine(player.icon);
                    Console.WriteLine(dialog[i]);
                }
                 c = Console.ReadKey(true).Key;
            }//dialog
            if (enemys.Length % 2 == 0)
                for (int j = 0; j < (enemys.Length * 3 + enemys.Length) / 2; j++)
                    player.icon = " " + player.icon;
            else
                for (int j = 0; j < (enemys.Length * 3 + enemys.Length) / 2 - 2; j++)
                    player.icon = " " + player.icon;
            ConsoleKey ckey = c;
            Random rand = new Random();
            int ind = -1;
            int points = 0;
            ind = rand.Next(enemys.Length);
            Boolean flag = false;
            Boolean DamageGet = false;
            Boolean IsBlocked = false;
            int enemyshp = 0;
            foreach(int[] EnStat in stats)
            {
                enemyshp += EnStat[0];
            }
            int spam = 0;
            while (player.hp > 0)
            {
                //Console.WriteLine(enemyshp);
                //Console.WriteLine(IsBlocked);
                if ((points + 2) % hardlevel == 0)//warning1!
                    Console.WriteLine("!");
                if ((points + 1) % hardlevel == 0)//warning2!
                    Console.WriteLine("!");
                if (Console.KeyAvailable) {
                    ckey = Console.ReadKey(true).Key;
                }
                if (points % hardlevel <= 3 && ckey == ConsoleKey.Spacebar)
                {
                    spam--;
                    IsBlocked = true;
                }
                if (points % hardlevel == 0)
                {
                    Console.WriteLine("!");
                    if (ckey == ConsoleKey.BrowserStop && !flag)
                    {
                        spam--;
                        player.hp -= (stats[ind][2] - player.armor);
                        DamageGet = true;
                    }
                    else if (IsBlocked)
                    {
                        player.hp -= (int)Math.Floor(0.7 * stats[ind][2]);
                        IsBlocked = false;
                        DamageGet = true;
                    }
                    ind = rand.Next(enemys.Length);
                }
                else
                {
                    if ((points+2) % hardlevel != 0 && (points+1) % hardlevel !=0)
                        Console.WriteLine("");
                }

                Console.WriteLine("HP =" + player.hp);
                //Console.WriteLine(player.icon.Length);//служебная информация
                if (player.icon.Substring(0, 1).Equals(" ") && ckey == ConsoleKey.A)
                {
                    spam--;
                    player.icon = player.icon.Remove(0, 4);
                    flag = true;
                }
                if ((player.icon.Length <= (enemys.Length * 3 + enemys.Length)) && ckey == ConsoleKey.D)
                {
                    spam--;
                    player.icon = "    " + player.icon;
                    flag = true;
                }
                if (!DamageGet && ckey == ConsoleKey.E && player.icon.Length / 3 <= stats.Length)
                {
                    spam++;
                    if (rand.Next(100) > 90 && spam > 6)
                    {

                        Console.WriteLine("Вы попались на парирование!!  -" + 3 * stats[(player.icon.Length / 3) - 1][2] + "HP");
                        player.hp -= 3 * stats[(player.icon.Length / 3) - 1][2];
                        Thread.Sleep(500);
                    }
                    else if ((player.icon.Length / 3) - 1 < enemys.Length)
                    {
                            stats[(player.icon.Length / 3) - 1][0] -= player.damage - stats[(player.icon.Length / 3) - 1][1];
                        if (stats[(player.icon.Length / 3) - 1][0] <= 0)
                        {
                            enemyshp -= player.damage - stats[(player.icon.Length / 3) - 1][1] + stats[(player.icon.Length / 3) - 1][0];
                            stats[(player.icon.Length / 3) - 1][0] = 0;
                        }
                        else
                            enemyshp -= player.damage - stats[(player.icon.Length / 3) - 1][1];
                    }
                }

                if (player.icon.Length / 3 <= stats.Length)
                    Console.WriteLine("Enemy HP = " + stats[(player.icon.Length / 3) - 1][0]);
                else
                    Console.WriteLine("No Enemy");

                if (player.icon.Length / 3 <= stats.Length && stats[(player.icon.Length / 3) - 1][0] == 0)
                    stats[(player.icon.Length / 3) - 1][2] = 0;//no dead damage

                Console.WriteLine();
                foreach (string en in enemys)
                    Console.Write(en + " ");
                Console.WriteLine();
                Console.WriteLine(player.icon);
                Thread.Sleep(60);
                if (!IsBlocked)
                    ckey = ConsoleKey.BrowserStop;
                if (points % 2 == 0)
                {
                    flag = false;
                }
                Console.Clear();
                //IsBlocked = false;
                DamageGet = false;
                points++;
                if (enemyshp <= 0)//Victory!
                {
                    //Save(player);
                    return;
                }
            }//fighting

        }
        static string[] RandomMapMaker(string[] LocationsNames){
          int length = LocationsNames.Length;
          string[] result = new string[length + length*6];
          Random rand = new Random();
          int spaces = 5;
          int otstup = 0;
          for(int i = 0; i < length; i++)
          {
            int rnd = rand.Next(3, 6);
            for(int j = 0; j <= rnd; j++)
            {
              if (j == rnd)
              {
                result[otstup + j] = StringsPower(" ", spaces) + "o " + LocationsNames[i];
                break;
              }
              string ways = "";
              int randway;
              if (spaces == 0)
                randway = rand.Next(1);
              else
                randway = rand.Next(2);
              if (randway == 0)
                ways = @"\";
              else if (randway == 1)
                ways = @"|";
              else if (randway == 2)
                ways = "/";
              result[otstup + j] = StringsPower(" ", spaces) + ways;
              if (ways.Equals("/"))
                spaces--;
              else if (ways.Equals(@"\"))
                spaces++;
              }
              otstup += (rnd + 1);
            }
            return result;
        }
        static void MapObserver(string[] map, int step)
        {
          int k = step;
          int locind = 0;
          ConsoleKey ckey = ConsoleKey.C;
          while (ckey != ConsoleKey.Enter)
          {
            for (int i = k - 5; i < k; i++)
            {
              Console.WriteLine(map[i]);
            }
            ckey = Console.ReadKey(true).Key;
            if (k < map.Length && locind <= map.Length && ckey == ConsoleKey.DownArrow)
              k += 1;
              else if (k > step && ckey == ConsoleKey.UpArrow)
              {
                locind = 0;
                k -= 1;
              }
              Console.Clear();
            }
          }
        static string StringsPower(string str, int length)
          {
              string result = "";
              for(int i = 0; i < length; i++)
              {
                  result += str;
              }
              return result;
          }
      }
  class Player{
    public int damage;
    public int hp;
    public int armor;
    public string icon;
    int money;
    public int karms;
    public void setMoney(int mon)
    {
        money = mon;
    }
    public int getMoney()
    {
        return money;
    }
    public Player(int hp, int armor, int damage, string icon)
    {
        this.damage = damage;
        this.hp = hp;
        this.armor = armor;
        this.icon = icon;
        money = 200;
    }
    //public int GetHp()
    //{
    //    return this.hp;
    //}
    //public int GetArmor()
    //{
    //    return this.armor;
    //}
    //public string GetIcon()
    //{
    //    return this.icon;
    //}
  }
}
