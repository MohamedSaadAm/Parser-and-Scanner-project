using DotNetGraph;
using DotNetGraph.Edge;
using DotNetGraph.Extensions;
using DotNetGraph.Node;
using GraphVizNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    internal class SyntaxTree
    {
        static UInt64 oval_id = 0;
        static UInt64 square_id = 0;
        static DotGraph graph = new DotGraph("SyntaxTree");

        public static DotNode MakeOvalNode(string data)
        {
            var myNode = new DotNode("o" + oval_id)
            {
                Shape = DotNodeShape.Oval,
                Label = data,
                FillColor = Color.White,
                FontColor = Color.Black,
                Style = DotNodeStyle.Solid,
                Width = 1f,
                Height = 1f,
                PenWidth = 1.5f
            };
            oval_id++;
            graph.Elements.Add(myNode);
            return myNode;
        }
        public static DotNode MakeSquareNode(string data)
        {
            var myNode = new DotNode("s" + square_id)
            {
                Shape = DotNodeShape.Square,
                Label = data,
                FillColor = Color.White,
                FontColor = Color.Black,
                Style = DotNodeStyle.Solid,
                Width = 1f,
                Height = 1f,
                PenWidth = 1.5f
            };
            graph.Elements.Add(myNode);
            square_id++;
            return myNode;
        }
        public static void ConnectChildNodes(DotNode node1, DotNode node2)
        {
            string first_node = node1.Identifier;
            string second_node = node2.Identifier;
            var myEdge = new DotEdge(first_node, second_node);
            graph.Elements.Add(myEdge);
            myEdge.SetCustomAttribute("minlen", "3");
        }
        public static void ConnectNodesAtTheSameLevel(DotNode node1, DotNode node2)
        {
            string first_node = node1.Identifier;
            string second_node = node2.Identifier;
            var myEdge = new DotEdge(first_node, second_node);
            myEdge.SetCustomAttribute("minlen", "6];\n    {rank=same;" + first_node + ";" + second_node + ";};\n#");
            graph.Elements.Add(myEdge);
        }
        public static void GenerateSyntaxTree()
        {
            var dot = graph.Compile(true);
            File.WriteAllText("dotFile.dot", dot);
            Process process = new System.Diagnostics.Process();
            ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "dot.exe";
            startInfo.Arguments = "-Tpng  dotFile.dot  -o  SyntaxTree.png";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            Process p = new Process();
            process.StartInfo.FileName = "SyntaxTree.png";
            process.Start();
            graph.Elements.Clear();
            oval_id = 0;
            square_id = 0;
        }
    }
}
