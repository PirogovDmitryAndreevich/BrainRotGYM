using UnityEngine;

[RequireComponent (typeof(ArmsViewController), typeof(LegsViewController), typeof(TorsoViewController))]
public class CharacterViewController : MonoBehaviour
{
    private LegsViewController _legsViewer;
    private ArmsViewController _armViewer;
    private TorsoViewController _torsoViewer;

    private void Awake()
    {
        _torsoViewer = GetComponent<TorsoViewController>();
        _legsViewer = GetComponent<LegsViewController>();
        _armViewer = GetComponent<ArmsViewController>();

        if (_legsViewer == null) Debug.LogError("LegsViewController не найден!");
        if (_armViewer == null) Debug.LogError("ArmsViewController не найден!");
    }

    public void UpdateLvlView(int lvl)
    {
        if (_torsoViewer != null)
            _torsoViewer.UpdateSprites(lvl);

        if (_armViewer != null)
            _armViewer.UpdateSprites(lvl);

        if (_legsViewer != null)
            _legsViewer.UpdateSprites(lvl);
    }
}
