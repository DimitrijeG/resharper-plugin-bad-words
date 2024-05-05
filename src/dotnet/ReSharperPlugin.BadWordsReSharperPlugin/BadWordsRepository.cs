using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.ReSharper.TestRunner.Abstractions.Extensions;

namespace ReSharperPlugin.BadWordsReSharperPlugin;

public static class BadWordsRepository
{
    private const string Delimiter = "==>";

    private const string DirectoryEnvVar = "BAD_WORDS_DIR";

    public static ConcurrentDictionary<string, string> Words { get; } = new();

    public static string GetMatchWord(string text)
    {
        return Words.Keys.FirstOrDefault(text.ToLower().Contains);
    }
    
    private static readonly FileSystemWatcher Watcher;
    
    static BadWordsRepository()
    {
        var dirPath = Environment.GetEnvironmentVariable(DirectoryEnvVar);
        if (dirPath == null)
            throw new IOException($"Environment variable {DirectoryEnvVar} is not set.");
        
        LoadAllFiles(dirPath);
        
        Watcher = new FileSystemWatcher(dirPath)
        {
            NotifyFilter = NotifyFilters.LastWrite
        };
        Watcher.Changed += (_, _) => LoadAllFiles(dirPath);
        Watcher.Renamed += (_, _) => LoadAllFiles(dirPath);
        Watcher.Deleted += (_, _) => LoadAllFiles(dirPath);
        Watcher.EnableRaisingEvents = true;
    }

    private static void LoadAllFiles(string dirPath)
    {
        Words.Clear();
        Directory.EnumerateFiles(dirPath, "*.txt").ForEach(
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