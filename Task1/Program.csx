using System;
using System.IO;

string inputFile = "input.txt";
string outputFile = "output.txt";
string inputString = File.OpenText(inputFile).ReadToEnd().ToLower();
string[] stopWords = { "in", "the", "for", "a", "to", "on", "at", "with", "about", "before" };

int termPointer = 0;
Term[] terms = new Term[7];

using (var outputStream = new StreamWriter(outputFile))
{
    int characterPointer = 0;
    
    string word = "";
    
    ReadInput:
    if (characterPointer < inputString.Length)
    {
        if (inputString[characterPointer] == ' ')
        {
            int stopWordIndexer = 0;
            TestWord:
            if (stopWordIndexer < stopWords.Length)
            {
                if(stopWords[stopWordIndexer] == word) 
                    goto StartNextWord;
                
                stopWordIndexer++;
                goto TestWord;
            }
    
            bool isFound = false;
            int termIndexer = 0;
            TryFind:
            if (termIndexer < termPointer && !isFound) 
            {
                if (terms[termIndexer].Word == word)
                {
                    isFound = true;
                }
                else
                    termIndexer++;
                goto TryFind;
            }
    
            if (!isFound)
            {
                termPointer++;
                terms[termIndexer].Word = word;
    
                if (termPointer > terms.Length * .7f)
                {
                    var expandedArr = new Term[(int)(terms.Length * 1.35f)];
                    int copyIndexer = 0;
                    CopyArray:
                    if (copyIndexer < terms.Length)
                    {
                        expandedArr[copyIndexer] = terms[copyIndexer];
                        copyIndexer++;
                        goto CopyArray;
                    }
    
                    terms = expandedArr;
                }
            }
            
            terms[termIndexer].Frequency++;
            
            StartNextWord:
            word = "";
        }
        else
            word += inputString[characterPointer];
        
        characterPointer++;
        goto ReadInput;
    }
    
    int num = terms.Length;
    int i = 0;
    int j = 0;
    Sort1:
    if(i < num - 1)
    {
        j = 0;
        
        Sort2:
        if (j < num - i - 1)
        {
            if (terms[j + 1].Frequency > terms[j].Frequency)
            {
                var tmp = terms[j];
                terms[j] = terms[j + 1];
                terms[j + 1] = tmp;
            }
            
            j++;
            goto Sort2;
        }
        
        i++;
        goto Sort1;
    }
    
    bool end = false;
    int wordPointer = 0;
    PrintWord:
    if (wordPointer < terms.Length && !end)
    {
        if (terms[wordPointer].Frequency == 0)
        {
            end = true;
            goto PrintWord;
        }
            
        Console.WriteLine($"{terms[wordPointer].Word} - {terms[wordPointer].Frequency}");
        outputStream.WriteLine($"{terms[wordPointer].Word} - {terms[wordPointer].Frequency}");
        wordPointer++;
        goto PrintWord;
    }
}


public struct Term
{
    public string Word;
    public int Frequency;
};