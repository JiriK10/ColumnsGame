using ColumnsGame.Enums;

namespace ColumnsGame.Models;

public interface IHighScores
{
    /// <summary>
    /// List of highest scores
    /// </summary>
    public List<HighScore> Scores { get; }

    /// <summary>
    /// Load highscores from LocalStorage
    /// </summary>
    public Task Load();

    /// <summary>
    /// Stores highscores to LocalStorage
    /// </summary>
    public Task Save();

    /// <summary>
    /// Returns if given score is highscore
    /// </summary>
    /// <param name="score">Game score</param>
    public bool IsHighScore(int score);

    /// <summary>
    /// Adds score into highscores or returns false if score is not highscore
    /// </summary>
    /// <param name="score">Game score</param>
    /// <param name="player">Player's name</param>
    public void AddScore(int score, string player);
}
