using EventR.Cli.Models;

namespace EventR.Cli.Services.Repositories;

public abstract class Repository<T> : IRepository<T> where T : BaseModel
{
    private readonly object _lock = new();

    private readonly Dictionary<string, T> _items = [];

    private bool _isLoaded = false;

    public IEnumerable<T> GetAll()
    {
        Initialise();
        return _items.Values;
    }

    public T? GetByIdentifier(string identifier)
    {
        Initialise();
        return _items.TryGetValue(identifier, out var endpoint) ? endpoint : null;
    }

    public void Save(T item)
    {
        PersistData(item);
        ImportItem(item);
    }

    public void Delete(string identifier)
    {
        lock (_lock)
        {
            _items.Remove(identifier);
        }
    }

    protected abstract IEnumerable<T> LoadData();

    protected abstract void PersistData(T item);

    protected void ImportItem(T item)
    {
        lock (_lock)
        {
            _items[item.Identifier] = item;
        }
    }

    private void Initialise()
    {
        lock (_lock)
        {
            if (!_isLoaded)
            {
                foreach (var item in LoadData())
                {
                    _items.Add(item.Identifier, item);
                }
                _isLoaded = true;
            }
        }
    }
}
