using EventR.Cli.Services.Repositories;

namespace EventR.Cli.Models;

public class TransitoryRepository<T> : Repository<T> where T : BaseModel
{
    protected override IEnumerable<T> LoadData()
    {
        return [];
    }

    protected override void PersistData(T item)
    {
        
    }
}
