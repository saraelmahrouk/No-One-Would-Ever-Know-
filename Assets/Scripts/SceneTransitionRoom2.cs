using UnityEngine;

public class SceneTransitionRoom2 : MonoBehaviour
{
    public int sceneIndexToLoad;
    public MainDoorRoom2 door;

    private bool hasTriggered = false;

    void Update()
    {
        if (hasTriggered) return;

        if (door == null) return;

        if (GameManagerRoom2.instance == null) return;

        bool allPapersRead =
            GameManagerRoom2.instance.papersRead >= GameManagerRoom2.instance.totalPapers;

        if (door.IsOpen && allPapersRead)
        {
            hasTriggered = true;
            FadeManager.FadeToScene(sceneIndexToLoad);
        }
    }
}