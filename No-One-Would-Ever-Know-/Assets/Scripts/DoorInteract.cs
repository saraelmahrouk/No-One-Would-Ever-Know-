using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteract : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 3f;
    public int sceneIndexToLoad = 0;

    [Header("UI References")]
    public GameObject promptText;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip clip;

    public static bool paperRead = false;

    private Transform player;

    void Start()
    {
        GameObject baldMan = GameObject.Find("BaldMan");

        if (baldMan != null)
        {
            player = baldMan.transform;
        }

        if (promptText != null)
        {
            promptText.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool playerNearby = distance <= interactRange;

        // If paper is not read, always hide door prompt
        if (!paperRead)
        {
            if (promptText != null)
                promptText.SetActive(false);

            return;
        }

        // Show door prompt only after paper is read and player is near door
        if (promptText != null)
            promptText.SetActive(playerNearby);

        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        if (clip != null && source != null)
        {
            source.PlayOneShot(clip);
        }

        SceneManager.LoadSceneAsync(sceneIndexToLoad);
    }
}