using System.Linq;
using System.Text.Json.Nodes;
using BlockchainDataStructures;
using BlockchainDataStructures.RadixDataStructure.Utils;
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

var prettyPrintTree = new PrettyPrintTree();
RadixTree radixTree = new();
var result = radixTree.Add(dataSet);
prettyPrintTree.PrintToConsole(result, "", true);
var allWordsInserted = radixTree.GetAllInsertedWords(result);

bool hasIssue = (allWordsInserted.Count != dataSet.Count);
if(!hasIssue)
{
    foreach(var word in dataSet)
    {
        hasIssue &= !allWordsInserted.Contains(word.Key);
    }
}
if (hasIssue)
    throw new Exception("There was an issue with the insert algorithm !");


//now try with fake random words
var thisDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
var mockDataJsonPath = File.ReadAllText(Path.Combine(thisDir, "RadixDataStructure", "mockData.json"));
var jsonObject = JsonNode.Parse(mockDataJsonPath)?.AsArray();
if (jsonObject == null)
    return;

var _dataSet = new Dictionary<string, int>();
for (int i = 0; i < jsonObject.Count; i++)
{
    var key = jsonObject.ElementAt(i);
    if(!_dataSet.ContainsKey(key.ToString()))
        _dataSet.Add(key.ToString(), i);
    else Console.WriteLine(key);
}

RadixTree _radixTree = new();
var input = _dataSet.ToList();
var _result = _radixTree.Add(input);
var _allAddedWords = _radixTree.GetAllInsertedWords(_result);

prettyPrintTree = new PrettyPrintTree();
var prettyPrintStr = prettyPrintTree.PrintToString(_result, "", true);
File.WriteAllText(Path.Combine(thisDir, "RadixDataStructure", "treeWithSortedInput.json"), prettyPrintStr);

Console.WriteLine("Check if the number of records inserted in tree matches the inout dataset given to feed the tree...");
bool _hasIssue = (_allAddedWords.Count != input.Count);

if (!_hasIssue)
{
    foreach (var word in input)
    {
        hasIssue &= !_allAddedWords.Contains(word.Key);
    }
}
if (hasIssue)
{
    Console.WriteLine("Trying to figure out what words have not been added to tree...");
    foreach (var word in input)
    {
        if(!_allAddedWords.Contains(word.Key))
        {
            Console.WriteLine($"word: {word.Key} has not been inserted");
        }
    }
    throw new Exception("There was an issue with the insert algorithm !");
}

//since the list of random words I picked on internet is already sorted and may help for the insert algorithm to work efficiently
//try to "unsort" the dataset and create a tree and compare with the previous one
var shuffledList = input.Shuffle();
RadixTree _radixTreeWithShuffledList = new();
var _resultTreeWithShuffledList = _radixTreeWithShuffledList.Add(shuffledList);

prettyPrintTree = new PrettyPrintTree();
prettyPrintStr = prettyPrintTree.PrintToString(_resultTreeWithShuffledList, "", true);
File.WriteAllText(Path.Combine(thisDir, "RadixDataStructure", "treeWithUnSortedInput.json"), prettyPrintStr);

var _allAddedWordsWithShuffledList = _radixTreeWithShuffledList.GetAllInsertedWords(_resultTreeWithShuffledList);
bool _hasTreeWithShuffledListIssue = _allAddedWordsWithShuffledList.Count != shuffledList.Count;
if(_hasTreeWithShuffledListIssue)
    throw new Exception("There was an issue with the insert algorithm !");

Console.WriteLine("Lookup for value: discrimination in tree");
var found = _radixTree.Lookup("discrimination", _result);
Console.WriteLine(found);
Console.ReadLine();

//merkle tree code section

MerkleTree merkleTree = new(250);
string root = merkleTree.GetRoot();
Console.Out.WriteLine(root);

Console.ReadLine();