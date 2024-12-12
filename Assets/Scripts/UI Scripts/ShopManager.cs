using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;


public class ShopManager : MonoBehaviour
{
    public PlayerPickup playerPickup;
    public int capacityUpgradeCost = 950;

    public int digestSpeedUpgradeCost = 1500;

    public int pointRateUpgradeCost = 1750;

    public int healthRecoveryCost = 500;

    public LocalizedString localizedCapacityCostString;
    public TextMeshProUGUI capacityCostText;

    public LocalizedString localizedDigestCostString;
    public TextMeshProUGUI digestCostText;

    public LocalizedString localizedPointRateString;
    public TextMeshProUGUI pointRateCostText;

    public LocalizedString localizedHealthRecoveryString;
    public TextMeshProUGUI healthRecoveryCostText;
    public Slider healthSlider;

    public AudioClip confirmSound;
    public AudioClip denySound;

    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        UpdateCapcityCostUI();
        UpdateDigestSpeedCostUI();
        UpdatePointRateCostUI();
        UpdateHealthCostUI();
    }

    public void UpgradeMaxCapacity()
    {
        if (playerPickup.points >= capacityUpgradeCost && playerPickup.capacityLevel < 5)
        {
            playerPickup.points -= capacityUpgradeCost;

            playerPickup.maxCapacity += 1;
            playerPickup.capacityLevel += 1;

            capacityUpgradeCost += 1000;
            PlaySound(confirmSound);
        }else{
            PlaySound(denySound);
        }
    }
    public void UpgradeDigestSpeed()
    {
        if (playerPickup.points >= digestSpeedUpgradeCost && playerPickup.digestionLevel < 5)
        {
            playerPickup.points -= digestSpeedUpgradeCost;
            if(playerPickup.digestionLevel == 4){
                playerPickup.digestionTime -= 0.5f;
            }else{
                playerPickup.digestionTime -= 1.0f;
            }
            playerPickup.digestionLevel += 1;

            digestSpeedUpgradeCost += 2000;
            PlaySound(confirmSound);
        }else{
            PlaySound(denySound);
        }
    }

    public void UpgradePointRate()
    {
        if (playerPickup.points >= pointRateUpgradeCost && playerPickup.pointRateLevel < 5)
        {
            playerPickup.points -= pointRateUpgradeCost;

            playerPickup.pointRateVal += 10;
            playerPickup.pointRateLevel += 1;

            pointRateUpgradeCost += 750;
            PlaySound(confirmSound);
        }else{
            PlaySound(denySound);
        }
    }

    public void RecoverHealth()
    {
        if (playerPickup.points >= healthRecoveryCost && playerPickup.currentHealth < playerPickup.maxHealth)
        {
            playerPickup.points -= healthRecoveryCost;
            playerPickup.currentHealth = playerPickup.maxHealth;
            healthSlider.value = playerPickup.currentHealth;
            healthRecoveryCost += 1000;
            PlaySound(confirmSound);
        }
        else{
            PlaySound(denySound);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void UpdateCapcityCostUI()
    {
        localizedCapacityCostString.Arguments = new object[] { capacityUpgradeCost };
        capacityCostText.text = localizedCapacityCostString.GetLocalizedString();
    }

    void UpdateDigestSpeedCostUI()
    {
        localizedDigestCostString.Arguments = new object[] { digestSpeedUpgradeCost };
        digestCostText.text = localizedDigestCostString.GetLocalizedString();
    }

    void UpdatePointRateCostUI()
    {
        localizedPointRateString.Arguments = new object[] { pointRateUpgradeCost };
        pointRateCostText.text = localizedPointRateString.GetLocalizedString();
    }
    void UpdateHealthCostUI()
    {
        localizedHealthRecoveryString.Arguments = new object[] { healthRecoveryCost };
        healthRecoveryCostText.text = localizedHealthRecoveryString.GetLocalizedString();
    }
}
