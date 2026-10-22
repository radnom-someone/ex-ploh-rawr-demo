using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject npcBun;
    [SerializeField] private GameObject npcBuilder;

    public bool cutscenesFinished = false;
    private void Start()
    {
        //cutscenesFinished = false;
    }

    public void FinishCutscene()
    {
        cutscenesFinished = true;
    }

    public void HideBun()
    {
        if (npcBun != null)
        {
            HideGameObject(npcBun);
        }
    }
    public void HideBuilder()
    {
        if (npcBun != null)
        {
            HideGameObject(npcBuilder);
        }
    }

    public void HideGameObject(GameObject go)
    {
        if (go != null)
        {
            go.SetActive(false);
            Debug.Log("Hidden: " +  go.name);
        }
    }

    public void MakeNPCsVisible()
    {
        
    }

}
