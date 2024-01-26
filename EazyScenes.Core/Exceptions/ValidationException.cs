namespace EazyScenes.Core;

public class ValidationException: Exception
{
    #region Properties

    public IEnumerable<Error> ValidationErrors { get; }

    #endregion

    #region Construction

    public ValidationException(IEnumerable<Error> validationErrors)
    {
        ValidationErrors = validationErrors;
    }

    #endregion
}
