using GpsUtil.Location;
using Microsoft.AspNetCore.Mvc;
using TourGuide.Models.DTOs;
using TourGuide.Services.Interfaces;
using TourGuide.Users;
using TripPricer;

namespace TourGuide.Controllers;

[ApiController]
[Route("[controller]")]
public class TourGuideController : ControllerBase
{
    private readonly ITourGuideService _tourGuideService;

    public TourGuideController(ITourGuideService tourGuideService)
    {
        _tourGuideService = tourGuideService;
    }

    [HttpGet("getLocation")]
    public ActionResult<VisitedLocation> GetLocation([FromQuery] string userName)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
        {
            return new JsonResult(null);
        }
        
        var user = GetUser(userName) ?? new User(Guid.NewGuid(), userName.Trim(), "000", "jon@tourGuide.com");

        var location = _tourGuideService.GetUserLocation(user);
        return Ok(location);
    }

    [HttpGet("getNearbyAttractions")]
    public JsonResult GetNearbyAttractions([FromQuery] string userName)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
        {
            return new JsonResult(null);
        }
        
        var user = GetUser(userName) ?? new User(Guid.NewGuid(), userName.Trim(), "000", "jon@tourGuide.com");

        var visitedLocation = _tourGuideService.GetUserLocation(user);
        var attractions = _tourGuideService.GetNearByAttractions(visitedLocation);
        var json = attractions.Select(attraction => new AttractionDto()
        {
            AttractionName = attraction.AttractionName,
            AttractionLocation = attraction,
            UserLocation = visitedLocation.Location,
            Distance = _tourGuideService.GetDistance(attraction, visitedLocation.Location)
        }).ToArray();
            
        return new JsonResult(json);
    }

    [HttpGet("getRewards")]
    public ActionResult<List<UserReward>> GetRewards([FromQuery] string userName)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
        {
            return BadRequest();
        }
        
        var user = GetUser(userName) ?? new User(Guid.NewGuid(), userName.Trim(), "000", "jon@tourGuide.com");

        var rewards = _tourGuideService.GetUserRewards(user);
        return Ok(rewards);
    }

    [HttpGet("getTripDeals")]
    public ActionResult<List<Provider>> GetTripDeals([FromQuery] string userName)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
        {
            return BadRequest();
        }
        
        var user = GetUser(userName) ?? new User(Guid.NewGuid(), userName.Trim(), "000", "jon@tourGuide.com");

        var deals = _tourGuideService.GetTripDeals(user);
        return Ok(deals);
    }

    private User? GetUser(string userName)
    {
        return _tourGuideService.GetUser(userName);
    }
}
