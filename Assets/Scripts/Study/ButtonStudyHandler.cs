using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public enum StudySteps
{
    HAND_TO_START, CLICK_PLACE, CLICK_QUBOID, CLICK_BACK_FROM_PLACE, CLICK_EDIT, CLICK_COLOR, CLICK_GREEN, CLICK_BACK_FROM_COLOR, CLICK_RESIZE, CLICK_BACK_FROM_EDIT, CLICK_PLACE_2, CLICK_SPHERE, CLICK_BACK_FROM_PLACE_2, CLICK_EDIT_2, CLICK_COLOR_2, CLICK_YELLOW
}

public enum Buttons
{
    PLACE, EDIT, DELETE, PLACE_BACK, PLACE_QUBOID, PLACE_PYRAMID, PLACE_SPHERE, PLACE_CYLINDER, EDIT_SIZE, EDIT_COLOR, COLOR_RED, COLOR_GREEN, COLOR_BLUE, COLOR_YELLOW, COLOR_BACK, EDIT_BACK
}

public class ButtonStudyHandler : MonoBehaviour
{
    [SerializeField] private GameObject TextOverlay;
    [SerializeField] private GameObject StartCube;
    [SerializeField] private GameObject DonePlate;

    private TextMeshPro TextOverlayMash;
    private StudySteps currentStep = StudySteps.HAND_TO_START;

    private DateTime? interactionStart;
    private DateTime? interactionEnd;
    private List<string> missclicks = new List<string>();
    private string currentFilename = "";

    public void onStartCubeTouched()
    {
        switch (currentStep)
        {
            case StudySteps.HAND_TO_START:
                this.currentFilename = DateTime.Now.ToString().Replace(" ", "-").Replace(":", "-").Replace(".", "-") + "-button-study.csv";
                Debug.Log(this.currentFilename);
                File.Create(currentFilename).Close();
                using (StreamWriter w = File.AppendText(this.currentFilename))
                {
                    w.WriteLine($"task;start-time;end-time;interaction-time;missclicks");
                }
                this.ShowText("Click on 'Place'");
                this.currentStep = StudySteps.CLICK_PLACE;
                break;
            case StudySteps.CLICK_PLACE:
                this.ShowText("Click on 'Quboid' and place a Quboid");
                this.currentStep = StudySteps.CLICK_QUBOID;
                break;
            case StudySteps.CLICK_QUBOID:
                this.ShowText("Click on 'Back'");
                this.currentStep = StudySteps.CLICK_BACK_FROM_PLACE;
                break;
            case StudySteps.CLICK_BACK_FROM_PLACE:
                this.ShowText("Click on 'Edit'");
                this.currentStep = StudySteps.CLICK_EDIT;
                break;
            case StudySteps.CLICK_EDIT:
                this.ShowText("Click on 'Color'");
                this.currentStep = StudySteps.CLICK_COLOR;
                break;
            case StudySteps.CLICK_COLOR:
                this.ShowText("Click on 'Green' and\n change the color of the Quboid");
                this.currentStep = StudySteps.CLICK_GREEN;
                break;
            case StudySteps.CLICK_GREEN:
                this.ShowText("Click on 'Back'");
                this.currentStep = StudySteps.CLICK_BACK_FROM_COLOR;
                break;
            case StudySteps.CLICK_BACK_FROM_COLOR:
                this.ShowText("Click on 'Size'");
                this.currentStep = StudySteps.CLICK_RESIZE;
                break;
            case StudySteps.CLICK_RESIZE:
                this.ShowText("Click on 'Back'");
                this.currentStep = StudySteps.CLICK_BACK_FROM_EDIT;
                break;
            case StudySteps.CLICK_BACK_FROM_EDIT:
                this.ShowText("Click on 'Place'");
                this.currentStep = StudySteps.CLICK_PLACE_2;
                break;
            case StudySteps.CLICK_PLACE_2:
                this.ShowText("Click on 'Sphere' and place it");
                this.currentStep = StudySteps.CLICK_SPHERE;
                break;
            case StudySteps.CLICK_SPHERE:
                this.ShowText("Click on 'Back'");
                this.currentStep = StudySteps.CLICK_BACK_FROM_PLACE_2;
                break;
            case StudySteps.CLICK_BACK_FROM_PLACE_2:
                this.ShowText("Click on 'Edit'");
                this.currentStep = StudySteps.CLICK_EDIT_2;
                break;
            case StudySteps.CLICK_EDIT_2:
                this.ShowText("Click on 'Color'");
                this.currentStep = StudySteps.CLICK_COLOR_2;
                break;
            case StudySteps.CLICK_COLOR_2:
                this.ShowText("Click on 'Yellow'\nand change the color of the Sphere");
                this.currentStep = StudySteps.CLICK_YELLOW;
                break;
            case StudySteps.CLICK_YELLOW:
                this.DonePlate.SetActive(true);
                break;
        }
    }

    public void OnStartCubeReleased()
    {
        this.TextOverlay.SetActive(false);
        this.StartCube.SetActive(false);
        this.interactionStart = DateTime.Now;
    }

