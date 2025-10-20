using System;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    private SceneElementBase _currentScene;
    private SceneElementBase _targetScene;

    public Action<Identificate> OnSwitchIsComplete;

    public void ShowScene(SceneElementBase targetScene, SceneElementBase currentScene)
    {
        _currentScene = currentScene;
        _targetScene = targetScene;

        if (currentScene != null)
        {
            TrainingAreaController.Instance.OnAnimationIsComplete -= Show;
            TrainingAreaController.Instance.OnAnimationIsComplete += Show;
            Hide();
            return;
        }

        Show();        
    }

    private void Show()
    {
        Debug.Log($"Show {_targetScene.Identifier}");

        TrainingAreaController.Instance.OnAnimationIsComplete -= Show;
        _targetScene.gameObject.SetActive(true);
        TrainingAreaController.Instance.SetTrainingArea(TrainingArea.Show);
        OnSwitchIsComplete?.Invoke(_targetScene.Identifier);

        _currentScene = null;
        _targetScene = null;
    }

    private void Hide()
    {
        Debug.Log($"Hide {_currentScene.Identifier}");
        _currentScene.gameObject.SetActive(false);
        TrainingAreaController.Instance.SetTrainingArea(TrainingArea.Hide);
    }

}
