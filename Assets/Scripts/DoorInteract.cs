using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteract : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 3f;
    public bool readPaper = true;
    public int sceneIndexToLoad = 0;

    [Header("UI References")]
    public GameObject promptText;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip clip;

    public float speed = 2f;
    private bool isMoving = false;
    private Quaternion targetRotation;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    public static bool paperRead;

    private Transform player;
    public bool IsOpen => isOpen;

    private void Awake()
    {

        paperRead = readPaper;

    }
    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, 90f, 0f));

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
        Debug.Log("paperRead: " + paperRead);

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
        if (isMoving) return;
        Debug.Log("OpenDoor called! isOpen: " + isOpen);

        if (clip != null && source != null)
        {
            source.PlayOneShot(clip, 0.4f);
        }

        isOpen = !isOpen;
        targetRotation = isOpen ? openRotation : closedRotation;

        StartCoroutine(RotateDoor());
    }




    System.Collections.IEnumerator RotateDoor()
    {
        isMoving = true;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            yield return null;
        }

        transform.rotation = targetRotation;
        isMoving = false;
    }
}