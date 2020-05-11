using System;
using System.Collections.Generic;
using System.Text;

namespace Escape_From_The_Woods
{
    class Tree
    {
        public int treeID;
        public int x;
        public int y;

        public Tree(int treeID, int x, int y)
        {
            this.treeID = treeID;
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Tree tree &&
                   treeID == tree.treeID &&
                   x == tree.x &&
                   y == tree.y;
        }
    }
}