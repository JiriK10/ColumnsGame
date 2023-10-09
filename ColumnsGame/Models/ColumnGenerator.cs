using ColumnsGame.Constants;
using ColumnsGame.Enums;

namespace ColumnsGame.Models;

public class ColumnGenerator : IColumnGenerator
{
    private Random _gemRandomizer = new Random();

    /// <summary>
    /// Generates random gem tile
    /// </summary>
    private GameAreaTile GenerateGemTile(bool joker)
    {
        if (joker && _gemRandomizer.NextDouble() <= GameParameters.JokerProbability)
        {
            return GameAreaTile.Joker;
        }
        return (GameAreaTile)_gemRandomizer.Next(GameParameters.FirstBasicGemTileType, GameParameters.LastBasicGemTileType + 1);
    }

    public GameAreaTile[] GenerateColumn(int size, int maxSame, bool joker)
    {
        int sequence = 0;
        GameAreaTile sequenceTile = GameAreaTile.Empty;
        GameAreaTile[] column = new GameAreaTile[size];
        for (int i = 0; i < size; i++)
        {
            while (column[i] == GameAreaTile.Empty)
            {
                GameAreaTile randomTile = GenerateGemTile(joker);
                bool randomIsSame = randomTile == sequenceTile || randomTile == GameAreaTile.Joker;
                if (!randomIsSame || sequence < maxSame)
                {
                    column[i] = randomTile;
                    sequence = randomIsSame ? sequence + 1 : 1;
                    if (!randomIsSame || sequenceTile == GameAreaTile.Empty || sequenceTile == GameAreaTile.Joker)
                    {
                        sequenceTile = randomTile;
                    }
                }
            }
        }
        return column;
    }

    public GameAreaTile[] GenerateColumn(int minSize, int maxSize, int maxSame, bool joker)
    {
        return GenerateColumn(_gemRandomizer.Next(minSize, maxSize + 1), maxSame, joker);
    }
}
