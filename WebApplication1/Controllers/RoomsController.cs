using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    // GET: api/rooms?minCapacity=20&hasProjector=true&activeOnly=true
    [HttpGet]
    public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
        var query = StaticData.Rooms.AsQueryable();

        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            query = query.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly.HasValue && activeOnly.Value)
            query = query.Where(r => r.IsActive);

        return Ok(query.ToList());
    }

    // GET: api/rooms/{id}
    [HttpGet("{id:int}")]
    public IActionResult GetRoomById(int id)
    {
        var room = StaticData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null)
        {
            return NotFound($"Sala o ID {id} nie została znaleziona.");
        }
        return Ok(room);
    }

    // GET: api/rooms/building/{buildingCode}
    [HttpGet("building/{buildingCode}")]
    public IActionResult GetRoomsByBuilding(string buildingCode)
    {
        var roomsInBuilding = StaticData.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(roomsInBuilding);
    }

    // POST: api/rooms
    [HttpPost]
    public IActionResult CreateRoom([FromBody] Room room)
    {
        room.Id = StaticData.Rooms.Count > 0 ? StaticData.Rooms.Max(r => r.Id) + 1 : 1;
        StaticData.Rooms.Add(room);
        return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
    }

    // PUT: api/rooms/{id}
    [HttpPut("{id:int}")]
    public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
    {
        var existingRoom = StaticData.Rooms.FirstOrDefault(r => r.Id == id);
        if (existingRoom == null)
        {
            return NotFound($"Sala o ID {id} nie istnieje.");
        }
        existingRoom.Name = updatedRoom.Name;
        existingRoom.BuildingCode = updatedRoom.BuildingCode;
        existingRoom.Floor = updatedRoom.Floor;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.HasProjector = updatedRoom.HasProjector;
        existingRoom.IsActive = updatedRoom.IsActive;
        return Ok(existingRoom);
    }

    // DELETE: api/rooms/{id}
    [HttpDelete("{id:int}")]
    public IActionResult DeleteRoom(int id)
    {
        var room = StaticData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null)
        {
            return NotFound($"Sala o ID {id} nie została znaleziona.");
        }
        bool hasReservations = StaticData.Reservations.Any(res => res.RoomId == id);
        if (hasReservations)
        {
            return Conflict("Nie można usunąć sali, ponieważ istnieją powiązane z nią rezerwacje.");
        }
        StaticData.Rooms.Remove(room);
        return NoContent(); // 204 No Content
    }
}