namespace Models.DTO;

/// <summary>
/// Локальное состояние установленной выкладки отдела.
/// </summary>
public class LocalUpdateState
{
    public string InstalledReleaseId { get; set; } = string.Empty;

    public string InstalledMajorVersion { get; set; } = string.Empty;

    public string InstalledDisplayName { get; set; } = string.Empty;

    public string PreviousReleaseId { get; set; } = string.Empty;

    public string PreviousMajorVersion { get; set; } = string.Empty;

    public string PreviousDisplayName { get; set; } = string.Empty;
}
