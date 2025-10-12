[System.Serializable]
public class CharacterProgressData
{
    public CharactersEnum CharacterID;
        
    public int Balk = 0;
    public int Bench = 0;
    public int HorizontalBars = 0;
    public int Foots = 0;
    public int LvlBalk = 1;
    public int LvlBench = 1;
    public int LvlHorizontalBars = 1;
    public int LvlFoots = 1;

    public CharacterProgressData() { }

    public CharacterProgressData(CharactersEnum id)
    {
        CharacterID = id;
    }
}