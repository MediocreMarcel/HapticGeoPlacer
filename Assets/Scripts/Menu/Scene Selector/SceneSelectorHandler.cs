using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectorHandler : MonoBehaviour
{
    public void OnButtonStudyPressed()
    {
        SceneManager.LoadScene("Study-HapticButtonSurvey");
    }

    public void OnBuildStudyPressed()
    {
        SceneManager.LoadScene("BuildStudyPreview");
    }

    public void OnBuildStudyStartPressed()
    {
        SceneManager.LoadScene("BuildStudy");
    }

    public void OnFreeModePressed()
    {
        SceneManager.LoadScene("GeoPlacer");
    }

    public void OnMainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