    public void OnButtonPressed(int buttonAsInt)
    {
        Buttons button = (Buttons)buttonAsInt;
        switch (button)
        {
            case Buttons.PLACE:
                if (this.currentStep.Equals(StudySteps.CLICK_PLACE))
                {
                    this.TaskCompleated("PLACE");
                }
                else if (this.currentStep.Equals(StudySteps.CLICK_PLACE_2))
                {
                    this.TaskCompleated("PLACE2");
                }
                else
                {
                    missclicks.Add("PLACE");
                }
                break;
            case Buttons.EDIT:
                if (this.currentStep.Equals(StudySteps.CLICK_EDIT))
                {
                    this.TaskCompleated("EDIT");
                }
                else if (this.currentStep.Equals(StudySteps.CLICK_EDIT_2))
                {
                    this.TaskCompleated("EDIT2");
                }
                else
                {
                    missclicks.Add("EDIT");
                }
                break;
            case Buttons.DELETE:
                missclicks.Add("DELETE");
                break;
            case Buttons.PLACE_BACK:
                if (this.currentStep.Equals(StudySteps.CLICK_BACK_FROM_PLACE))
                {
                    this.TaskCompleated("PLACE_BACK");
                }
                else if (this.currentStep.Equals(StudySteps.CLICK_BACK_FROM_PLACE_2))
                {
                    this.TaskCompleated("PLACE_BACK_2");
                }
                else
                {
                    missclicks.Add("PLACE_BACK");
                }
                break;
            case Buttons.PLACE_QUBOID:
                if (this.currentStep.Equals(StudySteps.CLICK_QUBOID))
                {
                    this.TaskCompleated("CLICK_QUBOID", false);
                }
                else
                {
                    missclicks.Add("PLACE_QUBOID");
                }
                break;
            case Buttons.PLACE_SPHERE:
                if (this.currentStep.Equals(StudySteps.CLICK_SPHERE))
                {
                    this.TaskCompleated("CLICK_SPHERE", false);
                }
                else
                {
                    missclicks.Add("PLACE_QUBOID");
                }
                break;
            case Buttons.PLACE_CYLINDER:
                missclicks.Add("PLACE_CYLINDER");
                break;
            case Buttons.PLACE_PYRAMID:
                missclicks.Add("PLACE_PYRAMID");
                break;
            case Buttons.EDIT_SIZE:
                if (this.currentStep.Equals(StudySteps.CLICK_RESIZE))
                {
                    this.TaskCompleated("EDIT_SIZE", false);
                }
                else
                {
                    missclicks.Add("EDIT_SIZE");
                }
                break;
            case Buttons.EDIT_COLOR:
                if (this.currentStep.Equals(StudySteps.CLICK_COLOR))
                {
                    this.TaskCompleated("CLICK_COLOR");
                }
                else if (this.currentStep.Equals(StudySteps.CLICK_COLOR_2))
                {
                    this.TaskCompleated("CLICK_COLOR_2");
                }
                else
                {
                    missclicks.Add("CLICK_COLOR");
                }
                break;
            case Buttons.COLOR_GREEN:
                if (this.currentStep.Equals(StudySteps.CLICK_GREEN))
                {
                    this.TaskCompleated("COLOR_GREEN", false);
                }
                else
                {
                    missclicks.Add("COLOR_GREEN");
                }
                break;
            case Buttons.COLOR_YELLOW:
                if (this.currentStep.Equals(StudySteps.CLICK_YELLOW))
                {
                    this.TaskCompleated("COLOR_YELLOW", false);
                }
                else
                {
                    missclicks.Add("COLOR_YELLOW");
                }
                break;
            case Buttons.COLOR_RED:
                missclicks.Add("COLOR_RED");
                break;
            case Buttons.COLOR_BLUE:
                missclicks.Add("COLOR_BLUE");
                break;
            case Buttons.COLOR_BACK:
                if (this.currentStep.Equals(StudySteps.CLICK_BACK_FROM_COLOR))
                {
                    this.TaskCompleated("COLOR_BACK");
                }
                else
                {
                    missclicks.Add("COLOR_BACK");
                }
                break;
            case Buttons.EDIT_BACK:
                if (this.currentStep.Equals(StudySteps.CLICK_BACK_FROM_EDIT))
                {
                    this.TaskCompleated("EDIT_BACK");
                }
                else
                {
                    missclicks.Add("EDIT_BACK");
                }
                break;
            default:
                break;
        }
    }

    public void OnResizeDone()
    {
        if (this.currentStep.Equals(StudySteps.CLICK_RESIZE))
        {
            this.ShowStartCube();
        }
    }

    public void OnColorDone(int color)
    {
        //Color 2 = Green
        //Color 3 = Yellow
        if (this.currentStep.Equals(StudySteps.CLICK_GREEN) && color == 2)
        {
            this.ShowStartCube();
        } else if (this.currentStep.Equals(StudySteps.CLICK_YELLOW) && color == 3)
        {
            this.ShowStartCube();
        }
    }

    void ShowText(string text)
    {
        this.TextOverlayMash.text = text;
        this.TextOverlay.SetActive(true);
    }

    void ShowStartCube()
    {
        this.StartCube.SetActive(true);
    }

    void resetStats()
    {
        this.interactionStart = null;
        this.interactionEnd = null;
        this.missclicks.Clear();
        this.currentFilename = "";
    }

    void TaskCompleated(string task, bool showCube = true)
    {
        this.interactionEnd = DateTime.Now;
        using (StreamWriter w = File.AppendText(this.currentFilename))
        {
            w.WriteLine($"{task};{this.interactionStart.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")};{this.interactionEnd.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")};{new TimeSpan(interactionEnd.Value.Ticks - interactionStart.Value.Ticks).TotalMilliseconds};{string.Join(",", this.missclicks.ToArray())}");
        }
        this.missclicks.Clear();
        if (showCube)
        {
            this.ShowStartCube();
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        this.TextOverlayMash = TextOverlay.transform.GetChild(0).GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
