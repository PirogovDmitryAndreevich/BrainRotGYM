using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Basic Info")]
    public string Name;
    public CharactersEnum CharacterID;
    public Color MainColor;
    public Color SecondaryColor;

    [Header("Sprites")]
    public Sprite FaceSprite;
    public Sprite DecorationsSprite;
    public Sprite FootSprite;
    public Sprite HeadSprite;
    public Sprite Icon;
    public Sprite Shorts;

    [Header("Unlock list")]
    [SerializeReference]
    public List<UnlockCondition> unlockConditions = new();

    public CharacterData() {}

    public CharacterData (CharacterData other)
    {
        other.Name = this.Name;
        other.CharacterID = this.CharacterID;
        other.MainColor = this.MainColor;
        other.SecondaryColor = this.SecondaryColor;
        other.FaceSprite = this.FaceSprite;
        other.DecorationsSprite = this.DecorationsSprite;
        other.FootSprite = this.FootSprite;
        other.HeadSprite = this.HeadSprite;
        other.Icon = this.Icon;
        other.Shorts = this.Shorts;
    }
}