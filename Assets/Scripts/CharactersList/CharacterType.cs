using UnityEngine;

[System.Serializable]
public class CharacterType
{
    public string Name;
    public CharactersEnum CharacterID;
    public Color MainColor;
    public Color SecondaryColor;
    public Sprite FaceSprite;
    public Sprite DecorationsSprite;
    public Sprite FootSprite;

    [HideInInspector] public int Balk = 0;
    [HideInInspector] public int Bench = 0;
    [HideInInspector] public int HorizontalBars = 0;
    [HideInInspector] public int Foots = 0;

    [HideInInspector] public int LvlBalk = 1;
    [HideInInspector] public int LvlBench = 1;
    [HideInInspector] public int LvlHorizontalBars = 1;
    [HideInInspector] public int LvlFoots = 1;

    public CharacterType(CharacterType other)
    {
        if (other == null) return;

        //  опируем базовые данные
        Name = other.Name;
        CharacterID = other.CharacterID;
        MainColor = other.MainColor;
        SecondaryColor = other.SecondaryColor;
        FaceSprite = other.FaceSprite;
        DecorationsSprite = other.DecorationsSprite;
        FootSprite = other.FootSprite;

        //  опируем статистику (дл€ новых персонажей она будет начальной)
        Balk = other.Balk;
        Bench = other.Bench;
        HorizontalBars = other.HorizontalBars;
        Foots = other.Foots;
        LvlBalk = other.LvlBalk;
        LvlBench = other.LvlBench;
        LvlHorizontalBars = other.LvlHorizontalBars;
        LvlFoots = other.LvlFoots;
    }
}
