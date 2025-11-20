namespace Models.DTO;

/// <summary>
/// DTO данных исполнителя.
/// </summary>
public class ExecutorDataDTO
{
    public required string Name { get; set; }

    public required string Grade { get; set; }

    public required string Phone { get; set; }

    public required string Email { get; set; }
}