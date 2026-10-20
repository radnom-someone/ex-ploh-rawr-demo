using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    private CharacterController playerController;
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure your player has the Player tag.");
            return;
        }

        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;

        TpPlayerToSpawn();
    }

    public void TpPlayerToSpawn()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<CharacterController>();
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure your player has the Player tag.");
            return;
        }
        playerController.enabled = false;

        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;

        playerController.enabled = true;
        Debug.Log("Player teleported at PlayerSpawnPoint.");
    }
}
