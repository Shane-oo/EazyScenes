using System.Security.Claims;
using Carter;
using EazyScenes.Core;
using EazyScenes.Users.Authentications.AuthenticateUserWithPassword;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddictErrors = OpenIddict.Abstractions.OpenIddictConstants.Errors;
using OpenIddictClaims = OpenIddict.Abstractions.OpenIddictConstants.Claims;
using OpenIddictDestinations = OpenIddict.Abstractions.OpenIddictConstants.Destinations;
using OpenIddictScopes = OpenIddict.Abstractions.OpenIddictConstants.Scopes;

namespace EazyScenes.Users.Authentications;

[UsedImplicitly]
public class AuthenticationsModule: CarterModule
{
    #region Constants

    private const string AUTH_SCHEME = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;

    #endregion

    #region Construction

    public AuthenticationsModule(): base("api/authentications")
    {
    }

    #endregion

    #region Private Methods

    private static IResult Forbid(Error error)
    {
        return Results.Forbid(new AuthenticationProperties(new Dictionary<string, string>
                                                           {
                                                               [OpenIddictServerAspNetCoreConstants.Properties.Error] = error.Code,
                                                               [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                                                   error.Description
                                                           }),
                              new[] { AUTH_SCHEME });
    }

    private static IResult SignInUser(OpenIddictRequest request, UserAuthenticationPayload payload)
    {
        var identity = new ClaimsIdentity(AUTH_SCHEME, OpenIddictClaims.Name, OpenIddictClaims.Role);

        identity.AddClaim(OpenIddictClaims.ClientId, request.ClientId!);
        identity.AddClaim(OpenIddictClaims.Subject, payload.UserId.Value.ToString());
        identity.AddClaim(OpenIddictClaims.Name, payload.UserName);
        identity.AddClaim(OpenIddictClaims.Role, payload.Role);

        // allow all claims to be added in the access tokens
        identity.SetDestinations(_ => new[] { OpenIddictDestinations.AccessToken });

        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(OpenIddictScopes.OfflineAccess, OpenIddictScopes.OpenId, OpenIddictScopes.Roles);

        return Results.SignIn(principal, null, AUTH_SCHEME);
    }

    #endregion

    #region Public Methods

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/token", async (HttpContext httpContext, ISender sender, CancellationToken cancellationToken) =>
                                       {
                                           var request = httpContext.GetOpenIddictServerRequest();

                                           if (request is null)
                                           {
                                               return Forbid(new Error(OpenIddictErrors.InvalidRequestObject, "The token request could not be retrieved"));
                                           }

                                           if (request.IsPasswordGrantType())
                                           {
                                               var command = new AuthenticateUserWithPasswordCommand(request.Username, request.Password);
                                               var result = await sender.Send(command, cancellationToken);

                                               return result.IsFailure ? Forbid(result.ErrorResult) : SignInUser(request, result.Value);
                                           }

                                           return Forbid(new Error(OpenIddictErrors.UnsupportedGrantType, "Grant type not supported"));
                                       });


        app.MapGet("/fakeData",async (IOpenIddictApplicationManager manager) =>
                                {
                                    var application = new OpenIddictApplicationDescriptor
                                                      {
                                                          ClientId = "EazyScenes",
                                                          DisplayName = "EazyScenes Public Client Application",
                                                          Permissions =
                                                          {
                                                              OpenIddictConstants.Permissions.Endpoints.Token,

                                                              OpenIddictConstants.Permissions.GrantTypes.Password,

                                                              OpenIddictConstants.Permissions.ResponseTypes.Token
                                                          },
                                                          ClientType = OpenIddictConstants.ClientTypes.Public
                                                      };
                                    await manager.CreateAsync(application);
                                });
    }

    #endregion
}
