using System.Text.Json.Nodes;
using BlockChainDataStructures;
using BlockchainDataStructures.RadixDataStructure.Utils;
using BlockchainDataStructures.Utils;
using BlockChainDataStructures.RadixdataStructure;
using System.Diagnostics;

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

Console.WriteLine("----------------SimpleRadixTree-----------------------");

SimpleRadixTree simpleRadixTree = new();
var rootNodeSimpleRadixTree = simpleRadixTree.Add(dataSet);
prettyPrintTree.PrintToConsole(rootNodeSimpleRadixTree, "", true);
var allWordsInsertedIntoSimpleRadixTree = simpleRadixTree.GetAllInsertedWords(rootNodeSimpleRadixTree);
Console.WriteLine($"GetAllInsertedWords function returned following words in tree:  {string.Join(", ", allWordsInsertedIntoSimpleRadixTree)}");

var valueToLookupSimpleRadixTree = dataSet[5];
var valueRetrievedSimpleRadixTree = simpleRadixTree.Lookup(valueToLookupSimpleRadixTree.Key, rootNodeSimpleRadixTree);
if (valueRetrievedSimpleRadixTree == -1)
    Console.WriteLine($"The value for the following string: {valueToLookupSimpleRadixTree.Key} was not found in tree!");
else
    Console.WriteLine($"The value found by the lookup function for string: {valueToLookupSimpleRadixTree.Key} is {valueRetrievedSimpleRadixTree}");

Console.WriteLine("------------------------------------------------------");

Console.WriteLine("----------------CompressedRadixTree-----------------------");

CompressedRadixTree compressedRadixTree = new();
var rootNodeCompressedRadixTree = compressedRadixTree.Add(dataSet);
prettyPrintTree.PrintToConsole(rootNodeCompressedRadixTree, "", true);
var allWordsInsertedIntoCompressedRadixTree = compressedRadixTree.GetAllInsertedWords(rootNodeCompressedRadixTree);
Console.WriteLine($"GetAllInsertedWords function returned following words in tree:  {string.Join(", ", allWordsInsertedIntoCompressedRadixTree)}");

var valueToLookup = dataSet[5];
var valueRetrieved = compressedRadixTree.Lookup(valueToLookup.Key, rootNodeCompressedRadixTree);
if (valueRetrieved == -1)
    Console.WriteLine($"The value for the following string: {valueToLookup.Key} was not found in tree!");
else
    Console.WriteLine($"The value found by the lookup function for string: {valueToLookup.Key} is {valueRetrieved}");

Console.WriteLine("------------------------------------------------------");

bool hasIssue = (allWordsInsertedIntoCompressedRadixTree.Count != dataSet.Count);
if (!hasIssue)
{
    foreach (var word in dataSet)
    {
        hasIssue &= !allWordsInsertedIntoCompressedRadixTree.Contains(word.Key);
    }
}
if (hasIssue)
    throw new Exception("There was an issue with the insert algorithm !");


//now try with fake random words picked at
//https://github.com/dwyl/english-words/blob/master/words_dictionary.json
//https://github.com/words/an-array-of-french-words
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
}
var input = _dataSet.ToList();

var _measureTimePerformance = new MeasureTimePerformance();

Console.WriteLine("----------------SimpleRadixTree performances-----------------------");

SimpleRadixTree _simpleRadixTree = new();

var ramAllocationSimpleRadixTreeBeforeInsertion = Process.GetCurrentProcess().WorkingSet64;
var allocationInMBSimpleRadixTreeBeforeInsertion = ramAllocationSimpleRadixTreeBeforeInsertion / (1024* 1024);

_measureTimePerformance.Init();
_measureTimePerformance.Start();

var _rootNodeSimpleRadixTree = _simpleRadixTree.Add(input);

_measureTimePerformance.Stop();

var ramAllocationSimpleRadixTreeAfterInsertion = Process.GetCurrentProcess().WorkingSet64;
var allocationInMBSimpleRadixTreeAfterInsertion = ramAllocationSimpleRadixTreeAfterInsertion / (1024 * 1024);

Console.WriteLine($"Insertion of {input.Count} sorted values in simple radix tree completed in ${_measureTimePerformance.GetElapsedTime()}");
Console.WriteLine($"Memory allocation for the simple radix tree insert function : {allocationInMBSimpleRadixTreeAfterInsertion - allocationInMBSimpleRadixTreeBeforeInsertion} MB");

var allAddedWordsFromSimpleRadixTree = _simpleRadixTree.GetAllInsertedWords(_rootNodeSimpleRadixTree);

var valueToLookupIntoSimpleRadixTree = input[350903]; //in json file : unshakiness
_measureTimePerformance.Init();
_measureTimePerformance.Start();

var valueRetrievedFromSimleRadixTree = compressedRadixTree.Lookup(valueToLookupIntoSimpleRadixTree.Key, _rootNodeSimpleRadixTree);

_measureTimePerformance.Stop();
if (valueRetrievedFromSimleRadixTree == -1)
    Console.WriteLine($"The value for the following string: {valueToLookupIntoSimpleRadixTree.Key} was not found in tree!");
else
    Console.WriteLine($"The value found by the lookup function for string: {valueToLookupIntoSimpleRadixTree.Key} is {valueRetrievedFromSimleRadixTree}");

