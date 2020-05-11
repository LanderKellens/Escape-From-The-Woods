using System;
using System.Collections.Generic;
using System.Text;

namespace Escape_From_The_Woods
{
    class Map
    {
        public int x;
        public int y;

        public List<Tree> bomen = new List<Tree>();
        public List<Monkey> apen = new List<Monkey>();

        public Map(int x, int y, List<Tree> bomen, List<Monkey> apen)
        {
            this.x = x;
            this.y = y;
            this.bomen = bomen;
            this.apen = apen;
        }
    }
}
