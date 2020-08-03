namespace SearchFight.Services.Interfaces
{
    internal interface IHtmlParser
    {
        int ExtractNumber(string text);
        int AfterIndexOfTag(string text, string tag, int startIndex);
        int IndexOfTag(string text, string tag, int startIndex);
    }
}