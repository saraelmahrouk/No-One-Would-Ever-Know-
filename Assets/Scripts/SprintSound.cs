using UnityEngine;

public class SprintAudio : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource source;
    public AudioClip sprintClip;


    private bool isSprinting = false;

    void Update()
    {
        bool sprintKey = Input.GetKey(KeyCode.LeftShift);

        if (sprintKey && !isSprinting)
        {
            StartSprinting();
        }
        else if (!sprintKey && isSprinting)
        {
            StopSprinting();
        }
    }

    void StartSprinting()
    {
        isSprinting = true;

        if (source != null && sprintClip != null)
        {
            source.clip = sprintClip;
            source.loop = true;
            source.Play();
        }
    }

    void StopSprinting()
    {
        isSprinting = false;

        if (source != null)
        {
            source.Stop();
        }
    }
}