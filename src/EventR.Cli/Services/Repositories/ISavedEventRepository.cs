using EventR.Cli.Models;

namespace EventR.Cli.Services.Repositories;

public interface ISavedEventRepository : IRepository<SavedEvent>
{
    IEnumerable<SavedEvent> LoadLocation(SavedEventLocation location);
}
