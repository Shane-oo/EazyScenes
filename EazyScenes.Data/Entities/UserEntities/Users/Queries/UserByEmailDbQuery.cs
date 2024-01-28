using Microsoft.EntityFrameworkCore;

namespace EazyScenes.Data.Entities.Queries;

public interface IUserByEmailDbQuery: IDbQuery<User>
{
    IUserByEmailDbQuery WithParams(string normalizedEmail);
}

public class UserByEmailDbQuery: IUserByEmailDbQuery
{
    #region Fields

    private readonly Context _context;

    private string _normalizedEmail;

    #endregion

    #region Construction

    public UserByEmailDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public Task<User> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.Users.Where(u => u.NormalizedEmail.Equals(_normalizedEmail));

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public IUserByEmailDbQuery WithParams(string normalizedEmail)
    {
        _normalizedEmail = normalizedEmail;
        return this;
    }

    #endregion
}
