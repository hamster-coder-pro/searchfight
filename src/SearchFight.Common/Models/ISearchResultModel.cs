namespace SearchFight.Common.Models
{
    public interface ISearchResultModel
    {
        bool IsSucceed { get; set; }
        string? Error { get; set; }
    }
}