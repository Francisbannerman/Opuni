using Referral.Settings;

namespace Referral.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public IClientRepository Client { get; private set; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Client = new ClientRepository(_db);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}