using ColumnsGame.Constants;
using ColumnsGame.Enums;
using System.Timers;

namespace ColumnsGame.Models;

public class ColumnFallManager : IColumnFallManager
{
    private readonly IColumnGenerator _columnGenerator;
    private readonly IGameArea _gameArea;

    public ColumnFallManager(IColumnGenerator columnGenerator, IGameArea gameArea)
    {
        _columnGenerator = columnGenerator;
        _gameArea = gameArea;
    }


    public GameAreaPosition ColumnPosition { get; private set; } = new GameAreaPosition();

    private GameAreaPosition ColumnEndPosition { get { return ColumnPosition.ShiftPosition(0, GameParameters.PlayerColumnSize - 1); } }

    public GameAreaTile[] CurrentColumn { get; private set; } = Array.Empty<GameAreaTile>();

    public GameAreaTile[] NextColumn { get; private set; } = Array.Empty<GameAreaTile>();

    public event EventHandler OnFall;
    public event EventHandler OnFall​Completed;

    private System.Timers.Timer fallTimer { get; set; }


    public void Start(int speed)
    {
        ColumnPosition = _gameArea.GetColumnStartPosition();
        CurrentColumn = NextColumn.Length > 0 ? NextColumn : GenerateColumn();
        NextColumn = GenerateColumn();

        StartTimer(speed);
    }

    /// <summary>
    /// Generates new random column
    /// </summary>
    private GameAreaTile[] GenerateColumn() => _columnGenerator.GenerateColumn(GameParameters.PlayerColumnSize, GameParameters.ShortestScoredLine - 1, true);

    /// <summary>
    /// Starts fall timer
    /// </summary>
    private void StartTimer(int speed)
    {
        fallTimer = new System.Timers.Timer(speed);
        fallTimer.AutoReset = true;
        fallTimer.Elapsed += TimerTick;
        fallTimer.Start();
    }

    /// <summary>
    /// Fall timer tick handler
    /// </summary>
    private void TimerTick(object? sender, ElapsedEventArgs e)
    {
        if (IsFallEnd())
        {
            fallTimer.Stop();
            OnFall​Completed(this, null);
        }
        else
        {
            MoveCurrentColumn(MoveDirection.Down);
            OnFall(this, null);
        }
    }

    /// <summary>
    /// Returns if fall should be ended
    /// </summary>
    private bool IsFallEnd()
    {
        return !_gameArea.IsEmpty(ColumnEndPosition.ShiftPosition(0, 1));
    }

    public void RotateCurrentColumn()
    {
        CurrentColumn = CurrentColumn.SkipLast(1).Prepend(CurrentColumn.Last()).ToArray();
    }

    public void MoveCurrentColumn(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Left:
                if (_gameArea.IsEmpty(ColumnEndPosition.ShiftPosition(-1, 0)))
                {
                    ColumnPosition.X = ColumnPosition.X - 1;
                }
                break;
            case MoveDirection.Right:
                if (_gameArea.IsEmpty(ColumnEndPosition.ShiftPosition(1, 0)))
                {
                    ColumnPosition.X = ColumnPosition.X + 1;
                }
                break;
            case MoveDirection.Down:
                if (_gameArea.IsEmpty(ColumnEndPosition.ShiftPosition(0, 1)))
                {
                    ColumnPosition.Y = ColumnPosition.Y + 1;
                    fallTimer.Stop();
                    fallTimer.Start();
                }
                else
                {
                    TimerTick(null, null);
                }
                break;
        }
    }
}
