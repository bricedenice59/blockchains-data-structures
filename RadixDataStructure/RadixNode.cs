using System;
namespace BlockchainDataStructures.RadixDataStructure
{
    /// <summary>
    /// A node in a tree
    /// </summary>
    public class RadixNode
    {
        public string Key { get; set; }
        public int Value { get; set; }
        public readonly List<RadixNode> ChildrenNodes;

        public RadixNode()
        {
            Key = String.Empty;
            Value = 0;
            ChildrenNodes = new List<RadixNode>();
        }

        public RadixNode(string key, int value) 
        {
            Key = key;
            Value = value;
            ChildrenNodes = new List<RadixNode>();
        }
    }

    public static class RadixNodeExtension
    {
        public static bool IsNodeRoot(this RadixNode node)
        {
            if (node == null) return false;
            return node.Key == string.Empty &&
                node.Value == 0 &&
                node.ChildrenNodes.Count == 0;
        }
    }
}