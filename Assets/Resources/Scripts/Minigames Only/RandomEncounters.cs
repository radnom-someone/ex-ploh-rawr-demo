using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RandomEncounters : MonoBehaviour
{
    [Header("Guaranteed encounter on area enter")]
    public bool encounterOnStart;

    [Header("Distance until next encounter roll")]
    [Tooltip("Distance to travel until next attempt to roll encounter")]
    [SerializeField] private float minDistance = 500f;
    [SerializeField] private float maxDistance = 2000f;

    [Header("Encounter Event")]
    [Tooltip("This method gets called every roll.")]
    public UnityEvent onEncounterInvoked = new UnityEvent();

    private GameObject player;
    private GameObject battleground;
    private Collider bgCollider;
    private PlayerInteract piScript;


    private float distanceUntilNextAttempt;
    private float distanceTravelled;
    private float distanceMoved;
    private int encounterRoll;
    private int targetAmount;

    private Vector3 lastPosition;
    private bool isMoving;
    private bool playerInside;


    //DEBUG UI
    //private TextMeshProUGUI totalDistanceText;

    
    void Start()
    {
        distanceTravelled = 0f;
        playerInside = false;

        // First encounter can be guaranteed.
        if (encounterOnStart)
        {
            targetAmount = 10;
            distanceUntilNextAttempt = 1f;
        }
        else
        {
            targetAmount = 1;
        }

        ResetEncounterDistance();

        /*
        if (totalDistanceText != null)
        {
            UpdateDistanceText();
            totalDistanceText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("RandomEncounters: TotalDistance UI is not assigned. (encounter calculations will still work)");
        }
        */
    }
    public void EncounterEvent()
    {
        Debug.Log("[Random Encounter] This is a placeholder. You have rolled an encounter");

    }


    /*private void UpdateDistanceText()
    {
        if (totalDistanceText == null)
        {
            return;
        }
        totalDistanceText.text = "Total Distance Travelled: " + distanceTravelled.ToString("F1") +" / " +distanceUntilNextAttempt.ToString("F1");
    }*/
    private void ResetEncounterDistance()
    {
        float minimum = Mathf.Max(0.01f, minDistance);
        float maximum = Mathf.Max(minimum, maxDistance);

        distanceUntilNextAttempt = Random.Range(minimum, maximum);
    }
    private void AttemptEncounter()
    {
        distanceTravelled = 0f;

        ResetEncounterDistance();

        // Roll a number from 1 to 10.
        encounterRoll = Random.Range(1, 11);

        if (targetAmount >= encounterRoll)
        {
            Debug.Log(
                "[Random Encounter] Encounter rolled! " +
                targetAmount + " / " + encounterRoll
            );

            targetAmount = 1;

            onEncounterInvoked.Invoke();
        }
        else
        {
            Debug.Log(
                "[Random Encounter] No encounter rolled. " +
                "Increasing chance... " +
                targetAmount + " / " + encounterRoll
            );

            targetAmount += 1;

            if (targetAmount > 10)
            {
                targetAmount = 10;
            }
        }

        //UpdateDistanceText();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInside = false;

        /*if (totalDistanceText != null)
        {
            totalDistanceText.gameObject.SetActive(false);
        }*/
    } //When player stops touching battleground, hide the distance travelled text.

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (!playerInside)
        {
            playerInside = true;
            lastPosition = other.transform.position;
            return;
        }
        Vector3 currentPosition = other.transform.position;


        float distanceMoved = Vector3.Distance(lastPosition,currentPosition);

        lastPosition = currentPosition;

        if (distanceMoved <= 0f)
        {
            return;
        }

        distanceTravelled += distanceMoved;

        //UpdateDistanceText();

        if (distanceTravelled >= distanceUntilNextAttempt)
        {
            AttemptEncounter();
            //Debug.Log("Attempt encounter");
        }
    }


}

