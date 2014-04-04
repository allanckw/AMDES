using System.Collections.Generic;
using System.Linq;
using MvvmFoundation.Wpf;
using System;

namespace CircularDependencyTool
{
    /// <summary>
    /// Represents an item in a graph.
    /// </summary>
    public class Node : ObservableObject, IComparable<Node>
    {
        #region Constructor

        public Node(string id, double locationX, double locationY, bool evalTrue)
        {
            this.NodeDependencies = new List<Node>();
           
            this.LocationX = locationX;
            this.LocationY = locationY;
            this.ID = id;
            this.evalToTrue = !evalTrue;
        }

        #endregion // Constructor

        #region Properties

        public string ID { get; private set; }

        public double LocationX
        {
            get { return _locationX; }
            set
            {
                if (value == _locationX)
                    return;

                _locationX = value;

                base.RaisePropertyChanged("LocationX");
            }
        }

        public double LocationY
        {
            get { return _locationY; }
            set
            {
                if (value == _locationY)
                    return;

                _locationY = value;

                base.RaisePropertyChanged("LocationY");
            }
        }

        public List<Node> NodeDependencies { get; private set; }


        public bool HasCircularDependency
        {
            get { return this.evalToTrue; }
        }

        public double NodeWidth
        {
            get { return 100; }
        }

        public double NodeHeight
        {
            get { return 30; }
        }

        #endregion // Properties

        #region Methods

        

        private class NodeInfo
        {
            public NodeInfo(Node node)
            {
                this.Node = node;
                _index = 0;
            }

            public Node Node { get; private set; }

            public NodeInfo GetNextDependency()
            {
                if (_index < this.Node.NodeDependencies.Count)
                {
                    var nextNode = this.Node.NodeDependencies[_index++];
                    return new NodeInfo(nextNode);
                }

                return null;
            }

            int _index;
        }

        #endregion // Methods

        #region Fields

        double _locationX, _locationY;

        bool evalToTrue;

        #endregion // Fields

        public int CompareTo(Node n)
        {
            return this.ID.CompareTo(n.ID);
        }
    }
}