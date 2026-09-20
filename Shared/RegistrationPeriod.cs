namespace Shared;


public static class RegistrationPeriod
{
    public static readonly DateTime StartDate = new(2026, 8, 25, 0, 0, 0);
    public static readonly DateTime EndDate = new(2026, 9, 25, 23, 59, 59);

    public static bool IsOpen => IsOpenAt(DateTime.Now);

    public static bool IsOpenAt(DateTime when) => when >= StartDate && when <= EndDate;

    public static string DisplayRange => $"{StartDate:MMM d, yyyy} - {EndDate:MMM d, yyyy}";

    public static string ClosedMessage => $"Registration is currently closed. It opens {DisplayRange}.";
}
