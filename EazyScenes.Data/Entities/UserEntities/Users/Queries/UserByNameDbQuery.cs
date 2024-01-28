using Microsoft.EntityFrameworkCore;

namespace EazyScenes.Data.Entities.Queries;

public interface IUserByNameDbQuery: IDbQuery<User>
{
    IUserByNameDbQuery Include(params string[] include);

    IUserByNameDbQuery WithParams(string normalizedUserName);
}

public class UserByNameDbQuery: IUserByNameDbQuery
{
    #region Fields

    private readonly Context _context;


    private string[] _include;
    private string _normalizedUserName;

    #endregion

    #region Construction

    public UserByNameDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<User> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.Users.Where(u => u.NormalizedUserName == _normalizedUserName);

        if (_include != null)
        {
            query = _include.Aggregate(query, (current, expression) => current.Include(expression));
        }

        var user = await query.FirstOrDefaultAsync(cancellationToken);

        return user;
    }

    public IUserByNameDbQuery Include(params string[] include)
    {
        _include = include;
        return this;
    }

    public IUserByNameDbQuery WithParams(string normalizedUserName)
    {
        _normalizedUserName = normalizedUserName;
        return this;
    }

    #endregion
}
