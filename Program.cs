using System.Text.Json.Nodes;
using BlockChainDataStructures;
using BlockChainDataStructures.RadixdataStructure;
using BlockchainDataStructures.RadixDataStructure.Utils;
using BlockchainDataStructures.Utils;
using System;

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

RadixTree _radixTree = new();
var input = _dataSet.ToList();

var _measureTimePerformance = new MeasureTimePerformance();
_measureTimePerformance.Init();
_measureTimePerformance.Start();

var _result = _radixTree.Add(input);

_measureTimePerformance.Stop();

Console.WriteLine($"Insertion of {input.Count} sorted values in tree completed in ${_measureTimePerformance.GetElapsedTime()}");

var _allAddedWords = _radixTree.GetAllInsertedWords(_result);

prettyPrintTree = new PrettyPrintTree();
//prettyPrintTree.PrintToConsole(_result, "", true);
//var fileToWrite = Path.Combine(thisDir, "RadixDataStructure", "treeWithSortedInput.txt");
//File.WriteAllText(fileToWrite, prettyPrintStr);
//Console.WriteLine($"Tree structure written in file: {fileToWrite}");

Console.WriteLine("Check if the number of records inserted in tree matches the dataset given to feed the tree...");
bool _hasIssue = (_allAddedWords.Count != input.Count);
if (_hasIssue)
    throw new Exception("There was an issue with the insert algorithm !");

//compare lookup value with Contains vs tree lookup fucntion
var lookupWordsList = input.Skip(300000).Take(3000);
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
RadixTree _radixTreeWithShuffledList = new();

_measureTimePerformance.Init();
_measureTimePerformance.Start();

var _resultTreeWithShuffledList = _radixTreeWithShuffledList.Add(shuffledList);

_measureTimePerformance.Stop();

Console.WriteLine($"Insertion of {shuffledList.Count} unsorted values in tree completed in ${_measureTimePerformance.GetElapsedTime()}");

prettyPrintTree = new PrettyPrintTree();
//prettyPrintTree.PrintToConsole(_resultTreeWithShuffledList, "", true);
//var _fileToWrite = Path.Combine(thisDir, "RadixDataStructure", "treeWithUnSortedInput.txt");
//File.WriteAllText(_fileToWrite, prettyPrintStr);
//Console.WriteLine($"Tree structure written in file: {_fileToWrite}");

var _allAddedWordsWithShuffledList = _radixTreeWithShuffledList.GetAllInsertedWords(_resultTreeWithShuffledList);
bool _hasTreeWithShuffledListIssue = _allAddedWordsWithShuffledList.Count != shuffledList.Count;
if(_hasTreeWithShuffledListIssue)
    throw new Exception("There was an issue with the insert algorithm !");

Console.ReadLine();

//merkle tree code section

MerkleTree merkleTree = new(250);
string root = merkleTree.GetRoot();
Console.Out.WriteLine(root);

Console.ReadLine();