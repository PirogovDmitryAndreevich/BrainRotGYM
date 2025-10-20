using UnityEngine;

[System.Serializable]
public class CharacterProgressData
{
    public CharactersEnum CharacterID;
        
    public int Balk = 0;
    public int Bench = 0;
    public int HorizontalBars = 0;
    public int Foots = 0;

    public int Level = 1;

    public int LvlBalk = 1;
    public int BalksUpdatePoint = 0;

    public int LvlBench = 1;
    public int BenchUpdatePoint = 0;

    public int LvlHorizontalBars = 1;
    public int HorizontalBarsUpdatePoint = 0;

    public int LvlFoots = 1;
    public int FootsUpdatePoint = 0;

    public bool IsLevelRequiresUpdate = false;
    public bool IsStatsRequiresUpdate = false;

    public void UpdateLevel()
    {
        Level = Mathf.Min(LvlBalk, LvlBench, LvlHorizontalBars, LvlFoots);
    }
        

    public CharacterProgressData() { }

    public CharacterProgressData(CharactersEnum id)
    {
        CharacterID = id;
    }
}