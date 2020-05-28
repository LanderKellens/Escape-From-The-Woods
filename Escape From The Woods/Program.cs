using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

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

            BosID();
            Task Bos = Task.Factory.StartNew(() => WoodData());
            Task Aap = Task.Factory.StartNew(() => MonkeyData());

            Task.WaitAll(Aap, Bos);
        }
        public static void WoodData()
        {
            SqlConnection connect = new SqlConnection(@"Data Source=LAPTOP-2RD68A3P\SQLEXPRESS01;Initial Catalog=EscapeFromTheWoods;Integrated Security=True");
            connect.Open();
            SqlCommand Command = new SqlCommand("SELECT * FROM [DBO].[WOODRECORDS]", connect);
            SqlDataAdapter sqlAdapt = new SqlDataAdapter(Command);
            DataTable set = new DataTable();
            sqlAdapt.Fill(set);

            SqlCommand command = new SqlCommand("INSERT INTO [DBO].[WOODRECORDS] (recordID,WoodID,TreeID,x,y) VALUES (@record,@Wood,@Tree,@X,@Y)", connect);
            command.Parameters.AddWithValue("@record", set.Rows.Count + 1);
            command.Parameters.AddWithValue("@Wood", bos.ID);
            String TreeIDS = "";
            foreach (Tree item in bos.bomen)
            {
                TreeIDS += item.treeID.ToString() + ", ";
            }
            command.Parameters.AddWithValue("@Tree", TreeIDS);
            command.Parameters.AddWithValue("@X", bos.x);
            command.Parameters.AddWithValue("@Y", bos.y);
            command.ExecuteNonQuery();
            connect.Close();

        }
        public static void MonkeyData()
        {
            SqlConnection connect = new SqlConnection(@"Data Source=LAPTOP-2RD68A3P\SQLEXPRESS01;Initial Catalog=EscapeFromTheWoods;Integrated Security=True");
            connect.Open();
            SqlCommand Command = new SqlCommand("SELECT * FROM [dbo].[MONKEYRECORD]", connect);
            SqlDataAdapter SqlAdapt = new SqlDataAdapter(Command);
            DataTable set = new DataTable();
            SqlAdapt.Fill(set);
            int recordCount = 1;
            foreach (Monkey monkey in bos.apen)
            {
                SqlCommand command = new SqlCommand("INSERT INTO [dbo].[MONKEYRECORD] (recordID,monkeyID,monkeyNaam,woodID,seqnr,treeID,x,y) VALUES (@record,@monkey,@name,@wood,@seq,@tree,@X,@Y)", connect);
                command.Parameters.AddWithValue("@record", recordCount);
                command.Parameters.AddWithValue("@monkey", monkey.monkeyID);
                command.Parameters.AddWithValue("@name", monkey.naam);
                command.Parameters.AddWithValue("@Wood", bos.ID);
                String path = "";
                foreach (Tree tree in monkey.pad)
                {
                    path += tree.treeID + ", ";
                }

                command.Parameters.AddWithValue("@seq", path);
                command.Parameters.AddWithValue("@Tree", monkey.startTree.treeID);
                command.Parameters.AddWithValue("@X", monkey.startTree.x);
                command.Parameters.AddWithValue("@Y", monkey.startTree.y);
                command.ExecuteNonQuery();
                recordCount = recordCount + 1;
            }
            connect.Close();
        }
        public static void BosID()
        {
            SqlConnection connect = new SqlConnection(@"Data Source=LAPTOP-2RD68A3P\SQLEXPRESS01;Initial Catalog=EscapeFromTheWoods;Integrated Security=True");
            connect.Open();
            SqlCommand Command = new SqlCommand("SELECT * FROM [DBO].[WOODRECORDS]", connect);
            SqlDataAdapter sqlAdapt = new SqlDataAdapter(Command);
            DataTable set = new DataTable();
            sqlAdapt.Fill(set);
            bos.ID = set.Rows.Count + 1;
        }
    }
}