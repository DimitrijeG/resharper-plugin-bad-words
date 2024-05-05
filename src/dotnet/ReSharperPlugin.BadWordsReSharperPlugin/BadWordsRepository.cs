using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains;
using JetBrains.Application.Threading;
using JetBrains.Application.Threading.Tasks;
using JetBrains.DataFlow;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.PsiGen.Util;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.ReSharper.TestRunner.Abstractions.Extensions;
using JetBrains.ReSharper.UnitTestFramework.Common.Extensions;
using TaskScheduler = JetBrains.Application.Threading.AsyncProcessing.TaskScheduler;

namespace ReSharperPlugin.BadWordsReSharperPlugin;

public static class BadWordsRepository
{
    private const string Delimiter = "==>";

    private const string DirPath =
        "/Users/dimitrijegasic/OneDrive - Univerzitet u Novom Sadu/Projects/jetbrains/bad_words";

    public static ConcurrentDictionary<string, string> Words { get; } = new();

    public static string GetMatchWord(string text)
    {
        return Words.Keys.FirstOrDefault(text.ToLower().Contains);
    }

    
    private static readonly FileSystemWatcher Watcher;
    
    static BadWordsRepository()
    {
        LoadAllFiles();
        
        Watcher = new FileSystemWatcher(DirPath)
        {
            NotifyFilter = NotifyFilters.LastWrite
        };
        Watcher.Changed += (_, _) => LoadAllFiles();
        Watcher.Renamed += (_, _) => LoadAllFiles();
        Watcher.Deleted += (_, _) => LoadAllFiles();
        Watcher.EnableRaisingEvents = true;
    }

    private static void LoadAllFiles()
    {
        Words.Clear();
        Directory.EnumerateFiles(DirPath, "*.txt").ForEach(
            path => Task.Run(() => ParseFile(path)));
    }

    private static void ParseFile(string path)
    {
        if (Path.GetExtension(path) != ".txt")
            return;

        using var sr = new StreamReader(path);
        sr.ReadToEnd().Split('\n').ForEach(line =>
        {
            var parts = line.ToLower().Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0) return;
            if (parts.Length != 2)
                throw new IOException("ReSharper bad words registry has invalid line format.");
            
            Words[parts[0].Trim()] = parts[1].Trim();
        });
    }
}