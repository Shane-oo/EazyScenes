using Microsoft.EntityFrameworkCore;

namespace EazyScenes.Data.Entities.Queries;

public interface IRoleByRoleIdDbQuery: IDbQuery<Role>
{
    IRoleByRoleIdDbQuery WithParams(RoleId roleId);
}

public class RoleByRoleIdDbQuery: IRoleByRoleIdDbQuery
{
    #region Fields

    private readonly Context _context;
    private RoleId _roleId;

    #endregion

    #region Construction

    public RoleByRoleIdDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<Role> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.Roles.Where(r => r.Id == _roleId);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public IRoleByRoleIdDbQuery WithParams(RoleId roleId)
    {
        _roleId = roleId;
        return this;
    }

    #endregion
}
