using Carter;
using EazyScenes.Users.Users.CreateUser;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EazyScenes.Users.Users;

[UsedImplicitly]
public class UsersModule: CarterModule
{
    #region Construction

    public UsersModule(): base("api/users")
    {
    }

    #endregion

    #region Public Methods

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:int}", (int id) =>
                                {
                                    return Results.Ok(id);
                                })
           .RequireAuthorization("Admin");

        app.MapPost("/", async (CreateUserRequest request, ISender sender, CancellationToken cancellationToken) =>
                                            {
                                                var command = new CreateUserCommand(request.Email, request.UserName, request.Password, request.ConfirmPassword);
                                                var result = await sender.Send(command, cancellationToken);

                                                return result.IsFailure ? Results.BadRequest(result.ErrorResult) : Results.Created();
                                            });
    }

    #endregion
}
