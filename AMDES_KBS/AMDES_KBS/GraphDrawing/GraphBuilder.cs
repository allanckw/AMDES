using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CircularDependencyTool
{
    public class GraphBuilder
    {
        // NOTE: This method is invoked from AppWindow.xaml
        public static IEnumerable<Graph> BuildGraphs()
        {
            //can create multiple graph, but in our case we only need 1 graph
            return new List<Graph>
            {
                BuildGraph("ComplexCircular.xml")                
            };
        }

        static Graph BuildGraph(string xmlFileName)
        {
            string path = string.Format(@"Graphs\{0}", xmlFileName);
            XDocument xdoc = XDocument.Load(path);

            // Create a graph.
            var graphElem = xdoc.Element("graph");
            string title = graphElem.Attribute("title").Value;
            var graph = new Graph(title);

            var nodeElems = graphElem.Elements("node").ToList();

            // Create all of the nodes and add them to the graph.
            foreach (XElement nodeElem in nodeElems)
            {
                string id = nodeElem.Attribute("id").Value + " ABCDE FGHIJK LMNO PQRS TUVW XYZ";
                graph.addGraphNodes(id);
            }

            return graph;
        }
    }
}