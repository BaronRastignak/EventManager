using System.Text.Json.Serialization;
using EventManager.Domain.Events;

namespace EventManagerService.ViewModels;

public record SocialEventDTO(
    [property: JsonPropertyName("id")] int? Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("date")] DateTime Date)
{
    public static SocialEventDTO FromSocialEvent(SocialEvent socialEvent)
    {
        return new SocialEventDTO(socialEvent.Id, socialEvent.Name, socialEvent.Date);
    }
}
