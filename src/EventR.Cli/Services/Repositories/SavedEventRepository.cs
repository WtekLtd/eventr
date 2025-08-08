using EventR.Cli.Models;

namespace EventR.Cli.Services.Repositories;

public class SavedEventRepository(IRepository<SavedEventLocation> savedEventLocationRepository) : Repository<SavedEvent>, ISavedEventRepository
{
    public IEnumerable<SavedEvent> LoadLocation(SavedEventLocation location)
    {
        foreach (var savedEvent in LoadSavedEvents(location))
        {
            ImportItem(savedEvent);
            yield return savedEvent;
        }
    }

    protected override IEnumerable<SavedEvent> LoadData()
    {
        var locations = savedEventLocationRepository.GetAll();
        return locations
            .SelectMany(LoadSavedEvents);
    }

    protected override void PersistData(SavedEvent item)
    {
        if (!Directory.Exists(item.Location.Path))
        {
            Directory.CreateDirectory(item.Location.Path);
        }

        var filePath = Path.Combine(item.Location.Path, $"{item.Name}.json");
        File.WriteAllText(filePath, item.Data);
    }

    private IEnumerable<SavedEvent> LoadSavedEvents(SavedEventLocation location)
    {
        if (!Directory.Exists(location.Path))
        {
            yield break;
        }

        var files = Directory.GetFiles(location.Path);
        foreach (var file in files)
        {
            var data = File.ReadAllText(file);
            yield return new()
            {
                Data = data,
                Identifier = Guid.NewGuid().ToString(),
                Location = location,
                Name = Path.GetFileNameWithoutExtension(file)
            };
        }
    }
}