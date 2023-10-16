using Blazored.LocalStorage;
using ColumnsGame.Constants;

namespace ColumnsGame.Models;

 public class HighScores : IHighScores
{
    private readonly ILocalStorageService _localStorage;

    public HighScores(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }


    private const string StorageKeyName = "HighScores";

    public List<HighScore> Scores { get; private set; } = new List<HighScore>();

    public async Task Load()
    {
        Scores = await _localStorage.GetItemAsync<List<HighScore>>(StorageKeyName) ?? new List<HighScore>();
    }

    public async Task Save()
    {
        await _localStorage.SetItemAsync(StorageKeyName, Scores);
    }

    public bool IsHighScore(int score)
    {
        return Scores.Where(highScore => highScore.Score >= score).Count() < GameParameters.HighScoresListLength;
    }

    public void AddScore(int score, string player)
    {
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
    }
}
