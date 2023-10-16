namespace ColumnsGame.Models;

 public record HighScore
{
    /// <summary>
    /// Player's score
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Player's name
    /// </summary>
    public string Player { get; set; } = string.Empty;

    /// <summary>
    /// TimeStamp of game
    /// </summary>
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}
