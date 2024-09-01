using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    private static WindowsManager instance;
    public static WindowsManager Instance { get => instance; }

    [SerializeField] private LevelController levelController;
    [SerializeField] private Camera menuCamera;

    private Transform container;

    public List<IWindowUI> windows = new();

    private IWindowUI activeWindow;
    private List<IWindowUI> ActivePopups = new();

    private void Awake()
    {
        instance = this;

        container = new GameObject("Container").transform;
        container.SetParent(transform);

        if (WindowsConfig.Instance.CreateOnlyBasicWindowsAtStart)
            Initialize();
        else
            InitializeAllWindows();
    }

    private void Start()
    {
        DisableWindowsExcept(WindowType.MainMenu);
        activeWindow = GetWindowByType(WindowType.MainMenu);
    }

    private void InitializeAllWindows()
    {

        foreach (var prefab in WindowsConfig.Instance.Windows[0].WindowPrefabs)
        {
            var window = Instantiate(prefab.RealizeWindow(), container);
            if (window.TryGetComponent<IWindowUI>(out var component))
            {
                windows.Add(component);
                window.SetActive(false);
                if (component.Type == WindowType.MainMenu)
                {
                    window.SetActive(true);
                    activeWindow = component;
                }
            }
            else
                Debug.LogError($"Prefab by index: {WindowsConfig.Instance.Windows[0].WindowPrefabs.IndexOf(prefab)} " +
                    $"dont contain IWindowUI component");
        }
    }

    private void Initialize()
    {
        var BasicWindows = WindowsConfig.Instance.BasicWindows;
        BasicWindows.ForEach(w => CreateWindowByType(w));

        if (BasicWindows.Count == 0)
        {
            CreateWindowByType(WindowType.MainMenu);
            CreateWindowByType(WindowType.LevelChoosePopup);
            CreateWindowByType(WindowType.LevelGoalsPopup);
        }
    }

    public void CreateWindowByType(WindowType type)
    {
        var prefab = WindowsConfig.Instance.Windows[0].WindowPrefabs.Find(w => w.Type == type).RealizeWindow();
        var window = Instantiate(prefab, container);
        windows.Add(window.GetComponent<IWindowUI>());
    }

    private void DestroyWindow(IWindowUI window)
    {
        window.DestroySelf();
    }

    private void DisableWindowsExcept(WindowType type)
        => windows.ForEach(w => w.Window.SetActive(w.Type == type));

    private IWindowUI GetWindowByType(WindowType type)
        => windows.Find(w => w.Type == type);

    public HUD GetHUDUpdate()
    {
        CloseAllWindows();
        var HUD = windows.Find(hud => hud.Type == WindowType.HUD);
        if (HUD == null)
        {
            CreateWindowByType(WindowType.HUD);
            activeWindow = windows[windows.Count - 1];
            return activeWindow.Window.GetComponent<HUD>();

        }
        HUD.Window.SetActive(true);
        return HUD.Window.GetComponent<HUD>();
    }

    public void PreloadLevel(int level)
    {
        levelController.SetLevelConfig(level);
        ActivePopups.Last().Window.GetComponent<LevelGoals>().SetText(level);
    }

    public void StartLevel()
    {
        CloseAllWindows();
        menuCamera.enabled = false;
        levelController.gameObject.SetActive(true);
    }

    public void EndLevel()
    {
        levelController.gameObject.SetActive(false);
        levelController.Reload();
        OpenWindow(WindowType.MainMenu);
    }

    public void OpenWindow(WindowType type)
    {
        var window = windows.Find(w => w.Type == type);
        CloseAllWindows();
        if (window == null)
        {
            CreateWindowByType(type);
            activeWindow = windows[windows.Count - 1];
            return;
        }
        activeWindow = window;
        window.Window.SetActive(true);
    }

    public void OpenPopup(WindowType type)
    {
        var popup = windows.Find(w => w.Type == type);
        if (popup == null)
        {
            CreateWindowByType(type);
            ActivePopups.Add(windows[windows.Count - 1]);
            return;
        }
        popup.Window.SetActive(true);
        ActivePopups.Add(popup);
    }

    public void CloseWindow(IWindowUI window)
    {
        if (WindowsConfig.Instance.DestroyInactiveWindows)
        {
            windows.Remove(window);
            DestroyWindow(window);
        }
        else
            window.Window.SetActive(false);
    }

    public void ClosePopup(IWindowUI popup)
    {
        ActivePopups.Remove(popup);
        if (WindowsConfig.Instance.DestroyInactiveWindows)
        {
            windows.Remove(popup);
            DestroyWindow(popup);
        }
        else
            popup.Window.SetActive(false);
    }

    private void CloseAllWindows()
    {
        for (int i = ActivePopups.Count - 1; i >= 0; i--)
            ClosePopup(ActivePopups[i]);
        CloseWindow(activeWindow);
    }
}

