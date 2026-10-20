using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NPC_Tower : NPC
{
    [Header("Confirmation UI")]
    [SerializeField] private GameObject confirmationUI;
    [SerializeField] private Confirmation confirmationScript;
    [SerializeField] private Button buttonYes; // of confirmation ui

    [Header("Quest")]
    [SerializeField] private string npcName = "Tower";

    [Header("Scene")]
    [SerializeField] private string sceneName = "TowerDefense";

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
            buttonYes.onClick.AddListener(LoadTowerDefenseScene);
        }
    }
    private void OnDisable()
    {
        if (buttonYes != null)
        {
            buttonYes.onClick.RemoveListener(LoadTowerDefenseScene);
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
        confirmationScript.EditDescription("You are heading to a tower defense minigame. If you fail, you can try again.");

        Time.timeScale = 0f; // pause

        // Hide player controls
        PlayerInteract.UI_JOYSTICK.gameObject.SetActive(false);
        PlayerInteract.UI_INTERACT.gameObject.SetActive(false);

        confirmCanvas.enabled = true;
        confirmationUI.transform.Find("Button Yes").GetComponent<Button>().onClick.AddListener(LoadTowerDefenseScene);
    }

    public void LoadTowerDefenseScene()
    {
        if (string.IsNullOrWhiteSpace("TowerDefense"))
        {
            Debug.LogError("NPC_Tower: Scene name is empty.", this);
            return;
        }
        PlayerInteract.UI_JOYSTICK.gameObject.SetActive(true);
        PlayerInteract.UI_INTERACT.gameObject.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("TowerDefense");
    }

}
