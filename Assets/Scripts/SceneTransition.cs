using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public int sceneIndexToLoad;
    public DoorInteract door;

    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered && door != null && door.IsOpen)
        {
            hasTriggered = true;
            FadeManager.FadeToScene(sceneIndexToLoad);
        }
    }

}