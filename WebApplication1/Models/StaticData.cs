namespace WebApplication1.Models;

public static class StaticData
{
    public static List<Room> Rooms { get; set; } = new List<Room>
    {
        new Room { Id = 1, Name = "Lab 11", BuildingCode = "K", Floor = 1, Capacity = 10, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Lab 24", BuildingCode = "L", Floor = 2, Capacity = 12, HasProjector = true, IsActive = true },
        new Room { Id = 3, Name = "Sa 101", BuildingCode = "M", Floor = 3, Capacity = 13, HasProjector = true, IsActive = true },
        new Room { Id = 4, Name = "Magazyn", BuildingCode = "N", Floor = 4, Capacity = 14, HasProjector = false, IsActive = false },
        new Room { Id = 5, Name = "Sala 301", BuildingCode = "O", Floor = 5, Capacity = 15, HasProjector = false, IsActive = true }
    };

    public static List<Reservation> Reservations { get; set; } = new List<Reservation>
    {
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Jan Kowalski", Topic = "C#", Date = new DateOnly(2026, 5, 10), StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(10, 0, 0), Status = "confirmed" },
        new Reservation { Id = 2, RoomId = 2, OrganizerName = "Anna Nowak", Topic = "REST API", Date = new DateOnly(2026, 5, 10), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 30, 0), Status = "planned" },
        new Reservation { Id = 3, RoomId = 3, OrganizerName = "Firma XYZ", Topic = "Konferencja IT", Date = new DateOnly(2026, 5, 15), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(16, 0, 0), Status = "confirmed" },
        new Reservation { Id = 4, RoomId = 1, OrganizerName = "Jan Kowalski", Topic = "C#", Date = new DateOnly(2026, 5, 10), StartTime = new TimeSpan(10, 30, 0), EndTime = new TimeSpan(12, 0, 0), Status = "confirmed" }
    };
}