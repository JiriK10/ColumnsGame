using ColumnsGame.Constants;
using ColumnsGame.Enums;

namespace ColumnsGame.Models;

public class GameArea : IGameArea
{
    private readonly IColumnGenerator _columnGenerator;

    public GameArea(IColumnGenerator columnGenerator)
    {
        _columnGenerator = columnGenerator;
    }


    public GameAreaTile[,] Tiles { get; private set; } = {};


    public void InitTiles()
    {
        Tiles = new GameAreaTile[GameParameters.GameAreaWidth, GameParameters.GameAreaHeight];

        // Generate initial gem tiles
        int sequence = 0;
        for (int x = 0; x < GameParameters.GameAreaWidth; x++)
        {
            GameAreaTile[] column = sequence >= 2 ? Array.Empty<GameAreaTile>() : _columnGenerator.GenerateColumn(0, 2, GameParameters.ShortestScoredLine - 1, false);
            if (column.Length > 0)
            {
                sequence++;
                GameAreaPosition position = new GameAreaPosition()
                {
                    X = x, 
                    Y = GameParameters.GameAreaHeight - column.Length
                };
                SetColumn(position, column);
            }
            else
            {
                sequence = 0;
            }
        }
    }

    public bool SetColumn(GameAreaPosition position, params GameAreaTile[] tiles)
    {
        for (int i = tiles.Length - 1; i >= 0; i--)
        {
            if (!IsValidPosition(position.X, position.Y + i))
            {
                return false;
            }
            Tiles[position.X, position.Y + i] = tiles[i];
        }
        return true;
    }

    public GameAreaPosition GetColumnStartPosition()
    {
        return new GameAreaPosition()
        {
            X = (int)Math.Floor(GameParameters.GameAreaWidth / (double)2) - 1,
            Y = -2
        };
    }

    /// <summary>
    /// Tests that given position is inside game area
    /// </summary>
    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && y >= 0 && x < GameParameters.GameAreaWidth && y < GameParameters.GameAreaHeight;
    }

    public bool IsEmpty(GameAreaPosition position)
    {
        return IsValidPosition(position.X, position.Y) && Tiles[position.X, position.Y] == GameAreaTile.Empty;
    }

    #region Evaluate

    private class EvaluationLine
    {
        /// <summary>
        /// Start of line - most bottom/left tile on game area
        /// </summary>
        public GameAreaPosition Start { get; set; }

        /// <summary>
        /// Length of line in tiles
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Offset for calculating next tile on line
        /// </summary>
        public GameAreaPosition NextTileOffset { get; set; }
    }

    private List<GameAreaPosition> _evaluationDirections = new List<GameAreaPosition>()
    {
        new GameAreaPosition() { X = 1, Y = 0 },    // 🡺 horizontal
        new GameAreaPosition() { X = 0, Y = -1 },   // 🡹 vertical
        new GameAreaPosition() { X = 1, Y = -1 },   // 🡽 diagonal
        new GameAreaPosition() { X = -1, Y = -1 },  // 🡼 diagonal
    };

    public int Evaluate()
    {
        // Find longest line
        EvaluationLine? longestLine = null;
        foreach (var evaluationDirection in _evaluationDirections)
        {
            EvaluationLine? longestDirectionLine = EvaluateDirection(evaluationDirection);
            if (longestDirectionLine != null && (longestLine?.Length ?? 0) < longestDirectionLine.Length)
            {
                longestLine = longestDirectionLine;
            }
        }

        // Replace longest line with empty tiles
        if (longestLine != null)
        {
            for (int i = 0; i < longestLine.Length; i++)
            {
                Tiles[longestLine.Start.X + i * longestLine.NextTileOffset.X, longestLine.Start.Y + i * longestLine.NextTileOffset.Y] = GameAreaTile.Empty;
            }
        }

        return longestLine?.Length ?? 0;
    }

    /// <summary>
    /// Finds longest line for given direction
    /// </summary>
    /// <param name="nextTileOffset">Offset for calculating next tile on line</param>
    private EvaluationLine? EvaluateDirection(GameAreaPosition nextTileOffset)
    {
        int loopXStart = 0;
        int loopXEnd = GameParameters.GameAreaWidth;
        int loopXStep = 1;
        if (nextTileOffset.X == 1)
        {
            loopXEnd -= GameParameters.ShortestScoredLine - 1;
        }
        else if (nextTileOffset.X == -1)
        {
            loopXStart = GameParameters.GameAreaWidth - 1;
            loopXEnd = GameParameters.ShortestScoredLine - 2;
            loopXStep = -1;
        }

        int loopYEnd = nextTileOffset.Y == -1 ? GameParameters.ShortestScoredLine - 1 : 0;

        EvaluationLine? longestLine = null;
        for (int y = GameParameters.GameAreaHeight - 1; y >= loopYEnd; y--)
        {
            for (int x = loopXStart; x != loopXEnd; x += loopXStep)
            {
                EvaluationLine? longestLineFromPosition = EvaluateDirectionLine(nextTileOffset, x, y);
                if (longestLineFromPosition != null && (longestLine?.Length ?? 0) < longestLineFromPosition.Length)
                {
                    longestLine = longestLineFromPosition;
                }
            }
        }
        return longestLine;
    }

    /// <summary>
    /// Finds longest line for given direction and start position
    /// </summary>
    /// <param name="nextTileOffset">Offset for calculating next tile on line</param>
    /// <param name="x">Start position X</param>
    /// <param name="y">Start position Y</param>
    private EvaluationLine? EvaluateDirectionLine(GameAreaPosition nextTileOffset, int x, int y)
    {
        int linelength = 0;
        int currentX = x;
        int currentY = y;
        GameAreaTile lineTile = GameAreaTile.Empty;

        while (IsValidPosition(currentX, currentY))
        {
            GameAreaTile currentTile = Tiles[currentX, currentY];
            if (currentTile == GameAreaTile.Empty)
            {
                break;
            }

            if (lineTile == GameAreaTile.Empty || lineTile == GameAreaTile.Joker)
            {
                lineTile = currentTile;
            }
            else if (lineTile != currentTile && currentTile != GameAreaTile.Joker)
            {
                break;
            }

            linelength++;

            currentX += nextTileOffset.X;
            currentY += nextTileOffset.Y;
        }

        if (linelength >= GameParameters.ShortestScoredLine)
        {
            return new EvaluationLine()
            {
                Start = new GameAreaPosition() { X = x, Y = y },
                NextTileOffset = nextTileOffset,
                Length = linelength
            };
        }
        return null;
    }

    #endregion Evaluate

    public bool ShakeDown()
    {
        int moves = 0;
        int?[] firstEmptyTile = new int?[GameParameters.GameAreaWidth];
        for (int y = GameParameters.GameAreaHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < GameParameters.GameAreaWidth; x++)
            {
                if (Tiles[x, y] == GameAreaTile.Empty)
                {
                    if (firstEmptyTile[x] == null)
                    {
                        firstEmptyTile[x] = y;
                    }
                }
                else if (firstEmptyTile[x] > y)
                {
                    Tiles[x, firstEmptyTile[x]!.Value] = Tiles[x, y];
                    Tiles[x, y] = GameAreaTile.Empty;
                    firstEmptyTile[x]--;
                    moves++;
                }
            }
        }
        return moves > 0;
    }
}
