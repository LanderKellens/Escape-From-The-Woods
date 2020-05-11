using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Escape_From_The_Woods
{
    class Program
    {
        public static Map bos;
        public static string log = @"C:\Users\mkell\Desktop\Thread_async\Log.txt";
        public static List<string> allesLog = new List<string>();
        static void Main(string[] args)
        {
            string path = @"C:\Users\mkell\Desktop\Thread_async\Foto";

            Console.Write("Wat is X: ");
            int x = Int32.Parse(Console.ReadLine());

            Console.Write("Wat is Y: ");
            int y = Int32.Parse(Console.ReadLine());

            Console.Write("Wat is het aantal bomen: ");
            int aantalBomen = Int32.Parse(Console.ReadLine());

            Console.Write("\nMax 10 apen!\nWat is het aantal apen: ");
            int aantalApen = Int32.Parse(Console.ReadLine());

            Random random = new Random();

            List<Monkey> apen = new List<Monkey>();
            List<Tree> bomen = new List<Tree>();

            List<string> namen = new List<string>
            {
                "William", "marc", "Lukas", "Levi", "Sven", "Laurens", "Robrecht", "Carmen", "Silke", "Lander"
            };

            List<Color> kleuren = new List<Color>
            {
                Color.Blue, Color.Yellow, Color.Chocolate, Color.White, Color.Brown, Color.DarkGray, Color.DeepPink, Color.Gold, Color.LightSeaGreen, Color.Red
            };

            for (int i = 1; i <= aantalBomen; i++)
            {
                bomen.Add(new Tree(i, random.Next(x - 10), random.Next(y - 10)));
            };

            for (int i = 1; i <= aantalApen; i++)
            {
                apen.Add(new Monkey(i, namen[i - 1], bomen[random.Next(bomen.Count -1)], kleuren[i - 1]));
            }
            bos = new Map(x, y, bomen, apen);


            

            Bitmap bm = new Bitmap(x, y);

            Pen p = new Pen(Color.Green, 1);

            Graphics g = Graphics.FromImage(bm);

            foreach (Tree boom in bomen)
            {
                g.DrawEllipse(p, boom.x, boom.y, 10, 10);
            }

            foreach (Monkey aap in apen)
            {
                Tree kortsteBoom;
                kortsteBoom = aap.ZoekKorsteBoomVanafEnIgnore(aap.startTree);
                Tree startPunt = aap.startTree;

                while (aap.KorsteBoomGetal(aap.startTree, kortsteBoom) < aap.AfstandRandBerekenen(aap.startTree.x, aap.startTree.y))
                {
                    Pen pen = new Pen(aap.kleur, 1);
                    g.DrawLine(pen, aap.startTree.x + 5, aap.startTree.y + 5, kortsteBoom.x + 5, kortsteBoom.y + 5);
                    allesLog.Add(aap.naam + " is in boom " + aap.startTree.treeID + " op (" + aap.startTree.x + "," + aap.startTree.y + ")");
                    //foreach (string item in allesLog)
                    //{
                    //    Console.WriteLine(item);
                    //}
                    //Console.WriteLine(aap.naam + " is in boom " + aap.startTree.treeID);
                    aap.startTree = kortsteBoom;
                    kortsteBoom = aap.ZoekKorsteBoomVanafEnIgnore(aap.startTree);

                    
                }

                using (StreamWriter InhoudFile = new StreamWriter(log))
                {
                    foreach (string item in allesLog)
                    {
                        InhoudFile.WriteLine(item);
                    }
                }

                SolidBrush b = new SolidBrush(aap.kleur);
                g.FillEllipse(b, startPunt.x, startPunt.y, 10, 10);
            }

            bm.Save(Path.Combine(path, "escapeRoutes.jpg"), ImageFormat.Jpeg);


            //foreach (Monkey aap in apen)
            //{
            //    Tree kortsteBoom;
            //    kortsteBoom = aap.ZoekKorsteBoomVanafEnIgnore(aap.startTree);

            //    using (StreamWriter InhoudFile = new StreamWriter(log))
            //    {
            //        while (aap.KorsteBoomGetal(aap.startTree, kortsteBoom) < aap.AfstandRandBerekenen(aap.startTree.x, aap.startTree.y))
            //        {


            //            InhoudFile.WriteLine(aap.naam + " is in boom " + aap.startTree.treeID);



            //            aap.startTree = kortsteBoom;
            //            kortsteBoom = aap.ZoekKorsteBoomVanafEnIgnore(aap.startTree);
            //        }


            //    }

            //}

        }


    }
}