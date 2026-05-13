using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class GameController : MonoBehaviour
{
    
    private List<bool> sequence;
    [SerializeField] List<GameObject> Objectives;
    [SerializeField] AudioSource AudioPlayer;
    [SerializeField] AudioClip RadioRecord;
    [SerializeField] private GameObject MirrorText;


    [Space]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoobj;
    [SerializeField] private GameObject FinalScene;
    [SerializeField] private TextMeshProUGUI AllHeWantedText;

    [SerializeField] private Animator _animator;
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    void Start()
    {
        sequence = new List<bool> {false, false};
        StartCoroutine("MovementTips");
    }




    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Mirror" && !sequence[0])
        {
            StartCoroutine("PlayRadio");
        }

        if(other.tag == "Camera" && sequence[0] && !sequence[1])
        {
            StartCoroutine("OpenCamera");
        }
    }

    IEnumerator MovementTips()
    {
        yield return new WaitForSeconds(0.5f);
        Objectives[0].SetActive(true);
        yield return new WaitForSeconds(3f);
        Objectives[0].SetActive(false);
        yield return new WaitForSeconds(1f);
        Objectives[1].SetActive(true);
    }


    IEnumerator PlayRadio()
    {
        Debug.Log("In front of mirror");
        Objectives[1].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        AudioPlayer.clip = RadioRecord;
        AudioPlayer.Play();

        yield return new WaitForSeconds(5f);
        MirrorText.SetActive(true);
        yield return new WaitForSeconds(2f);
        MirrorText.SetActive(false);

        yield return new WaitWhile(() => AudioPlayer.isPlaying);
        sequence[0] = true;

        Objectives[2].SetActive(true);
    }

    IEnumerator OpenCamera()
    {
        Debug.Log("Opening Camera");
        yield return new WaitForSeconds(0.5f);

        // Play Video
        Objectives[2].SetActive(false);
        videoobj.SetActive(true);
        if (videoPlayer == null)
            yield break;

        bool finished = false;

        // Subscribe to finish event
        videoPlayer.loopPointReached += OnVideoFinished;

        void OnVideoFinished(VideoPlayer vp)
        {
            finished = true;
        }

        videoPlayer.Play();

        // Wait until video actually starts playing
        yield return new WaitUntil(() => videoPlayer.isPlaying);

        // Wait until it finishes
        yield return new WaitUntil(() => finished);

        // Cleanup
        videoPlayer.loopPointReached -= OnVideoFinished;

        videoobj.SetActive(false);
        Debug.Log("Video finished playing!");


        // Open Final Scene
        yield return new WaitForSeconds(2f);
        _animator.SetBool(IsDeadHash, true);
        yield return new WaitForSeconds(3f);
        FinalScene.SetActive(true);
        StartCoroutine(FadeVertexColor(AllHeWantedText,4f));
        sequence[1] = true;
    }





    public IEnumerator FadeVertexColor(TextMeshProUGUI text, float duration)
    {
        if (text == null)
            yield break;

        float time = 0f;

        Color targetColor = Color.white;

        // Start fully transparent
        Color startColor = new Color(targetColor.r, targetColor.g, targetColor.b, 0f);

        while (time < duration)
        {
            float t = time / duration;

            Color currentColor = Color.Lerp(startColor, targetColor, t);
            text.color = currentColor;

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final color
        text.color = targetColor;
    }


}
