using System.Text;

namespace BlockchainDataStructures
{
    //https://stackoverflow.com/questions/1649027/how-do-i-print-out-a-tree-structure
    public class PrettyPrintTree
    {
        private StringBuilder _sb;
        public PrettyPrintTree()
        {
            _sb = new StringBuilder(); 
        }
        public string PrintToString(RadixDataStructure.RadixNode tree, string indent, bool last)
        {
            _sb.Append(indent);
            //Console.Write(indent);
            if (last)
            {
                _sb.Append("\\-");
                //Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                _sb.Append("|-");
                //Console.Write("|-");
                indent += "| ";
            }
            var str = tree.Value.HasValue ? $"{tree.Key} ({tree.Value.Value})" : tree.Key;
            _sb.AppendLine(str);
            //Console.WriteLine(str);

            for (int i = 0; i < tree.ChildrenNodes.Count; i++)
                PrintToString(tree.ChildrenNodes[i], indent, tree.IsLast);

            return _sb.ToString();
        }

        public void PrintToConsole(RadixDataStructure.RadixNode tree, string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                //Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }
            var str = tree.Value.HasValue ? $"{tree.Key} ({tree.Value.Value})" : tree.Key;
            Console.WriteLine(str);

            for (int i = 0; i < tree.ChildrenNodes.Count; i++)
                PrintToConsole(tree.ChildrenNodes[i], indent, tree.IsLast);
        }
    }
}

