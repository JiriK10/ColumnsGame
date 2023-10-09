using ColumnsGame.Enums;

namespace ColumnsGame.Models;

public interface IGameArea
{
    /// <summary>
    /// Matrix of game area tiles
    /// </summary>
    public GameAreaTile[,] Tiles { get; }

    /// <summary>
    /// Initialize game area with empty tiles and generate some gem tiles at bottom of game area
    /// </summary>
    public void InitTiles();

    /// <summary>
    /// Put vertical column of tiles into game area
    /// </summary>
    /// <param name="position">Position of first tile from top<</param>
    /// <param name="tiles">Tiles column that starts from top tile</param>
    /// <returns>All tiles are in game area</returns>
    public bool SetColumn(GameAreaPosition position, params GameAreaTile[] tiles);

    /// <summary>
    /// Return start position at top of game area where starts column of tiles that is controlled by player
    /// </summary>
    public GameAreaPosition GetColumnStartPosition();

    /// <summary>
    /// Returns if given position is empty
    /// </summary>
    /// <param name="position">Position</param>
    public bool IsEmpty(GameAreaPosition position);

    /// <summary>
    /// Evaluates game area - removes aligned tiles and transform them to score
    /// </summary>
    /// <returns>Score from aligned tile</returns>
    public int Evaluate();

    /// <summary>
    /// Shakes down tiles on game area
    /// </summary>
    /// <returns>True if any tile moves during shake down</returns>
    public bool ShakeDown();
}