int nbWordsToLookup = 3000;
//compare lookup value with Contains vs tree lookup fucntion
var lookupWordsListForSimpleRadixTree = input.Skip(300000).Take(nbWordsToLookup);
_measureTimePerformance.Init();
_measureTimePerformance.Start();
foreach (var word in lookupWordsListForSimpleRadixTree)
{
    allAddedWordsFromSimpleRadixTree.Contains(word.Key);
}
_measureTimePerformance.Stop();
Console.WriteLine($"lookup {nbWordsToLookup} values using contains completed in ${_measureTimePerformance.GetElapsedTime()}");

_measureTimePerformance.Init();
_measureTimePerformance.Start();
foreach (var word in lookupWordsListForSimpleRadixTree)
{
    _simpleRadixTree.Lookup(word.Key, _rootNodeSimpleRadixTree);
}
_measureTimePerformance.Stop();
Console.WriteLine($"lookup {nbWordsToLookup} values using the simple radix tree lookup function completed in ${_measureTimePerformance.GetElapsedTime()}");

//since the list of random words I picked on internet is already sorted and may help for the insert algorithm to work efficiently
//try to "unsort" the dataset and create a tree and compare with the previous one
var shuffledListForSimpleRadixTree = input.Shuffle();
CompressedRadixTree _simpleRadixTreeWithShuffledList = new();

_measureTimePerformance.Init();
_measureTimePerformance.Start();

var _resultSimpleTreeWithShuffledList = _simpleRadixTreeWithShuffledList.Add(shuffledListForSimpleRadixTree);

_measureTimePerformance.Stop();

Console.WriteLine($"Insertion of {shuffledListForSimpleRadixTree.Count} unsorted values in simple tree completed in ${_measureTimePerformance.GetElapsedTime()}");

Console.WriteLine("-------------------------------------------------------------------");
Console.WriteLine();

GC.Collect();

Console.WriteLine("----------------CompressedRadixTree performances-----------------------");

CompressedRadixTree _radixTree = new();

var ramAllocationCompressedRadixTreeBeforeInsertion = Process.GetCurrentProcess().WorkingSet64;
var allocationInMBCompressedRadixTreeBeforeInsertion = ramAllocationCompressedRadixTreeBeforeInsertion / (1024 * 1024);

_measureTimePerformance.Init();
_measureTimePerformance.Start();

var _result = _radixTree.Add(input);

_measureTimePerformance.Stop();

var ramAllocationCompressedRadixTreeAfterInsertion = Process.GetCurrentProcess().WorkingSet64;
var allocationInMBCompressedRadixTreeAfterInsertion = ramAllocationCompressedRadixTreeAfterInsertion / (1024 * 1024);

Console.WriteLine($"Insertion of {input.Count} sorted values in tree completed in ${_measureTimePerformance.GetElapsedTime()}");
Console.WriteLine($"Memory allocation for the compressed radix tree insert function : {allocationInMBCompressedRadixTreeAfterInsertion - allocationInMBCompressedRadixTreeBeforeInsertion} MB");

var _allAddedWords = _radixTree.GetAllInsertedWords(_result);

Console.WriteLine("Check if the number of records inserted in tree matches the dataset given to feed the tree...");
bool _hasIssue = (_allAddedWords.Count != input.Count);
if (_hasIssue)
    throw new Exception("There was an issue with the insert algorithm !");

//compare lookup value with Contains vs tree lookup fucntion
var lookupWordsList = input.Skip(300000).Take(nbWordsToLookup);
if (!_hasIssue)
{
    _measureTimePerformance.Init();
    _measureTimePerformance.Start();
    foreach(var word in lookupWordsList)
    {
        _allAddedWords.Contains(word.Key);
    }
    _measureTimePerformance.Stop();
    Console.WriteLine($"lookup {lookupWordsList.Count()} values using contains completed in ${_measureTimePerformance.GetElapsedTime()}");
}

//search with the tree's lookup function is amazingly fast!
if (!_hasIssue)
{
    _measureTimePerformance.Init();
    _measureTimePerformance.Start();
    foreach (var word in lookupWordsList)
    {
        _radixTree.Lookup(word.Key, _result);
    }
    _measureTimePerformance.Stop();
    Console.WriteLine($"lookup {lookupWordsList.Count()} values using the tree lookup function completed in ${_measureTimePerformance.GetElapsedTime()}");
}

//since the list of random words I picked on internet is already sorted and may help for the insert algorithm to work efficiently
//try to "unsort" the dataset and create a tree and compare with the previous one
var shuffledList = input.Shuffle();
CompressedRadixTree _radixTreeWithShuffledList = new();

_measureTimePerformance.Init();
_measureTimePerformance.Start();

var _resultTreeWithShuffledList = _radixTreeWithShuffledList.Add(shuffledList);

_measureTimePerformance.Stop();

Console.WriteLine($"Insertion of {shuffledList.Count} unsorted values in tree completed in ${_measureTimePerformance.GetElapsedTime()}");

var _allAddedWordsWithShuffledList = _radixTreeWithShuffledList.GetAllInsertedWords(_resultTreeWithShuffledList);
bool _hasTreeWithShuffledListIssue = _allAddedWordsWithShuffledList.Count != shuffledList.Count;
if(_hasTreeWithShuffledListIssue)
    throw new Exception("There was an issue with the insert algorithm !");

Console.WriteLine("-------------------------------------------------------------------");

Console.ReadLine();

//merkle tree code section

MerkleTree merkleTree = new(250);
string root = merkleTree.GetRoot();
Console.Out.WriteLine(root);

Console.ReadLine();