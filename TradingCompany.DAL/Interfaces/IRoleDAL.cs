using TradingCompany.DTO;

namespace TradingCompany.DAL.Interfaces
{
    public interface IRoleDAL: IGenericDAL<Role>
    {
        bool AddRoleToUser(int userId, RoleType roleType);
        bool RemoveRoleFromUser(int userId, RoleType roleType);
    }
}
