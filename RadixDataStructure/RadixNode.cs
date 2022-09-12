using System;
namespace BlockchainDataStructures.RadixDataStructure
{
    /// <summary>
    /// A node in a tree
    /// </summary>
    public class RadixNode
    {
        public string Key { get; set; }
        public int? Value { get; set; }
        public bool IsLast { get; set; }
        public readonly List<RadixNode> ChildrenNodes;

        public RadixNode()
        {
            Key = String.Empty;
            Value = null;
            ChildrenNodes = new List<RadixNode>();
        }

        public RadixNode(string key, int? value) 
        {
            Key = key;
            Value = value;
            ChildrenNodes = new List<RadixNode>();
        }
    }

    public static class RadixNodeExtension
    {
        /// <summary>
        /// Is the current node the root one?
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsNodeRoot(this RadixNode node)
        {
            if (node == null) return false;
            return node.Key == string.Empty &&
                node.Value.HasValue && node.Value == 0 &&
                node.ChildrenNodes.Count == 0;
        }

        /// <summary>
        /// Set isLast flag to true if the last node does not contain any child
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static RadixNode SetLast(this RadixNode node)
        {
            var subNodes = node.ChildrenNodes.Select(SetLast);
            foreach(var sub in subNodes.Where(x => x.ChildrenNodes.Count() == 0))
            {
                sub.IsLast = true;
            }

            return node;
        }
    }
}