using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace ReSharperPlugin.BadWordsReSharperPlugin.ElementProblemAnalyzer;

[ElementProblemAnalyzer(typeof(IComment), HighlightingTypes =
    new[] {typeof(BadWordNamingWarning)})]
public class BadWordNamingAnalyzer : ElementProblemAnalyzer<IComment>
{
    protected override void Run(IComment element, ElementProblemAnalyzerData data,
        IHighlightingConsumer consumer)
    {
        if (!MatchesBadWord(element.CommentText))
            return;

        consumer.AddHighlighting(new BadWordNamingWarning(element, element.GetDocumentRange()));
    }

    private static bool MatchesBadWord(string text)
    { 
        return BadWordsRepository.GetMatchWord(text) != null;
    }
}