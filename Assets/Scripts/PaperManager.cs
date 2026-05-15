using UnityEngine;

public class PaperManager : MonoBehaviour
{
    public static PaperManager Instance;

    public int papersRead = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddPaper()
    {
        papersRead++;
    }
}