using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

string inputFile = "input.txt";
string outputFile = "output.txt";
List<string> stopWords = new() { "in", "the", "for", "a", "to", "on", "at", "with", "about", "before" }; 

Dictionary<string, List<int>> tearmFrequancy = new();
var bookStream = File.OpenText(inputFile);
int outputLines = 25;

using (var outputStream = new StreamWriter(outputFile))
{
    int pageIndex = 1;
    int lineIndex = 1;
    int linesPrePage = 45;
    
    ReadBook:
    if (!bookStream.EndOfStream)
    {
        if (lineIndex % linesPrePage == 0)
            pageIndex++;
        
        string line = bookStream.ReadLine()?.ToLower() ?? string.Empty;
        int characterPointer = 0;
        string word = "";
    
        ReadLine:
        if (characterPointer < line.Length)
        {
            if (line[characterPointer] == ' ')
            {
                word = Regex.Replace(word.ToLower(), "[^a-zA-Z]", "");
                if (string.IsNullOrEmpty(word))
                {
                    goto StartNextWord;
                }
    
                int stopWordIndexer = 0;
                TestWord:
                if (stopWordIndexer < stopWords.Count)
                {
                    if(stopWords[stopWordIndexer] == word)
                        goto StartNextWord;
                
                    stopWordIndexer++;
                    goto TestWord;
                }
            
                if (!tearmFrequancy.ContainsKey(word))
                {
                    tearmFrequancy.Add(word, new List<int> {pageIndex});
                }
                else
                {
                    tearmFrequancy[word].Add(pageIndex);
                    if (tearmFrequancy[word].Count >= 100)
                    {
                        tearmFrequancy.Remove(word);
                        stopWords.Add(word);
                    }
                }
            
                StartNextWord:
                word = "";
            }else
                word += line[characterPointer];
        
            characterPointer++;
            goto ReadLine;
        }
    
        lineIndex++;
        goto ReadBook;
    }
    
    var wordsArr = tearmFrequancy.Keys.ToArray();
    int num = wordsArr.Length;
    int i = 0;
    int j = 0;
    Sort1:
    if(i < num - 1)
    {
        j = 0;
        
        Sort2:
        if (j < num - i - 1)
        {
            if (String.Compare(wordsArr[j], wordsArr[j + 1], StringComparison.Ordinal) > 0)
            {
                var tmp = wordsArr[j];
                wordsArr[j] = wordsArr[j + 1];
                wordsArr[j + 1] = tmp;
            }
            
            j++;
            goto Sort2;
        }
        
        i++;
        goto Sort1;
    }
    
    int wordPointer = 0;
    PrintWord:
    if (wordPointer < wordsArr.Length && wordPointer < outputLines)
    {
        Console.Write($"{wordsArr[wordPointer]} - ");
        outputStream.Write($"{wordsArr[wordPointer]} - ");
        int dataIndex = 0;
        PrintPage:
        if (dataIndex < tearmFrequancy[wordsArr[wordPointer]].Count)
        {
            Console.Write(tearmFrequancy[wordsArr[wordPointer]][dataIndex] + " ");
            outputStream.Write(tearmFrequancy[wordsArr[wordPointer]][dataIndex] + " ");
            dataIndex++;
            goto PrintPage;
        }
        Console.Write('\n');
        outputStream.Write('\n');
        
        wordPointer++;
        goto PrintWord;
    }
}

