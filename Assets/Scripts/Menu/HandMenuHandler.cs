using UnityEngine;

public enum Menus
{
    Main = 0,
    Place = 1,
    Edit = 2,
    Color = 3
}

public class HandMenuHandler : MonoBehaviour
{

    [SerializeField] private GameObject MainMenu;
    private MainMenuHandler MainMenuHandler;
    [SerializeField] private GameObject PlaceMenu;
    private PlaceMenuHandler PlaceMenuHandler;
    [SerializeField] private GameObject EditMenu;
    private EditMenuHandler EditMenuHandler;
    [SerializeField] private GameObject ColorMenu;
    private ColorMenuHandler ColorMenuHandler;

    private Menus CurrentMenu = Menus.Main;

    private void Start()
    {
        //Get All Menu Handlers
        this.MainMenuHandler = MainMenu.GetComponent<MainMenuHandler>();
        this.PlaceMenuHandler = PlaceMenu.GetComponent<PlaceMenuHandler>();
        this.EditMenuHandler = EditMenu.GetComponent<EditMenuHandler>();
        this.ColorMenuHandler = ColorMenu.GetComponent<ColorMenuHandler>();
    }

    /// <summary>
    /// Opens a specific Menu and closes other menus
    /// </summary>
    /// <param name="MenuIntRepresented">Int representation of the Menus Enum</param>
    public void OpenMenu(int MenuIntRepresented)
    {
        Menus Menu = (Menus)MenuIntRepresented;
        this.CurrentMenu = Menu;
        switch (Menu)
        {
            case Menus.Main:
                this.MainMenu.SetActive(true);
                this.PlaceMenuHandler.CloseMenu();
                this.EditMenuHandler.CloseMenu();
                this.ColorMenuHandler.CloseMenu();
                break;
            case Menus.Place:
                this.PlaceMenu.SetActive(true);
                this.MainMenuHandler.CloseMenu();
                this.EditMenuHandler.CloseMenu();
                this.ColorMenuHandler.CloseMenu();
                break;
            case Menus.Edit:
                this.EditMenu.SetActive(true);
                this.MainMenuHandler.CloseMenu();
                this.PlaceMenuHandler.CloseMenu();
                this.ColorMenuHandler.CloseMenu();
                break;
            case Menus.Color:
                this.ColorMenu.SetActive(true);
                this.MainMenuHandler.CloseMenu();
                this.PlaceMenuHandler.CloseMenu();
                this.EditMenuHandler.CloseMenu();
                break;
        }
    }

    /// <summary>
    /// Closes all menus
    /// </summary>
    public void CloseAllMenus()
    {
        this.MainMenuHandler.CloseMenu();
        this.PlaceMenuHandler.CloseMenu();
        this.EditMenuHandler.CloseMenu();
        this.ColorMenuHandler.CloseMenu();
    }

    /// <summary>
    /// Hides all Menus, keeps all toggle selections
    /// </summary>
    public void HideAllMenus()
    {
        this.MainMenu.SetActive(false);
        this.PlaceMenu.SetActive(false);
        this.EditMenu.SetActive(false);
        this.ColorMenu.SetActive(false);
    }

    /// <summary>
    /// Opens a the last opened menu
    /// </summary>
    public void OpenCurrentMenu()
    {
        switch (this.CurrentMenu)
        {
            case Menus.Main:
                this.MainMenu.SetActive(true);
                break;
            case Menus.Place:
                this.PlaceMenu.SetActive(true);
                break;
            case Menus.Edit:
                this.EditMenu.SetActive(true);
                break;
            case Menus.Color:
                this.ColorMenu.SetActive(true);
                break;
        }
    }
}
