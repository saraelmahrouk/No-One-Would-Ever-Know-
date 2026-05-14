using UnityEngine;

public class LookAtEntityDetector : MonoBehaviour
{
    public Camera cam;
    public float distance = 10f;
    public LayerMask entityLayer;

    private bool triggered = false;
    private bool canCheck = false;

    void Start()
    {
        // prevents instant triggering at scene start
        Invoke(nameof(EnableDetection), 1.5f);
    }

    void EnableDetection()
    {
        canCheck = true;
    }

    void Update()
    {
        if (!canCheck || triggered) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distance, entityLayer))
        {
            triggered = true;
            GameManagerRoom2.instance.TriggerEntitySeen();
        }
    }
}