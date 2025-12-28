using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;


namespace DoomMapGuessr.Services.Markdown
{

    /// <summary>
    /// A simplified Markdown to Avalonia Controls parser.
    /// </summary>
    /// <remarks>
    /// Initializes a new Markdown parser.
    /// </remarks>
    /// <param name="paragraphSpacing">The paragraph spacing to apply</param>
    public partial class MarkdownParser(double paragraphSpacing)
    {

        private double ParagraphSpacing => paragraphSpacing;

        /// <summary>
        /// Parses simplified Markdown and converts it into a
        /// <see cref="StackPanel"/> containing other controls.
        /// </summary>
        /// <param name="markdown">The markdown</param>
        /// <param name="dataIfNull">The <see cref="StackPanel"/> to use if no data; empty if <c>null</c></param>
        /// <returns></returns>
        public StackPanel Parse(string markdown, StackPanel? dataIfNull = null)
        {

            if (String.IsNullOrWhiteSpace(markdown))
                return dataIfNull ?? new();

            StackPanel container = new()
            {
                Spacing = ParagraphSpacing
            };

            string[] lines = markdown.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            int curLineIdx = 0;

            while (curLineIdx < lines.Length)
            {

                string line = lines[curLineIdx].TrimEnd('\r'); // gets rid of those yucky carriage returns cuz fuck bill gates

                // Headingsss
                if (line.StartsWith("# "))
                    container.Children.Add(CreateTextBlock(Clean(line[2..]), 1));
                else if (line.StartsWith("## "))
                    container.Children.Add(CreateTextBlock(Clean(line[3..]), 2));
                else if (line.StartsWith("### "))
                    container.Children.Add(CreateTextBlock(Clean(line[4..]), 3));
                else if (line.StartsWith("-# ")) // discord style caption
                    container.Children.Add(CreateTextBlock(Clean(line[4..]), -1));

                // Code :)
                else if (line.Trim().StartsWith("```"))
                {

                    var codeBlock = ParseCodeBlock(lines, ref curLineIdx);
                    container.Children.Add(codeBlock);

                }

                // quoteee
                else if (line.StartsWith("> "))
                    container.Children.Add(CreateQuote(line[2..]));

                else
                {

                    string cleanedText = Clean(line);

                    if (!String.IsNullOrWhiteSpace(cleanedText))
                        container.Children.Add(CreateTextBlock(cleanedText));

                }

            }

            return container;

        }

        private static string Clean(string text)
        {

            if (String.IsNullOrWhiteSpace(text))
                return String.Empty;

            string cleaned = text;

            // No bold
            cleaned = BoldStar().Replace(cleaned, "$1");
            cleaned = BoldUnderscore().Replace(cleaned, "$1");

            // No italic
            cleaned = ItalicStar().Replace(cleaned, "$1");
            cleaned = ItalicUnderscore().Replace(cleaned, "$1");

            // No inline code
            cleaned = InlineCode().Replace(cleaned, "$1");

            // No links but the text in the links should be kept i think
            cleaned = Links().Replace(cleaned, "$1");

            // No fucking imagens, fucking dumbass
            cleaned = Images().Replace(cleaned, "");

            // No lists
            cleaned = UnorderedLists().Replace(cleaned, "");
            cleaned = OrderedLists().Replace(cleaned, "");

            // No horizontal rules
            if (HorizontalRule().IsMatch(cleaned))
                return String.Empty;

            // No code block markers
            cleaned = cleaned.Replace("```", "");

            // No table pipes
            cleaned = cleaned.Replace("|", " ");

            // No strikethrough
            cleaned = Strikethrough().Replace(cleaned, "$1");

            // No HTML tags
            cleaned = HtmlTags().Replace(cleaned, "");

            // Multiple whitespace becomes single space.
            cleaned = MultipleWhitespace().Replace(cleaned, " ");

            return cleaned.Trim();

        }

        private static TextBox ParseCodeBlock(string[] lines, ref int index)
        {

            index++; // we gotta skip the opening code thingy
            List<string> codeLines = [];

            while (index < lines.Length && !lines[index].Trim().StartsWith("```"))
            {

                codeLines.Add(lines[index].TrimEnd('\r'));
                ++index;

            }

            if (index < lines.Length)
                index++;

            string code = String.Join('\n', codeLines);
            return new()
            {

                Text = code,
                FontFamily = ((FontFamily?)Application.Current?.FindResource("CascadiaCode")) ?? new FontFamily("Consolas,Courier New,monospace"),
                FontSize = 14,
                FontWeight = FontWeight.SemiBold,
                TextWrapping = TextWrapping.NoWrap,
                TextAlignment = TextAlignment.Start,
                AcceptsTab = true,
                AcceptsReturn = true,
                IsReadOnly = true // good shite

            };

        }

        private static TextBlock CreateTextBlock(string text, int headingLevel = 0)
        {

            int fontSize = headingLevel switch
            {

                -1 => 10,
                1 => 28,
                2 => 24,
                3 => 20,
                _ => 14,

            };

            var fontWeight = headingLevel switch
            {

                -1 => FontWeight.Normal,
                1 => FontWeight.Black,
                2 => FontWeight.Bold,
                3 => FontWeight.Bold,
                _ => FontWeight.Normal, // just assume normal instead of IdealWeight

            };

            return new()
            {

                Text = text.Trim(),
                FontWeight = fontWeight,
                FontSize = fontSize,
                TextWrapping = TextWrapping.Wrap

            };

        }

        private static TextBlock CreateQuote(string text) => new()
        {

            Text = text.Trim(),
            FontWeight = FontWeight.SemiLight,
            FontSize = 10,
            FontStyle = FontStyle.Italic,
            TextWrapping = TextWrapping.Wrap

        };

        #region GeneratedRegex

        [GeneratedRegex(@"\s+")]
        private static partial Regex MultipleWhitespace();

        [GeneratedRegex(@"<[^>]+>")]
        private static partial Regex HtmlTags();

        [GeneratedRegex(@"~~(.+?)~~")]
        private static partial Regex Strikethrough();

        [GeneratedRegex(@"^[-*]{3,}$")]
        private static partial Regex HorizontalRule();

        [GeneratedRegex(@"^[\s]*[-*]\s+")]
        private static partial Regex UnorderedLists();

        [GeneratedRegex(@"^[\s]*\d+\.\s+")]
        private static partial Regex OrderedLists();

        [GeneratedRegex(@"!\[.*?\]\(.+?\)")]
        private static partial Regex Images();

        [GeneratedRegex(@"\[(.+?)\]\(.+?\)")]
        private static partial Regex Links();

        [GeneratedRegex(@"`(.+?)`")]
        private static partial Regex InlineCode();

        [GeneratedRegex(@"(?<!_)_(?!_)(.+?)(?<!_)_(?!_)")]
        private static partial Regex ItalicUnderscore();

        [GeneratedRegex(@"(?<!\*)\*(?!\*)(.+?)(?<!\*)\*(?!\*)")]
        private static partial Regex ItalicStar();

        [GeneratedRegex(@"__(.+?)__")]
        private static partial Regex BoldUnderscore();

        [GeneratedRegex(@"\*\*(.+?)\*\*")]
        private static partial Regex BoldStar();

        #endregion

    }

}

