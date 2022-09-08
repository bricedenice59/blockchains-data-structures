using System.Reflection.Emit;
using System.Xml.Linq;
using BlockchainDataStructures.RadixDataStructure;

namespace BlockChainDataStructures.RadixdataStructure
{
    /// <summary>
    /// https://medium.com/coinmonks/data-structure-in-ethereum-episode-2-radix-trie-and-merkle-trie-d941d0bfd69a
    /// </summary>
    public class RadixTree
    {
        private RadixNode _rootNode;

        public RadixTree()
        {
            _rootNode = new RadixNode();
        }

        /// <summary>
        /// Add a list of keys and values to the tree
        /// </summary>
        /// <param name="keyValues"></param>
        public void Add(IEnumerable<KeyValuePair<string, int>> keyValues)
        {
            foreach (var keyValue in keyValues)
            {
                //we always want to insert a new key/value pair by traversing the tree from the rootNode
                Insert(keyValue, _rootNode);
            }
        }

        /// <summary>
        /// Insert a pair of key/value into the tree
        /// </summary>
        /// <param name="keyValue"><pair of key/value/param>
        /// <param name="node">the root node</param>
        private void Insert(KeyValuePair<string, int> keyValue, RadixNode node)
        {
            string key = keyValue.Key;
            int value = keyValue.Value;

            if (string.IsNullOrEmpty(key)) return;

            //is the current node, the root? then create a child with the complete word and its value
            if (node.IsNodeRoot())
            {
                node.ChildrenNodes.Add(new RadixNode(key, value));
                return;
            }
            else
            {
                var result = GetNearestBestConsecutivesMatchesChars(key, string.Empty,  0, node);
                var charMatching = result.Item1;
                var matchingNode = result.Item2;
                var newWordPart = key.Substring(charMatching, key.Length - charMatching);

                matchingNode.ChildrenNodes.Add(new RadixNode(newWordPart, value));
            }

        }

        /// <summary>
        /// Get the number of consecutive characters that are matching a given key
        /// </summary>
        /// <param name="keyLookup">The value we want to insert into the tree</param>
        /// <param name="previousKey">The previous key, used for recursion only, starts with parameter value = empty</param>
        /// <param name="matches">number of consecutive chars, used for recursion only, starts with parameter value = 0</param>
        /// <param name="currentNode">the current node traversed recursively</param>
        /// <returns>Returns a tuple, 1. consecutive characters that are matching, 2. the node that matches condition 1</returns>
        (int, RadixNode) GetNearestBestConsecutivesMatchesChars(string keyLookup, string previousKey, int matches, RadixNode currentNode)
        {
            int previousMatches = matches;
            RadixNode node = currentNode;
            matches = 0;

            var newWordPart = keyLookup.Substring(previousMatches, keyLookup.Length - previousMatches);
            if (newWordPart.Length == 0)
                return (previousMatches, node);

            foreach (var childNode in currentNode.ChildrenNodes.Where(x => x.Key.StartsWith(newWordPart[0])))
            {
                string nodeKey = previousKey + childNode.Key;

                if (keyLookup.Equals(nodeKey)) return (keyLookup.Length, childNode);

                var exist = newWordPart.IndexOf(childNode.Key, 0);
                if (exist != 0)
                    continue;

                node = childNode;

                for (int i = 0; i < nodeKey.Length; i++)
                {
                    if (i == keyLookup.Length) break;
                    if (keyLookup[i] == nodeKey[i])
                        matches++;
                    else break;
                }

                if (previousMatches > matches)
                    break;

                return GetNearestBestConsecutivesMatchesChars(keyLookup, nodeKey, matches, node);
            }
            return (previousMatches, node);
        }
    }
}


