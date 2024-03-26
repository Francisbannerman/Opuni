using Referral.Model;
using Referral.Settings;

namespace Referral.Repositories;

public class ClientRepository : Repository<Client>, IClientRepository
{
    private readonly ApplicationDbContext _db;

    public ClientRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Client obj)
    {
        _db.Clients.Update(obj);
    }
}