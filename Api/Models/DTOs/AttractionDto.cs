using GpsUtil.Location;

namespace TourGuide.Models.DTOs;

public class AttractionDto
{
    public required string AttractionName { get; set; }
    public required Locations AttractionLocation { get; set; }
    public required Locations UserLocation { get; set; }
    public required double Distance { get; set; }
}