using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Nazwa organizatora jest wymagana.")]
    public string OrganizerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Temat jest wymagany.")]
    public string Topic { get; set; } = string.Empty;

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    [Required]
    public string Status { get; set; } = "planned";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "Czas zakończenia (EndTime) musi być późniejszy niż czas rozpoczęcia (StartTime).",
                new[] { nameof(EndTime) }
            );
        }
    }
}