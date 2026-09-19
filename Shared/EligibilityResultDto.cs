namespace Shared;

public class EligibilityResultDto
{
    public bool IsEligible { get; set; }
    public string Reason { get; set; } = string.Empty;
}