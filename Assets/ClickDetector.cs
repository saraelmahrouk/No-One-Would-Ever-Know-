using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    public int clicksNeeded = 5;        // how many clicks required
    public float timeWindow = 2f;        // seconds to do it in

    private int clickCount = 0;
    private float timer = 0f;

    public PopupTrigger popupScript;

    void Update()
    {
        // Detect click
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;
            timer = timeWindow;

            Debug.Log("Clicks: " + clickCount);

            if (clickCount >= clicksNeeded)
            {
                TriggerAction();
                ResetClicks();
            }
        }

        // Countdown timer
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                ResetClicks();
            }
        }
    }

    void ResetClicks()
    {
        clickCount = 0;
        timer = 0f;
    }

    void TriggerAction()
    {
        Debug.Log("Action triggered!");
 
        popupScript.ShowPopup();
    }
}