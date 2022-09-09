using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using BlockchainDataStructures.RadixDataStructure;

namespace BlockChainDataStructures.RadixdataStructure
{
    /// <summary>
    /// https://medium.com/coinmonks/data-structure-in-ethereum-episode-2-radix-trie-and-merkle-trie-d941d0bfd69a
    /// Wikipedia's section insertion is very intuitive and it was easier to me to build the different steps for the algo, check section Insertion from https://en.wikipedia.org/wiki/Radix_tree
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
        public RadixNode Add(IEnumerable<KeyValuePair<string, int>> keyValues)
        {
            foreach (var keyValue in keyValues)
            {
                //we always want to insert a new key/value pair by traversing the tree from the rootNode
                Insert(keyValue, _rootNode);
            }
            return _rootNode;
        }

        /// <summary>
        /// Insert a pair of key/value into the tree
        /// </summary>
        /// <param name="keyValue"><pair of key/value/param>
        /// <param name="node">the root node</param>
        private void Insert(KeyValuePair<string, int> keyValue, RadixNode node)
        {
            //is the current node, the root? then create a child with the complete word and its value
            if (node.IsNodeRoot())
            {
                node.ChildrenNodes.Add(new RadixNode(keyValue.Key, keyValue.Value));
                return;
            }
            else
            {
                string key = keyValue.Key;
                int value = keyValue.Value;
                var matches = GetNearestBestConsecutivesMatchesChars(key, node);

                if ((matches >= 0) && (matches < key.Length) && (matches >= node.Key.Length))
                {
                    bool inserted = false;
                    var keyPart = key.Substring(matches, key.Length - matches);
                    foreach (var childNode in node.ChildrenNodes)
                    {
                        if (childNode.Key.StartsWith(keyPart[0]))
                        {
                            inserted = true;
                            Insert(new KeyValuePair<string, int>(keyPart, value), childNode);
                        }
                    }
                    if (!inserted)
                    {
                        node.ChildrenNodes.Add(new RadixNode(keyPart, value));
                    }
                }
                else
                {
                    if (matches < key.Length)
                    {
                        //(https://en.wikipedia.org/wiki/Radix_tree) => Insertion 4th step
                        //Insert 'team' while splitting 'test' and creating a new edge label 'st'
                        //Insert 'toast' while splitting 'te' and moving previous strings a level lower

                        string root = key.Substring(0, matches);
                        string previousKey = node.Key.Substring(matches, node.Key.Length - matches);
                        string newKey = key.Substring(matches, key.Length - matches);

                        node.Key = root;

                        var newNodeWithPreviousKey = new RadixNode(previousKey,value);
                        newNodeWithPreviousKey.ChildrenNodes.AddRange(node.ChildrenNodes);

                        node.ChildrenNodes.Clear();
                        node.ChildrenNodes.Add(newNodeWithPreviousKey);

                        var newNodeWithNewKey = new RadixNode(newKey, value);
                        node.ChildrenNodes.Add(newNodeWithNewKey);

                        return;
                    }
                    if (matches > node.Key.Length)
                    {
                        string mewKey = node.Key.Substring(node.Key.Length, key.Length);
                        node.ChildrenNodes.Add(new RadixNode(mewKey,value));
                    }
                }
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
        int GetNearestBestConsecutivesMatchesChars(string keyLookup, RadixNode currentNode)
        {
            int matches = 0;
            var curentNodeKeyLength = currentNode.Key.Length;
            var keyLookupLength = keyLookup.Length;

            var smallestStrLength = keyLookupLength < curentNodeKeyLength ? keyLookupLength : curentNodeKeyLength;
            if (smallestStrLength == 0) return matches;

            for (int i = 0; i < smallestStrLength; i++)
            {
                if (keyLookup[i] == currentNode.Key[i])
                    matches++;
                else break;
            }
            return matches;
        }
    }
}


