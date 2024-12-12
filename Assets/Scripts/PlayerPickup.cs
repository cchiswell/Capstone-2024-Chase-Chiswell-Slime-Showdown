using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

//this is the main player behavior script!


public class PlayerPickup : MonoBehaviour
{
    public int capacity = 0;
    public int points = 0;
    public int maxCapacity = 5;
    public int pointRateVal = 50;

    public int capacityLevel = 1;
    public int pointRateLevel = 1;
    public int digestionLevel = 1;

    public int maxHealth= 3;
    public int currentHealth;
    public TextMeshProUGUI stomachCapacityText;
    public TextMeshProUGUI pointsText;
    public LocalizedString localizedCapacityString;
    public LocalizedString localizedPointsString;

    public LocalizedString localizedDigestionUpgradeString;
    public TextMeshProUGUI digestionUpgradeText;

    public LocalizedString localizedCapacityUpgradeString;
    public TextMeshProUGUI capacityUpgradeText;

    public LocalizedString localizedPointsUpgradeString;
    public TextMeshProUGUI pointsUpgradeText;

    public Slider digestionSlider;
    public Slider healthBar;
    public float digestionTime = 5.0f;
    public PlayerMovement playerMovement;
    private float digestionProgress = 0f;
    private bool isDigesting = false;

    public GameObject gameOverCanvas;

    public bool isInvincible = false;
    public float invincibilityDuration = 1.0f;
    public GameOverManager gameOverManager;

    public AudioClip takeDamageSound;

    public AudioClip digestCompleteSound;

    public AudioClip meatPickupSound;

    public AudioSource audioSource;

    void Start()
    {
        Time.timeScale = 1f;
        digestionSlider.gameObject.SetActive(false);
        UpdateStomachCapacityUI();
        UpdatePointsUI();
        UpdateDigestionUpgradeUI();
        UpdateCapacityUpgradeUI();
        UpdatePointsUpgradeUI();
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        playerMovement.enabled = true;
        gameOverCanvas.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (capacity > maxCapacity) capacity = maxCapacity;
        if (capacity < 0) capacity = 0;

        UpdateStomachCapacityUI();
        UpdatePointsUI();
        UpdateDigestionUpgradeUI();
        UpdateCapacityUpgradeUI();
        UpdatePointsUpgradeUI();

        // Check if the player is holding down the digest key (E)
        if (Input.GetKey(KeyCode.E) && capacity > 0)
        {
            if (!isDigesting)
            {
                // Start digesting, so show the slider
                digestionSlider.gameObject.SetActive(true);
                playerMovement.enabled = false;
            }
            isDigesting = true;
            digestionProgress += Time.deltaTime / digestionTime;

            // Update Slider to reflect progress
            digestionSlider.value = digestionProgress;

            // If digestion completes
            if (digestionProgress >= 1.0f)
            {
                AwardPoints();
                digestionProgress = 0f;
                digestionSlider.value = 0f;
                digestionSlider.gameObject.SetActive(false);
                playerMovement.enabled = true;
                isDigesting = false;
            }
        }
        else
        {
            // Reset digestion if key released before completion
            if (isDigesting && !Input.GetKey(KeyCode.E))
            {
                digestionProgress = 0f;
                digestionSlider.value = 0f;
                digestionSlider.gameObject.SetActive(false);
                playerMovement.enabled = true;
                isDigesting = false;
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void AwardPoints()
    {
        points += Mathf.FloorToInt(capacity * capacity * pointRateVal);
        capacity = 0;
        PlaySound(digestCompleteSound);
    }

    void UpdateStomachCapacityUI()
    {
        localizedCapacityString.Arguments = new object[] { capacity, maxCapacity };
        stomachCapacityText.text = localizedCapacityString.GetLocalizedString();   
    }

    void UpdatePointsUI()
    {
        localizedPointsString.Arguments = new object[] { points };
        pointsText.text = localizedPointsString.GetLocalizedString();
    }

    void UpdateDigestionUpgradeUI()
    {
        localizedDigestionUpgradeString.Arguments = new object[] { digestionLevel };
        digestionUpgradeText.text = localizedDigestionUpgradeString.GetLocalizedString();
    }
    void UpdateCapacityUpgradeUI()
    {
        localizedCapacityUpgradeString.Arguments = new object[] { capacityLevel };
        capacityUpgradeText.text = localizedCapacityUpgradeString.GetLocalizedString();
    }

    void UpdatePointsUpgradeUI()
    {
        localizedPointsUpgradeString.Arguments = new object[] { pointRateLevel };
        pointsUpgradeText.text = localizedPointsUpgradeString.GetLocalizedString();
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            PlaySound(takeDamageSound);
            currentHealth -= damage;
            healthBar.value = currentHealth;
            digestionProgress = 0f;
            StartCoroutine(InvincibilityCooldown());
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameOverManager.GameOver();
        }
    }

    void GameOver()
    {
        //stop the game
        Time.timeScale = 0;
        gameOverCanvas.SetActive(true);
        playerMovement.enabled = false;
    }

    private IEnumerator InvincibilityCooldown()
    {
        isInvincible = true;

        Renderer renderer = GetComponentInChildren<Renderer>();
        Material material = renderer.material;
        Color originalColor = material.color;

        // Enable emission if supported
        material.EnableKeyword("_EMISSION");
        Color flashColor = Color.red; // want to flash RED
        Color emissionColor = flashColor * Mathf.LinearToGammaSpace(2.0f);

        for (int i = 0; i < 5; i++) // Flash 5 times
        {
            material.color = flashColor; // Change object color
            material.SetColor("_EmissionColor", emissionColor); // Set emission color
            yield return new WaitForSeconds(0.1f);

            material.color = originalColor; // Reset color
            material.SetColor("_EmissionColor", Color.black); // Disable emission
            yield return new WaitForSeconds(0.1f);
        }

        material.DisableKeyword("_EMISSION"); // Disable emission when done
        isInvincible = false;

        //this makes the player model flash red to indicate damage while the invincibility cooldown is rolling
    }
}
