using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMPro.TextMeshProUGUI healthText; // looks like current/max
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // Prevent division by zero and update the slider value
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}"; // Update the text to show current/max health
        }
    }
}
