using UnityEngine;
using TMPro;
using System.Collections;

public class GameManagerRoom2 : MonoBehaviour
{
    public static GameManagerRoom2 instance;

    public TextMeshProUGUI captionText;
    public CanvasGroup captionGroup;

    public TextMeshProUGUI objectiveText;
    public CanvasGroup objectiveGroup;

    public AudioSource audioSource;
    public AudioClip whoIsThatClip;

    public int papersRead = 0;
    public int totalPapers = 3;

    private bool captionPlayed = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        captionText.gameObject.SetActive(false);
        objectiveText.gameObject.SetActive(false);

        captionGroup.alpha = 0;
        objectiveGroup.alpha = 0; 
    }

    public void TriggerEntitySeen()
    {
        if (!captionPlayed)
        {
            StartCoroutine(CaptionSequence());
            captionPlayed = true;
        }
    }

    IEnumerator CaptionSequence()
    {
        captionText.gameObject.SetActive(true);

        // 1st line
        captionText.text = "Who is that?";

        audioSource.PlayOneShot(whoIsThatClip);

        yield return StartCoroutine(FadeIn());

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(FadeOut());

        // 2nd line
        captionText.text = "I need to get out of this place";

        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOut());

        captionText.gameObject.SetActive(false);

        objectiveText.gameObject.SetActive(true);

        float t = 0f;
        objectiveGroup.alpha = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            objectiveGroup.alpha = t;
            yield return null;
        }

        objectiveGroup.alpha = 1f;

    }

    IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f; // speed
            captionGroup.alpha = t;
            yield return null;
        }

        captionGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * 2f;
            captionGroup.alpha = t;
            yield return null;
        }

        captionGroup.alpha = 0f;
    }


    public void CollectPaper()
    {
        papersRead++;

        objectiveText.gameObject.SetActive(true);
        objectiveGroup.alpha = 1f;

        UpdateObjectiveUI();
    }

    void UpdateObjectiveUI()
    {
        string debugText;

        if (papersRead >= totalPapers)
        {
            debugText = "Objective:\r\nFind a way out.";
        }
        else
        {
            debugText =
                "Objective:\r\nFind the diary papers.\r\n" +
                "Papers read: " + papersRead + "/" + totalPapers;
        }

        objectiveText.text = debugText;

        Debug.Log("SET TEXT: " + debugText);
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(whoIsThatClip);
    }
}