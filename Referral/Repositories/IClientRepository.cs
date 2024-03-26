using Referral.Model;

namespace Referral.Repositories;

public interface IClientRepository : IRepository<Client>
{
    void Update(Client obj);
}