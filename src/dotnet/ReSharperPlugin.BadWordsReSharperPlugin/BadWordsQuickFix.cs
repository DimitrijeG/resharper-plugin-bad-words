using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Feature.Services.Refactorings.Specific.Rename;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Resx.Utils;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using ReSharperPlugin.BadWordsReSharperPlugin.ElementProblemAnalyzer;

namespace ReSharperPlugin.BadWordsReSharperPlugin;

[QuickFix]
public class BadWordsQuickFix : QuickFixBase
{
    private readonly IComment _comment;

    public BadWordsQuickFix([NotNull] BadWordNamingWarning warning)
    {
        _comment = warning.Comment;
    }

    protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
    {
        // var newText = Regex.Replace(_comment.CommentText, "crap", "BADWORD", RegexOptions.IgnoreCase);

        var factory = CSharpElementFactory.GetInstance(_comment);
        var newComment = factory.CreateComment("// HAIIII");
        ModificationUtil.ReplaceChild(_comment, newComment);
        return null;
    }

    public override string Text => "Replace the bad word";

    public override bool IsAvailable(IUserDataHolder cache)
    {
        return _comment.IsValid();
    }
}