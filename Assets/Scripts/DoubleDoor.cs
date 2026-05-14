using UnityEngine;

public class DoubleDoor : MonoBehaviour
{
    [Header("Door Parts")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Settings")]
    public float openAngle = 90f;
    public float speed = 2f;
    public float interactRange = 3f;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip openClip;
    public AudioClip closeClip;

    private bool isOpen = false;

    private Quaternion leftClosedRot;
    private Quaternion rightClosedRot;

    private Quaternion leftOpenRot;
    private Quaternion rightOpenRot;

    public Transform player;
    void Start()
    {

        player = GameObject.Find("BaldMan").transform;

        leftClosedRot = leftDoor.localRotation;
        rightClosedRot = rightDoor.localRotation;

        // Left opens outward (-Y)
        leftOpenRot = Quaternion.Euler(0f, -openAngle, 0f);

        // Right opens outward (+Y)
        rightOpenRot = Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > interactRange)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;

            if (source != null && openClip != null && closeClip != null)
            {
                if (isOpen)
                    source.PlayOneShot(openClip, 0.4f);
                else
                    source.PlayOneShot(closeClip, 0.4f);
            }
        }


        if (isOpen)
        {
            leftDoor.localRotation = Quaternion.Slerp(
                leftDoor.localRotation,
                leftOpenRot,
                Time.deltaTime * speed
            );

            rightDoor.localRotation = Quaternion.Slerp(
                rightDoor.localRotation,
                rightOpenRot,
                Time.deltaTime * speed
            );
        }
        else
        {
            leftDoor.localRotation = Quaternion.Slerp(
                leftDoor.localRotation,
                leftClosedRot,
                Time.deltaTime * speed
            );

            rightDoor.localRotation = Quaternion.Slerp(
                rightDoor.localRotation,
                rightClosedRot,
                Time.deltaTime * speed
            );
        }
    }
}