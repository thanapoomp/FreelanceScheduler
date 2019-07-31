using System.Threading.Tasks;
using freelance.api.Models;

namespace freelance.api.Data
{
    public interface IDataRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
         Task<User> GetUser(int id);
    }
}