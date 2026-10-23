using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NPC_Minigames : NPC
{
    [Header("Confirmation UI")]
    [SerializeField] private GameObject confirmationUI;
    [SerializeField] private Confirmation confirmationScript;
    [SerializeField] private Button buttonYes; // of confirmation ui
    [SerializeField] private string confirmationDesc = "You will be playing a minigame where you repair lines of codes. If you fail, you can try again.";

    [Header("Quest")]
    [SerializeField] private string npcName = "Hammer";

    [Header("Scene")]
    [SerializeField] private string sceneName = "RepairSyntaxMinigame";

    void Awake()
    {

        if (confirmationScript == null)
        {
            confirmationScript = confirmationUI.GetComponent<Confirmation>();
        }

        if (buttonYes == null)
        {
            Transform buttonTransform = confirmationUI.transform.Find("Button Yes");
            if (buttonTransform != null)
            {
                buttonYes = buttonTransform.GetComponent<Button>();
            }
        }
    }

    private void OnEnable()
    {
        if (buttonYes != null)
        {
            buttonYes.onClick.AddListener(LoadMinigameScene);
        }
    }
    private void OnDisable()
    {
        if (buttonYes != null)
        {
            buttonYes.onClick.RemoveListener(LoadMinigameScene);
        }
    }
    new void Start()
    {
        // does not exists, maybe you deleted it?
        //confirmationUI = GameObject.Find("ConfirmationUI");
        //confirmationScript = confirmationUI.GetComponent<Confirmation>();
    }

    new void Update()
    {
        base.Update();
    }

    // This function is called when a player interacted this NPC
    public override void Interact() // Changed it to protected override for Quest system to work properly
    {
        QuestEvents.talkToEvent.Invoke(npcName); // Invoke the talkToEvent with the NPC's name

        Canvas confirmCanvas = confirmationUI.GetComponent<Canvas>();
        confirmationScript.EditDescription(confirmationDesc);

        Time.timeScale = 0f; // pause

        // Hide player controls
        //PlayerInteract.UI_JOYSTICK.gameObject.SetActive(false);
        //PlayerInteract.UI_INTERACT.gameObject.SetActive(false);

        confirmCanvas.enabled = true;
        confirmationUI.transform.Find("Button Yes").GetComponent<Button>().onClick.AddListener(LoadMinigameScene);
    }

    public void InteractAfterScene() // Changed it to protected override for Quest system to work properly
    {
        QuestEvents.talkToEvent.Invoke(npcName); // Invoke the talkToEvent with the NPC's name

        Canvas confirmCanvas = confirmationUI.GetComponent<Canvas>();
        confirmationScript.EditDescription(confirmationDesc);

        Time.timeScale = 0f; // pause

        // Hide player controls
        //PlayerInteract.UI_JOYSTICK.gameObject.SetActive(false);
        //PlayerInteract.UI_INTERACT.gameObject.SetActive(false);

        confirmCanvas.enabled = true;
        confirmationUI.transform.Find("Button Yes").GetComponent<Button>().onClick.AddListener(LoadMinigameScene);
    }


    public void LoadMinigameScene()
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("NPC_Minigame: Scene name is empty.", this);
            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);  
    }

}
