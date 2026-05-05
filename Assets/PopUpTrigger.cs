using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    public GameObject popupImage;

    public void ShowPopup()
    {
        popupImage.SetActive(true);
    }

    public void HidePopup()
    {
        popupImage.SetActive(false);

    }
}