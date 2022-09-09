using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using BlockchainDataStructures;
using BlockchainDataStructures.RadixDataStructure;
using BlockChainDataStructures;
using BlockChainDataStructures.RadixdataStructure;

//dataset exported from wikipedia for learning purpose
List<KeyValuePair<string, int>> dataSet = new Dictionary<string, int>
    {
      {"test",0},
      {"slow",1},
      {"water",2},
      {"slower",3},
      {"tester",4},
      {"team",5},
      {"toast",6},
    }.ToList();

RadixTree radixTree = new();
var result = radixTree.Add(dataSet);
PrettyPrintTree.Print(result, "", true);


//now try with fake random words
var thisDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
var mockDataJsonPath = File.ReadAllText(Path.Combine(thisDir, "RadixDataStructure", "mockData.json"));
var jsonObject = JsonNode.Parse(mockDataJsonPath)?.AsArray();
if (jsonObject == null)
    return;

var _dataSet = new List<KeyValuePair<string, int>>();
for (int i = 0; i < jsonObject.Count; i++)
{
    var key = jsonObject.ElementAt(i);
    _dataSet.Add(new KeyValuePair<string, int>(key.ToString(), i));
}
RadixTree _radixTree = new();
var _result = _radixTree.Add(_dataSet);
PrettyPrintTree.Print(_result, "", true);

Console.ReadLine();

//merkle tree code section

MerkleTree merkleTree = new(250);
string root = merkleTree.GetRoot();
Console.Out.WriteLine(root);

Console.ReadLine();