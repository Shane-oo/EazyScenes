using System.ComponentModel.DataAnnotations;

namespace EazyScenes.Web;

public class AppSettings
{
    #region Properties

    [Required]
    public string[] ClientUrls { get; init; }

    #endregion
}
