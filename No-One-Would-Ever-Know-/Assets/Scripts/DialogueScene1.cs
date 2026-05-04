using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueScene1 : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = 0.05f;
    public float startDelay = 3f;

    [Header("End Settings")]
    public bool hideAfterFinished = true;

    private int index;
    private CanvasGroup canvasGroup;
    private bool dialogueStarted = false;
    private bool dialogueFinished = false;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        HideDialogue();
    }

    void Start()
    {
        StartCoroutine(StartDialogueAfterDelay());
    }

    void Update()
    {
        if (!dialogueStarted || dialogueFinished) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    IEnumerator StartDialogueAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);

        ShowDialogue();
        StartDialogue();
    }

    void StartDialogue()
    {
        dialogueStarted = true;
        dialogueFinished = false;
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueFinished = true;

            if (hideAfterFinished)
            {
                HideDialogue();
            }
            else
            {
                ShowDialogue();
            }
        }
    }

    void ShowDialogue()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void HideDialogue()
    {
        if (textComponent != null)
        {
            textComponent.text = string.Empty;
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        dialogueStarted = false;
    }
}