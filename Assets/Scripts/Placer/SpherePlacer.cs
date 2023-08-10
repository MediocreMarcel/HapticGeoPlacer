using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpherePlacer : Placeable
{
    public GameObject SpherePrefab;
    public ButtonStudyHandler buttonStudyHandler;
    public override void GeneratePreview(Vector3 centerPosition, Vector3 scale)
    {
        if (this.previewObject == null)
        {
            this.previewObject = Instantiate(SpherePrefab, centerPosition, Quaternion.identity).gameObject;
        }

        this.SetPreviewMaterial(this.previewObject.GetComponent<Renderer>().material);

        this.previewObject.transform.position = centerPosition;
        this.previewObject.transform.localScale = scale;

        if (SceneManager.GetActiveScene().name == "Study-HapticButtonSurvey")
        {
            previewObject.GetComponent<ColorChanger>().ButtonStudyHandler = buttonStudyHandler;
        }
    }

}