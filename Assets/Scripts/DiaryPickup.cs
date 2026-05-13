using UnityEngine;
using TMPro;

public class DiaryPickup : MonoBehaviour
{
    public GameObject diaryPanel;
    public AudioSource diaryAudio;
    private bool playerInRange = false;
    private bool collected = false;

    void Update()
    {
        if (playerInRange && !collected && Input.GetKeyDown(KeyCode.E))
        {
            ShowDiary();
        }
        else if (collected && diaryPanel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            diaryPanel.SetActive(false);
            diaryAudio.Stop();
        }
    }

    void ShowDiary()
    {
        collected = true;
        diaryPanel.SetActive(true);
        diaryAudio.Play();
        GameManager.instance.CollectDiary();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}