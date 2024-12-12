using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatPickup : MonoBehaviour
{
    // Declare an event to notify objSpawner when picked up
    public delegate void PickedUpHandler();
    public event PickedUpHandler OnPickedUp;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player" && other.GetComponent<PlayerPickup>().capacity < other.GetComponent<PlayerPickup>().maxCapacity)
        {
            PlayerPickup playerPickup = other.GetComponent<PlayerPickup>();
            // Add 1 to stomach capacity
            other.GetComponent<PlayerPickup>().capacity++;

            //play the sound for a meat being picked up
            playerPickup.PlaySound(playerPickup.meatPickupSound);

            // Invoke the pickup event
            OnPickedUp?.Invoke();

            // Destroy the object after pickup
            Destroy(gameObject);
        }

        else if (other.CompareTag("Enemy"))
        {
            OnPickedUp?.Invoke();
            Destroy(gameObject);
        }
    }
}
