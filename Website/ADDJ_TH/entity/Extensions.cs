using System;
using System.Linq;
using System.Text.RegularExpressions;


public static class Extensions
{
    /// <summary>
    /// Wraps matched strings in HTML span elements styled with a background-color
    /// </summary>
    /// <param name="text"></param>
    /// <param name="keywords">Comma-separated list of strings to be highlighted</param>
    /// <param name="fullMatch">false for returning all matches, true for whole word matches only</param>
    /// <returns>string</returns>
    public static string HighlightKeyWords(this string text, string keywords, bool fullMatch)
    {
        if (text == String.Empty || keywords == String.Empty)
            return text;
        var words = keywords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (!fullMatch)
            return words.Select(word => word.Trim()).Aggregate(text,
                         (current, pattern) =>
                         Regex.Replace(current,
                                         pattern,
                                           string.Format("<backcolor=yellow><color=red><b>{0}</b></color></backcolor>",                                          
                                           "$0"),
                                           RegexOptions.IgnoreCase));
        return words.Select(word => "\\b" + word.Trim() + "\\b")
                    .Aggregate(text, (current, pattern) =>
                              Regex.Replace(current,
                              pattern,
                                string.Format("<backcolor=yellow><color=red><b>{0}</b></color></backcolor>",
                                "$0"),
                                RegexOptions.IgnoreCase));

    }
    public static string HighlightKeywords(this string input, string keywords)
    {
        if (input == String.Empty || keywords == String.Empty)
        {
            return input;
        }

        return Regex.Replace(
            input,
            String.Join("|", keywords.Split(' ').Select(x => Regex.Escape(x))),
            string.Format("<span class='highlightme'>{0}</span>", "$0"),
            RegexOptions.IgnoreCase);
    }
}