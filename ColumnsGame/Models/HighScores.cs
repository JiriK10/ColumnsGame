using ColumnsGame.Constants;

namespace ColumnsGame.Models;

 public class HighScores : IHighScores
{
    public List<HighScore> Scores { get; private set; } = new List<HighScore>();

    public void Load()
    {
        // TODO
    }

    public void Save()
    {
        // TODO
    }

    public bool AddScore(int score, string player)
    {
        if (Scores.Where(highScore => highScore.Score >= score).Count() >= GameParameters.HighScoresListLength)
        {
            return false;
        }

        Scores.Add(new HighScore()
        {
            Score = score,
            Player = player
        });
        Scores = Scores
            .OrderByDescending(score => score.Score)
            .ThenBy(score => score.TimeStamp)
            .Take(GameParameters.HighScoresListLength)
            .ToList();
        return true;
    }
}
