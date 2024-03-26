namespace Referral.Repositories;

public interface IUnitOfWork
{
    IClientRepository Client { get; }
    void Save();
}