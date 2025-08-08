using EventR.Cli.Configuration;
using EventR.Cli.Models;
using Microsoft.Extensions.Options;

namespace EventR.Cli.Services.Repositories;

public class SavedEventLocationRepository(IOptions<DebuggerSettings> clientSettings) : TransitoryRepository<SavedEventLocation>
{
    protected override IEnumerable<SavedEventLocation> LoadData()
    {
        return [
            ..clientSettings.Value.SharedEventsFolders.Select(sef => new SavedEventLocation
            {
                Identifier = Guid.NewGuid().ToString(),
                Name = sef.Name,
                Path = sef.Path
            })
        ];
    }
}
