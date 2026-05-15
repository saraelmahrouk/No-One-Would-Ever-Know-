using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int diariesCollected = 0;
    public TextMeshProUGUI progressText;
    public Image progressBarFill;
    public TextMeshProUGUI completionMessage;

    void Awake()
    {
        instance = this;
    }

    public void CollectDiary()
    {
        diariesCollected++;

        float fillAmount = diariesCollected / 3f;
        progressBarFill.fillAmount = fillAmount;
        progressText.text = "Diaries Found: " 
                          + diariesCollected + " / 3";

        if (diariesCollected >= 3)
        {
            StartCoroutine(ShowMessageAndTransition());
        }
    }

    IEnumerator ShowMessageAndTransition()
    {
        completionMessage.gameObject.SetActive(true);
        completionMessage.text = 
            "You have found all the diaries...\nThe truth awaits.";
        yield return new WaitForSeconds(3f);
        // transition to Room 3 will be added later
    }
    public void CollectPaper()
{
    CollectDiary();
}
}