namespace BlockchainDataStructures.RadixDataStructure
{
    internal interface IRadixTree<T> 
    {
        RadixNode Add(IEnumerable<KeyValuePair<string, T>> keyValues);

        T Lookup(string key, RadixNode node);

        List<string> GetAllInsertedWords(RadixNode node);
    }
}
