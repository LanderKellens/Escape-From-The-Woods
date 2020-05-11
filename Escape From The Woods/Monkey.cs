using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace Escape_From_The_Woods
{
    class Monkey
    {
        
        public int monkeyID;
        public string naam;
        public Tree startTree;
        public List<Tree> pad = new List<Tree>();
        public Color kleur;


        public Monkey(int monkeyID, string naam, Tree startTree, Color kleur)
        {
            this.monkeyID = monkeyID;
            this.naam = naam;
            this.startTree = startTree;
            this.kleur = kleur;
            this.pad.Add(startTree);
        }
            
        public Tree ZoekKorsteBoomVanafEnIgnore(Tree meegegevenBoom)
        {
            Dictionary<Tree, double> kort = new Dictionary<Tree, double>();
            foreach (Tree tree in Program.bos.bomen)
            {
                
                if (!pad.Contains(tree))
                {
                    double d = Math.Sqrt(Math.Pow(meegegevenBoom.x - tree.x, 2) + Math.Pow(meegegevenBoom.y - tree.y, 2));
                    kort.Add(tree, d);
                }
            }
            Console.WriteLine(naam);
            Tree eersteBoom = kort.OrderBy(i => i.Value).First().Key;

            pad.Add(eersteBoom);
            return eersteBoom;
        }

        public double KorsteBoomGetal(Tree boom, Tree KorsteBoom)
        {
            double d = Math.Sqrt(Math.Pow(boom.x - KorsteBoom.x, 2) + Math.Pow(boom.y - KorsteBoom.y, 2));
            return d;
        }

        public double AfstandRandBerekenen(int x, int y)
        {
            double distanceToBorder = (new List<double>() { Program.bos.y - y, Program.bos.x - x, y - 0, x - 0 }).Min();
            return distanceToBorder;
        }
    }
}