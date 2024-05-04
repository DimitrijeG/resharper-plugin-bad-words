using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperPlugin.BadWordsReSharperPlugin.ElementProblemAnalyzer;

[ElementProblemAnalyzer(typeof(IComment), HighlightingTypes =
    new[] {typeof(BadWordNamingWarning)})]
public class BadWordNamingAnalyzer : ElementProblemAnalyzer<IComment>
{
    protected override void Run(IComment element, ElementProblemAnalyzerData data,
        IHighlightingConsumer consumer)
    {
        var nodeText = element.CommentText.ToLower();

        if (!nodeText.Contains("crap"))
            return;

        var warning = new BadWordNamingWarning(element, element.GetDocumentRange());
        consumer.AddHighlighting(warning);
    }
}