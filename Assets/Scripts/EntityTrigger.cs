using UnityEngine;

public class EntityTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnBecameVisible()
    {
        if (!triggered)
        {
            triggered = true;
            GameManagerRoom2.instance.TriggerEntitySeen();
        }
    }
}