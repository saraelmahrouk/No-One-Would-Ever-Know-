using TMPro;
using UnityEngine;
using System.Collections;

public class MainDoorRoom2 : MonoBehaviour
{
    public Transform player;
    public float interactRange = 3f;

    public TextMeshProUGUI promptText;
    public CanvasGroup promptGroup;

    public float fadeSpeed = 4f;

    // 🔊 NEW: Audio
    public AudioSource audioSource;
    public AudioClip openDoorClip;

    private bool isOpen = false;

    public bool IsOpen => isOpen;

    // State tracking
    private bool wasInRange = false;
    private bool lastAllPapersRead = false;

    void Start()
    {
        if (promptGroup != null)
            promptGroup.alpha = 0f;

        if (promptText != null)
            promptText.gameObject.SetActive(false);

        if (audioSource != null)
            audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (player == null || GameManagerRoom2.instance == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        bool inRange = distance <= interactRange;
        bool allPapersRead = AllPapersRead();

        // UI logic
        if (inRange != wasInRange || allPapersRead != lastAllPapersRead)
        {
            if (inRange)
            {
                string msg = allPapersRead
                    ? "Press E to escape"
                    : "You still need to find all the papers";

                ShowMessage(msg);
            }
            else
            {
                StartCoroutine(FadeOut());
            }

            wasInRange = inRange;
            lastAllPapersRead = allPapersRead;
        }

        // Interaction
        if (inRange && allPapersRead && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDoor();
        }
    }

    // ✅ REPLACED (NO ROTATION NOW)
    void TriggerDoor()
    {
        if (isOpen) return;

        isOpen = true;

        // 🔊 Play sound
        if (audioSource != null && openDoorClip != null)
        {
            audioSource.Stop();
            audioSource.clip = openDoorClip;
            audioSource.Play();
        }

        // SceneTransition will detect IsOpen = true
    }

    bool AllPapersRead()
    {
        return GameManagerRoom2.instance != null &&
               GameManagerRoom2.instance.papersRead >= GameManagerRoom2.instance.totalPapers;
    }

    // ================= UI =================

    void ShowMessage(string msg)
    {
        if (promptText == null || promptGroup == null) return;

        promptText.text = msg;
        promptText.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            promptGroup.alpha = t;
            yield return null;
        }

        promptGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * fadeSpeed;
            promptGroup.alpha = t;
            yield return null;
        }

        promptGroup.alpha = 0f;
        promptText.gameObject.SetActive(false);
    }
}