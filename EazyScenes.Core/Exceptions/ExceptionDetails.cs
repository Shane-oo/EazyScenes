namespace EazyScenes.Core;

public class ExceptionDetails
{
    #region Properties

    public string Detail { get; }

    public IEnumerable<Error> Errors { get; }

    public int Status { get; }

    public string Title { get; }

    public string Type { get; }

    #endregion

    #region Construction

    public ExceptionDetails(int status, string type, string title, string detail, params Error[] errors)
    {
        Status = status;
        Type = type;
        Title = title;
        Detail = detail;
        Errors = errors;
    }

    #endregion
}
