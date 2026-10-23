using UnityEngine;
using UnityEngine.Events;

public class Chapter2Manager : MonoBehaviour
{
    [SerializeField] private GameObject childrenParent;
    [SerializeField] private UnityEvent onAllChildrenInactive = new UnityEvent();

    private bool allChildrenEventInvoked;

    private void Update()
    {
        if (childrenParent == null)
            return;

        if (childrenParent.transform.childCount == 0)
            return;

        foreach (Transform child in childrenParent.transform)
        {
            if (child.gameObject.activeSelf)
            {
                allChildrenEventInvoked = false;
                return;
            }
        }

        if (!allChildrenEventInvoked)
        {
            allChildrenEventInvoked = true;
            onAllChildrenInactive.Invoke();
        }
    }
}