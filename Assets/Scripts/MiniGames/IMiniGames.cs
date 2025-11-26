using System.Collections.Generic;

public interface IMiniGames 
{
    public MiniGamesType Type { get; }
    public int MaxCharacters { get; }
    void StartGame(List <CharacterProgressData> characters);
}
