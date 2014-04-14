using System;
using System.Collections.Generic;

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
                    y += 45;
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

        public void addGraphNodes(string id, bool evalTrue)
        {
            var node = new Node(id, this.X, this.Y, evalTrue);
            if (Nodes.Contains(node))
            {
                throw new InvalidOperationException("Please do not add the same set of question twice!");
            }
            else
            {
                if (Nodes.Count == 0)
                {
                    Nodes.Add(node);
                }
                else if (Nodes.Count > 0)
                {
                    Node dependency = Nodes[Nodes.Count - 1];
                    node.NodeDependencies.Add(dependency);
                    Nodes.Add(node);
                }
            }
        }

        public void resetGraph()
        {
            Nodes.Clear();
            x = 5; 
            y = 5;

        }

        public string Title { get; private set; }

        #endregion // Properties
    }
}