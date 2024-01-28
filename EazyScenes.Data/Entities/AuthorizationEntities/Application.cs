using OpenIddict.EntityFrameworkCore.Models;

namespace EazyScenes.Data.Entities.AuthorizationEntities;

public class Application: OpenIddictEntityFrameworkCoreApplication<int, Authorization, Token>;
