using UnityEngine;
using TMPro;

public class PaperInteract : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 2.5f;
    public AudioClip clip;

    [Header("UI References")]
    public GameObject paperImageUI;
    public GameObject promptText;

    [Header("Objective Message")]
    public TextMeshProUGUI objectiveText;
    public string newObjectiveMessage = "Find a way out.";

    private Transform player;
    private AudioSource source;
    private bool imageVisible = false;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        GameObject baldMan = GameObject.Find("BaldMan");

        if (baldMan != null)
            player = baldMan.transform;

        if (paperImageUI != null)
            paperImageUI.SetActive(false);

        if (promptText != null)
            promptText.SetActive(false);

        if (source != null)
        {
            source.playOnAwake = false;
            source.loop = false;
            source.clip = clip;
            source.Stop();
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool playerNearby = distance <= interactRange;

        if (promptText != null)
            promptText.SetActive(playerNearby && !imageVisible);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!imageVisible && playerNearby)
            {
                OpenPaper();
            }
            else if (imageVisible)
            {
                ClosePaper();
            }
        }
    }

    void OpenPaper()
    {
        imageVisible = true;
        DoorInteract.paperRead = true;

        if (paperImageUI != null)
            paperImageUI.SetActive(true);

        if (promptText != null)
            promptText.SetActive(false);

        if (objectiveText != null)
            objectiveText.text = newObjectiveMessage;

        if (source != null && clip != null)
        {
            source.Stop();
            source.clip = clip;
            source.time = 0f;
            source.Play();
        }
    }

    void ClosePaper()
    {
        imageVisible = false;

        if (paperImageUI != null)
            paperImageUI.SetActive(false);

        if (promptText != null)
            promptText.SetActive(true);

        if (source != null)
        {
            source.Stop();
            source.clip = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.CollectPaper();
            Destroy(gameObject);
        }
    }
}