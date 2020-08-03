using System;
using System.Linq;
using Search.Common.Extensions;
using SearchFight.Services.Exceptions;
using SearchFight.Services.Interfaces;

namespace SearchFight.Services.Services
{
    internal class HtmlParser: IHtmlParser
    {
        public int ExtractNumber(string text)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));

            var chars = text.Where(char.IsNumber).ToArray();
            if (!int.TryParse(string.Join("", chars), out var number))
            {
                throw new DataSearcherException($"Error parsing html. Can't build result number from text: \"{text}\"");
            }

            return number;
        }

        public int AfterIndexOfTag(string text, string tag, int startIndex)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));

            startIndex = text.AfterIndexOf(tag, startIndex, StringComparison.InvariantCulture);
            if (startIndex == -1)
            {
                throw new DataSearcherException($"Error parsing html: Can't find tag. \"{tag}\"");
            }

            if (startIndex == text.Length)
            {
                throw new DataSearcherException("Error parsing html: No text after found tag");
            }

            return startIndex;
        }

        public int IndexOfTag(string text, string tag, int startIndex)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            
            var result = text.IndexOf(tag, startIndex, StringComparison.InvariantCulture);
            if (result == -1)
            {
                throw new DataSearcherException($"Error parsing html: Can't find tag. \"{tag}\"");
            }

            return result;
        }
    }
}