using BlockchainDataStructures.RadixDataStructure;

namespace BlockChainDataStructures.RadixdataStructure
{
    /// https://medium.com/coinmonks/data-structure-in-ethereum-episode-2-radix-trie-and-merkle-trie-d941d0bfd69a <summary>
    /// 
    /// This is the implementation of a "non optimized" radix tree as a path is represented by a single character
    /// it takes more memory to insert a bunch of keys/values as you need to descend deep down the tree
    /// </summary>
    public class SimpleRadixTree : IRadixTree<int>
    {
        private RadixNode _rootNode;
        public SimpleRadixTree()
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
            _rootNode.SetLast();
            return _rootNode;
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

            int oneCharLength = 1;
            int nbOfCharsProcessed = 0;
            while (!string.IsNullOrEmpty(key))
            {
                var firstCharFromKeyPart = key[0].ToString();

                var n = node.ChildrenNodes.FirstOrDefault(x => x.Key == firstCharFromKeyPart);
                if (n == null)
                {
                    var nodeToInsert = new RadixNode(firstCharFromKeyPart, null);
                    node.ChildrenNodes.Add(nodeToInsert);
                    node = nodeToInsert;
                }
                else
                {
                    node = node.ChildrenNodes.First(x => x.Key == firstCharFromKeyPart);
                }

                key = key.Substring(oneCharLength, key.Length - oneCharLength);
                nbOfCharsProcessed += 1;

                if (nbOfCharsProcessed == keyValue.Key.Length)
                    node.Value = value;
            }
            return;
        }

        /// <summary>
        /// Search for a key in tree
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        /// <returns>true if found, otherwise false</returns>
        public int Lookup(string key, RadixNode node)
        {
            if (string.IsNullOrEmpty(key))
                return -1;

            return LookupInternal(key, node);
        }

        private int LookupInternal(string lookupKey, RadixNode node)
        {
            string key = lookupKey;
            int oneCharLength = 1; 
            int nbOfCharsProcessed = 0;
            while (!string.IsNullOrEmpty(key))
            {
                var firstCharFromKeyPart = key[0].ToString();

                var n = node.ChildrenNodes.FirstOrDefault(x => x.Key == firstCharFromKeyPart);
                if (n != null)
                {
                    node = n;
                    key = key.Substring(oneCharLength, key.Length - oneCharLength);
                    nbOfCharsProcessed += 1;
                }
                else break;
            }
            return (nbOfCharsProcessed == lookupKey.Length && node.Value.HasValue) ? node.Value.Value : -1;
        }

        public List<string> GetAllInsertedWords(RadixNode rootNode)
        {
            List<string> words = new ();
            if (rootNode == null) return words;
            GetAllInsertedWordsInternal(rootNode, string.Empty, words);
            return words;
        }
        private void GetAllInsertedWordsInternal(RadixNode node, string currentWord, List<string> words)
        {
            currentWord += node.Key;
            if (node.Value.HasValue)
                words.Add(currentWord);

            for (int i = 0; i < node.ChildrenNodes.Count; i++)
            {
                GetAllInsertedWordsInternal(node.ChildrenNodes[i], currentWord, words);
            }
        }
    }
}


