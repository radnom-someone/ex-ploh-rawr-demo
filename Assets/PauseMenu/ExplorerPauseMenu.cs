using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ExplorerPauseMenu : MonoBehaviour
{
    [Header("Generated UI")]
    public Canvas pauseButtonCanvas;
    public Canvas menuCanvas;
    public Canvas settingsCanvas;
    [Header("Your existing flashcards")]
    public ExplorerFlashcardPanel flashcardPanel;
    [Header("Saving")]
    [Tooltip("Connect existing synchronous save methods here. PlayerPrefs is flushed afterward.")]
    public UnityEvent onSaveBeforeQuit = new UnityEvent();
    public string mainMenuScene = "MainMenu";
    [Header("Pause Behavior")]
    [SerializeField] private bool freezeTimeInMenus = true;
    private bool frozeTime;
    private bool paused;
    private bool viewingFlashcards;
    private bool quitting;
    private float previousTimeScale = 1f;

    private void Start()
    {
        menuCanvas.enabled = false;
        settingsCanvas.enabled = false;
        FindFlashcards();
        pauseButtonCanvas.enabled = Time.timeScale > 0f;
    }
    private void FindFlashcards()
    {
        if (flashcardPanel != null) return;
        ExplorerFlashcardPanel[] panels = FindObjectsByType<ExplorerFlashcardPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (ExplorerFlashcardPanel panel in panels)
        {
            if (panel.gameObject.scene == gameObject.scene) { flashcardPanel = panel; break; }
        }
    }
    private void Update()
    {
        if (!paused)
        {
            pauseButtonCanvas.enabled = Time.timeScale > 0f && !OBJ_CMD.IsQuizOngoing;
            return;
        }
        // Existing flashcard Close() disables its Canvas. Return to pause menu.
        if (viewingFlashcards && (flashcardPanel == null || flashcardPanel.panelCanvas == null || !flashcardPanel.panelCanvas.enabled))
        {
            viewingFlashcards = false;
            menuCanvas.enabled = true;
        }
    }
    public void Pause()
    {
        if (OBJ_CMD.IsQuizOngoing)
        {
            return;
        }
        if (paused || quitting || Time.timeScale <= 0f) return;
        previousTimeScale = Time.timeScale;
        paused = true;
        frozeTime = freezeTimeInMenus;
        if (frozeTime) Time.timeScale = 0f;
        pauseButtonCanvas.enabled = false;
        settingsCanvas.enabled = false;
        menuCanvas.enabled = true;
    }
    public void Resume()
    {
        if (!paused || quitting) return;
        if (viewingFlashcards && flashcardPanel != null) flashcardPanel.Close();
        viewingFlashcards = false;
        menuCanvas.enabled = false;
        settingsCanvas.enabled = false;
        paused = false;
        if (frozeTime) Time.timeScale = previousTimeScale;
        frozeTime = false;
        pauseButtonCanvas.enabled = Time.timeScale > 0f;
    }
    public void OpenSettings()
    {
        menuCanvas.enabled = false;
        settingsCanvas.enabled = true;
    }
    public void BackToPause()
    {
        settingsCanvas.enabled = false;
        menuCanvas.enabled = true;
    }
    public void OpenFlashcards()
    {
        FindFlashcards();
        if (flashcardPanel == null || flashcardPanel.panelCanvas == null)
        {
            Debug.LogWarning("Pause menu: assign your FlashcardsCanvas's ExplorerFlashcardPanel in the Inspector.");
            return;
        }
        // The existing flashcard Close button is wired to Close(). Update handles return.
        flashcardPanel.Open();
        viewingFlashcards = true;
        menuCanvas.enabled = false;
        settingsCanvas.enabled = false;
    }
    public void QuitToMenu()
    {
        if (quitting) return;
        if (!Application.CanStreamedLevelBeLoaded(mainMenuScene))
        {
            Debug.LogError("Add " + mainMenuScene + " to your build scene list before using Quit to Menu.");
            return;
        }
        quitting = true;
        try
        {
            onSaveBeforeQuit.Invoke();
            PlayerPrefs.Save();
        }
        catch (System.Exception error)
        {
            quitting = false;
            Debug.LogException(error);
            return;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
    private void OnDestroy()
    {
        if (paused && !quitting && frozeTime) Time.timeScale = previousTimeScale;
    }
}
