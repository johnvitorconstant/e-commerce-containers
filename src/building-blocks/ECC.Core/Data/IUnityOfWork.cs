namespace ECC.Core.Data;

public interface IUnityOfWork
{
    Task<bool> Commit();
}