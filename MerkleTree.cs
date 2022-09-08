

using System.Text;

namespace BlockChainDataStructures
{
    public class MerkleTree
    {
        private readonly List<string> txHashes = new ();
        public MerkleTree(int numberOfTxHashes)
        {
            for (int i = 0; i < numberOfTxHashes; i++)
            {
                var randomHash = RandomHash.GetFromRandomString();
                txHashes.Add(randomHash);
            }
        }

        public string GetRoot()
        {
            return Compute(txHashes);
        }

        private string Compute(List<string> leavesList)
        {
            if (leavesList == null) return String.Empty;
            if (!leavesList.Any()) return String.Empty;

            //that is our root
            if (leavesList.Count == 1) return leavesList[0];

            var branch = new List<string>();

            //if number of leaves is odd, then add at last position of the list a duplicate of the last-1 item
            if (leavesList.Count % 2 == 1)
                leavesList.Add(leavesList.Last());

            for (int i = 0; i < leavesList.Count; i += 2)
            {
                var leftLeaf = Sha256Hash.Get(Encoding.UTF8.GetBytes(leavesList[i]));
                var righLeaf = Sha256Hash.Get(Encoding.UTF8.GetBytes(leavesList[i + 1]));
                branch.Add(leftLeaf + righLeaf);
            }
            return Compute(branch);
        }
    }
}


