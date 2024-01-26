using Carter;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EazyScenes.Users.Users;

[UsedImplicitly]
public class UsersModule: CarterModule
{
    #region Construction

    public UsersModule(): base("/users")
    {
        //RequireAuthorization();
    }

    #endregion

    #region Public Methods

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:int}", (int id) => { return Results.Ok(id); });

        app.MapPost("/", () =>
                         {
                             var id = 99;
                             return Results.Created($"{id}", id);
                         });
    }

    #endregion
}
