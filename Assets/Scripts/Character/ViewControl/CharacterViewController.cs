using UnityEngine;

[RequireComponent (typeof(ArmsViewController), typeof(LegsViewController), typeof(PressViewController))]
[RequireComponent (typeof(TorsViewController), typeof(PressViewController), typeof(FaceViewController))]
[RequireComponent (typeof(HeadViewController), typeof(DecorationViewController), typeof(FootViewController))]
[RequireComponent (typeof(ShortsViewController))]
public class CharacterViewController : MonoBehaviour
{
    private LegsViewController _legsViewer;
    private ArmsViewController _armViewer;
    private PressViewController _pressViewer;
    private TorsViewController _torsViewer;
    private FaceViewController _faceViewer;
    private HeadViewController _headViewer;
    private DecorationViewController _decorationViewer;
    private FootViewController _footViewer;
    private ShortsViewController _shortsViewer;

    private void Awake()
    {
        _torsViewer = GetComponent<TorsViewController>();        
        _pressViewer = GetComponent<PressViewController>();
        _legsViewer = GetComponent<LegsViewController>();
        _armViewer = GetComponent<ArmsViewController>();
        _faceViewer = GetComponent<FaceViewController>();
        _headViewer = GetComponent<HeadViewController>();
        _decorationViewer = GetComponent<DecorationViewController>();
        _footViewer = GetComponent<FootViewController>();
        _shortsViewer = GetComponent<ShortsViewController>();
    }

    public void UpdateCharacterView(CharacterData character)
    {
        _faceViewer.SetSprite(character.FaceSprite);
        _decorationViewer.SetSprite(character.DecorationsSprite);
        _footViewer.SetSprite(character.FootSprite);
        _headViewer.SetSprite(character.HeadSprite);
        _shortsViewer.SetSprite(character.Shorts);

        SetMainColor(character.MainColor);
        _pressViewer.SetColor(character.SecondaryColor);

    }

    public void UpdateLvlView(CharacterProgressData character)
    {
        if (_pressViewer != null)
            _pressViewer.UpdateSprites(character.LvlBench);

        if (_pressViewer != null)
            _torsViewer.UpdateSprites(character.LvlHorizontalBars);

        if (_armViewer != null)
            _armViewer.UpdateSprites(character.LvlBalk);

        if (_legsViewer != null)
            _legsViewer.UpdateSprites(character.LvlFoots);
    }

    private void SetMainColor(Color color)
    {       
        _torsViewer.SetColor(color);
        _legsViewer.SetColor(color);
        _armViewer.SetColor(color);
    }
}
