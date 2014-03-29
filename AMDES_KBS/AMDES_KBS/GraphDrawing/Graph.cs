using System.Collections.Generic;
using System.Linq;

namespace CircularDependencyTool
{
    /// <summary>
    /// Represents a set of nodes that can be dependent upon each other, 
    /// and will detect circular dependencies between its nodes.
    /// </summary>
    public class Graph
    {
        #region Constructor

        private int x = 5, y = 5;
        bool oddRow = true;

        public int X
        {
            get
            {
                if (Nodes.Count % 5 == 0 && Nodes.Count > 0)
                {
                    oddRow = !oddRow;
                    y += 50;
                    if (oddRow)
                    {
                        x += 130;
                        return x;
                    }
                    else
                    {
                        x -= 130;
                        return x;
                    }
                }
                else
                {
                    if (oddRow)
                    {
                        if (Nodes.Count % 5 == 1 && Nodes.Count > 1)
                        {
                            x += 260;
                            return x - 130;
                        }
                        else
                        {
                            x += 130;
                            return x - 130;
                        }
                    }
                    else
                    {
                        if (Nodes.Count % 5 == 1 && Nodes.Count > 1)
                        {
                            x -= 260;
                            return x + 130;
                        }
                        else
                        {
                            x -= 130;
                            return x + 130;
                        }
                    }
                }
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set { y = value; }
        }

        public Graph(string title)
        {
           
            this.Nodes = new List<Node>();
            this.Title = title;
        }

        #endregion // Constructor

        #region Properties

                public List<Node> Nodes { get; private set; }

        public void addGraphNodes(Node n)
        {

        }

        public string Title { get; private set; }

        #endregion // Properties
    }
}