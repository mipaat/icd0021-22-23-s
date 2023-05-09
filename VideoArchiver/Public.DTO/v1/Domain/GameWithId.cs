namespace Public.DTO.v1.Domain;

/// <summary>
/// Data for a Game entity, including its unique identifier
/// </summary>
public class GameWithId : GameWithoutId
{
    /// <summary>
    /// Unique identifier for this Game entity
    /// </summary>
    public Guid Id { get; set; }
}