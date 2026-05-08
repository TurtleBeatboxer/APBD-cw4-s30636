using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    // GET: api/reservations?date=2026-05-10&status=confirmed&roomId=2
    [HttpGet]
    public IActionResult GetReservations([FromQuery] DateOnly? date, [FromQuery] string? status, [FromQuery] int? roomId)
    {
        var query = StaticData.Reservations.AsQueryable();

        if (date.HasValue)
            query = query.Where(r => r.Date == date.Value);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

        if (roomId.HasValue)
            query = query.Where(r => r.RoomId == roomId.Value);

        return Ok(query.ToList());
    }

    // GET: api/reservations/{id}
    [HttpGet("{id:int}")]
    public IActionResult GetReservationById(int id)
    {
        var reservation = StaticData.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null)
        {
            return NotFound($"Rezerwacja o ID {id} nie została znaleziona.");
        }
        return Ok(reservation);
    }
    
    [HttpPost]
    public IActionResult CreateReservation([FromBody] Reservation reservation)
    {
        var room = StaticData.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room == null)
        {
            return NotFound($"Sala o ID {reservation.RoomId} nie istnieje.");
        }
        if (!room.IsActive)
        {
            return BadRequest("Nie można zarezerwować nieaktywnej sali.");
        }
        bool isConflict = StaticData.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.Status != "cancelled" &&
            (
                (reservation.StartTime >= r.StartTime && reservation.StartTime < r.EndTime) || 
                (reservation.EndTime > r.StartTime && reservation.EndTime <= r.EndTime) ||    
                (reservation.StartTime <= r.StartTime && reservation.EndTime >= r.EndTime)    
            )
        );

        if (isConflict)
        {
            return Conflict("Występuje konflikt czasowy z inną rezerwacją w tej sali.");
        }

        reservation.Id = StaticData.Reservations.Count > 0 ? StaticData.Reservations.Max(r => r.Id) + 1 : 1;
        StaticData.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
    }

    // PUT: api/reservations/{id}
    [HttpPut("{id:int}")]
    public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
    {
        var existingReservation = StaticData.Reservations.FirstOrDefault(r => r.Id == id);
        if (existingReservation == null)
        {
            return NotFound($"Rezerwacja o ID {id} nie istnieje.");
        }
        bool isConflict = StaticData.Reservations.Any(r =>
            r.Id != id && 
            r.RoomId == updatedReservation.RoomId &&
            r.Date == updatedReservation.Date &&
            r.Status != "cancelled" &&
            (
                (updatedReservation.StartTime >= r.StartTime && updatedReservation.StartTime < r.EndTime) ||
                (updatedReservation.EndTime > r.StartTime && updatedReservation.EndTime <= r.EndTime) ||
                (updatedReservation.StartTime <= r.StartTime && updatedReservation.EndTime >= r.EndTime)
            )
        );

        if (isConflict)
        {
            return Conflict("Aktualizacja powoduje konflikt czasowy z inną rezerwacją.");
        }

        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.OrganizerName = updatedReservation.OrganizerName;
        existingReservation.Topic = updatedReservation.Topic;
        existingReservation.Date = updatedReservation.Date;
        existingReservation.StartTime = updatedReservation.StartTime;
        existingReservation.EndTime = updatedReservation.EndTime;
        existingReservation.Status = updatedReservation.Status;

        return Ok(existingReservation);
    }

    // DELETE: api/reservations/{id}
    [HttpDelete("{id:int}")]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = StaticData.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null)
        {
            return NotFound($"Rezerwacja o ID {id} nie została znaleziona.");
        }

        StaticData.Reservations.Remove(reservation);
        return NoContent(); 
    }
}