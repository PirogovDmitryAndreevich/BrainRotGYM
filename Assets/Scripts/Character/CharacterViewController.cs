using UnityEngine;

[RequireComponent (typeof(ArmsViewController), typeof(LegsViewController), typeof(PressViewController))]
[RequireComponent (typeof(TorsViewController), typeof(PressViewController))]
public class CharacterViewController : MonoBehaviour
{
    private LegsViewController _legsViewer;
    private ArmsViewController _armViewer;
    private PressViewController _pressViewer;
    private TorsViewController _torsViewer;

    private void Awake()
    {
        _torsViewer = GetComponent<TorsViewController>();        
        _pressViewer = GetComponent<PressViewController>();
        _legsViewer = GetComponent<LegsViewController>();
        _armViewer = GetComponent<ArmsViewController>();

        if (_legsViewer == null) Debug.LogError("LegsViewController не найден!");
        if (_armViewer == null) Debug.LogError("ArmsViewController не найден!");
    }

    public void UpdateLvlView(int lvl)
    {
        if (_pressViewer != null)
            _pressViewer.UpdateSprites(lvl);

        if (_pressViewer != null)
            _torsViewer.UpdateSprites(lvl);

        if (_armViewer != null)
            _armViewer.UpdateSprites(lvl);

        if (_legsViewer != null)
            _legsViewer.UpdateSprites(lvl);
    }
}
