using ColumnsGame.Enums;

namespace ColumnsGame.Models;

public interface IColumnFallManager
{
    /// <summary>
    /// Position on game area of column of tiles that is controlled by player
    /// </summary>
    public GameAreaPosition ColumnPosition { get; }

    /// <summary>
    /// Column of tiles that is controlled by player
    /// </summary>
    public GameAreaTile[] CurrentColumn { get; }

    /// <summary>
    /// Column of tiles that is next in line after current column
    /// </summary>
    public GameAreaTile[] NextColumn { get; }

    /// <summary>
    /// Event that is raised after each fall
    /// </summary>
    public event EventHandler OnFall;

    /// <summary>
    /// Event that is raised when fall is completed
    /// </summary>
    public event EventHandler OnFall​Completed;


    /// <summary>
    /// Start fall of column of tiles that is controlled by player
    /// </summary>
    /// <param name="speed">Speed of falling</param>
    public void Start(int speed);

    /// <summary>
    /// Start fall for next column
    /// </summary>
    public void StartNextColumn();

    /// <summary>
    /// Rotates tiles in current column
    /// </summary>
    public void RotateCurrentColumn();

    /// <summary>
    /// Move current column (controlled by player) left/right/down
    /// </summary>
    public void MoveCurrentColumn(MoveDirection direction);
}
