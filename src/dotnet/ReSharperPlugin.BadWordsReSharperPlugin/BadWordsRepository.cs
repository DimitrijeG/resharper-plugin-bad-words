using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains;
using JetBrains.Application.Threading;
using JetBrains.DataFlow;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.TestRunner.Abstractions.Extensions;

namespace ReSharperPlugin.BadWordsReSharperPlugin;

public static class BadWordsRepository
{
    private const string Delimiter = "==>";
    private const string DirPath = "/Users/dimitrijegasic/OneDrive - Univerzitet u Novom Sadu/Projects/jetbrains/bad_words";
    
    public static Dictionary<string, string> Words { get; } = new();

    public static string GetMatchWord(string text)
    {
        return Words.Keys.FirstOrDefault(text.ToLower().Contains);
    }

    
    static BadWordsRepository()
    {
        LoadAllFiles(); // TODO ADD CONCURRENCY
        
        var watcher = new FileSystemWatcher(DirPath)
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
        };
        watcher.Changed += (_, file) => ParseFile(file.FullPath); // TODO FIX NOT UPDATING
        watcher.EnableRaisingEvents = true;
    }

    private static void LoadAllFiles()
    {
        Directory.EnumerateFiles(DirPath, "*.txt").ForEach(ParseFile);
    }

    private static void ParseFile(string path)
    {
        if (Path.GetExtension(path) != ".txt")
            return;
        
        using var sr = new StreamReader(path);
        sr.ReadToEnd().Split('\n').ForEach(line =>
        {
            var parts = line.ToLower().Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                throw new IOException("ReSharper bad words registry has invalid line format.");
            
            Words[parts[0].Trim()] = parts[1].Trim();
        });
    }
}