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
    public Sprite HeadSprite;
    public Sprite Icon;
    public Sprite Shorts;
    
    [HideInInspector] public int Balk = 0;
    [HideInInspector] public int Bench = 0;
    [HideInInspector] public int HorizontalBars = 0;
    [HideInInspector] public int Foots = 0;

    [HideInInspector] public int LvlBalk = 1;
    [HideInInspector] public int LvlBench = 1;
    [HideInInspector] public int LvlHorizontalBars = 1;
    [HideInInspector] public int LvlFoots = 1;

    public CharacterType(CharacterType character)
    {
        this.Name = character.Name;
        this.CharacterID = character.CharacterID;
        this.MainColor = character.MainColor;
        this.SecondaryColor = character.SecondaryColor;
        this.FaceSprite = character.FaceSprite;
        this.DecorationsSprite = character.DecorationsSprite;
        this.FootSprite = character.FootSprite;
        this.HeadSprite = character.HeadSprite;
        this.Icon = character.Icon;
        this.Shorts = character.Shorts;

        this.Balk = 0;
        this.Bench = 0;
        this.HorizontalBars = 0;
        this.Foots = 0;

        this.LvlBalk = 1;
        this.LvlBench = 1;
        this.LvlHorizontalBars = 1;
        this.LvlFoots = 1;
    }

    public CharacterType() { }
}
