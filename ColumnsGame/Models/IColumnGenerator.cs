using ColumnsGame.Enums;

namespace ColumnsGame.Models;

public interface IColumnGenerator
{
    /// <summary>
    /// Generates new random column of gem tiles
    /// </summary>
    /// <param name="size">Size of column</param>
    /// <param name="maxSame">Longest allowed sequence of same tiles (including jokers)</param>
    /// <param name="joker">Allow joker tile</param>
    public GameAreaTile[] GenerateColumn(int size, int maxSame, bool joker);

    /// <summary>
    /// Generates new random column of gem tiles with random size within limits
    /// </summary>
    /// <param name="minSize">Minimum size of column</param>
    /// <param name="maxSize">Maximum size of column</param>
    /// <param name="maxSame">Longest allowed sequence of same tiles (including jokers)</param>
    /// <param name="joker">Allow joker tile</param>
    public GameAreaTile[] GenerateColumn(int minSize, int maxSize, int maxSame, bool joker);
}
