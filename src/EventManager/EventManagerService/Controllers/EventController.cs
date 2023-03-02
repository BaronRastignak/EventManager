using System.Net;
using EventManager.Domain.Events;
using EventManagerService.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventManagerService.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly ISocialEventRepository _eventRepository;

    public EventController(ISocialEventRepository repository)
    {
        _eventRepository = repository;
    }

    // GET: api/<EventController>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SocialEventDTO>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<SocialEventDTO>>> GetEventsAsync()
    {
        //TODO: Return only events where this authenticated user is a participant
        var events = (await _eventRepository.GetEventsAsync())
            .Select(SocialEventDTO.FromSocialEvent);
        return Ok(events);
    }

    // GET api/<EventController>/5
    [HttpGet("{eventId}")]
    [ProducesResponseType(typeof(SocialEventDTO), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult> GetEventAsync(int eventId)
    {
        if (eventId < 0)
            return BadRequest();

        var foundEvent = await _eventRepository.GetAsync(eventId);
        return foundEvent is not null
            ? Ok(SocialEventDTO.FromSocialEvent(foundEvent))
            : NotFound();
    }

    // POST api/<EventController>
    [HttpPost]
    public async Task<ActionResult<SocialEventDTO>> CreateEvent([FromBody] SocialEventDTO socialEvent)
    {
        var newEvent = _eventRepository.Add(new SocialEvent(socialEvent.Name, socialEvent.Date));
        await _eventRepository.UnitOfWork.SaveEntitiesAsync();
        return SocialEventDTO.FromSocialEvent(newEvent);
    }

    // DELETE api/<EventController>/5
    [HttpDelete("{eventId}")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult> DeleteEntityAsync(int eventId)
    {
        if (eventId < 0)
            return BadRequest();

        var foundEvent = await _eventRepository.GetAsync(eventId);
        if (foundEvent is null)
            return NotFound();

        _eventRepository.Delete(foundEvent);
        await _eventRepository.UnitOfWork.SaveEntitiesAsync();
        return Ok();
    }
}
