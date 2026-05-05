using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public Image healthFill;

    public MazeTrigger maze;

    public HealthBarScript healthBar;
    void Start()
    {
        currentHealth = maxHealth;

        healthFill.fillAmount = 1f;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; 

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
