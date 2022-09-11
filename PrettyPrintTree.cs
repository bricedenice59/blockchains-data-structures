using System;
using System.Xml.Linq;

namespace BlockchainDataStructures
{
    //https://stackoverflow.com/questions/1649027/how-do-i-print-out-a-tree-structure
    public class PrettyPrintTree
    {
        public static void Print(RadixDataStructure.RadixNode tree, string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }
            Console.WriteLine(tree.Key);

            for (int i = 0; i < tree.ChildrenNodes.Count; i++)
                Print(tree.ChildrenNodes[i], indent, tree.IsLast);
        }
    }
}

