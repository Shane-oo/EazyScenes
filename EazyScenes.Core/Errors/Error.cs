namespace EazyScenes.Core;

public class Error
{
    #region Fields

    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "Value cannot be null");

    public static readonly Error Unknown = new("Error.Unknown", "Something went wrong");

    #endregion

    #region Properties

    public string Code { get; }

    public string Description { get; }

    #endregion

    #region Construction

    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public static Error NotFound(string param)
    {
        return new Error($"{param}.NotFound", $"{param} was not found");
    }

    #endregion
}
