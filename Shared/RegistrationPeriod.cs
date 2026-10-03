using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;

/// <summary>
/// The semester registration window, stored as a record in the database so it can
/// be changed without touching code or rebuilding. The most recently added record
/// (highest <see cref="Id"/>) is the one in effect, so opening a new window means
/// inserting a new row - or just editing the dates on the existing one.
/// </summary>
public class RegistrationPeriod
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    /// <summary>Optional label for the record, e.g. "Fall 2026/2027".</summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    [NotMapped]
    public bool IsOpen => IsOpenAt(DateTime.Now);

    public bool IsOpenAt(DateTime when) => when >= StartDate && when <= EndDate;

    /// <summary>e.g. "Aug 25, 2026 - Sep 25, 2026"</summary>
    [NotMapped]
    public string DisplayRange => $"{StartDate:MMM d, yyyy} - {EndDate:MMM d, yyyy}";

    [NotMapped]
    public string ClosedMessage =>
        $"Registration is currently closed. The registration window is {DisplayRange}.";

    /// <summary>Message to show when no window has been configured in the database at all.</summary>
    public const string NotConfiguredMessage =
        "Registration is currently closed. No registration window has been configured.";
}
