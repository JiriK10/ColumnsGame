namespace ColumnsGame.Models;

 public record GameAreaPosition
{
    /// <summary>
    /// Horizontal position
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Vertical position
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Returns new position shifted by given offsets
    /// </summary>
    /// <param name="xOffset">Horizontal offset</param>
    /// <param name="yOffset">Vertical offset</param>
    public GameAreaPosition ShiftPosition(int xOffset, int yOffset)
    {
        return new GameAreaPosition()
        {
            X = X + xOffset,
            Y = Y + yOffset
        };
    }
}
