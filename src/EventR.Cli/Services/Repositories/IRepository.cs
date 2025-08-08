using EventR.Cli.Models;

namespace EventR.Cli.Services.Repositories;

public interface IRepository<T> where T : BaseModel
{
    T? GetByIdentifier(string identifier);

    IEnumerable<T> GetAll();

    void Save(T item);

    void Delete(string identifier);
}
