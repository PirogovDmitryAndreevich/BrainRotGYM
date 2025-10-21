public interface ISelectableCharacter
{
    CharactersEnum CharacterID { get; }
    void Select();
    void Deselect();
}
