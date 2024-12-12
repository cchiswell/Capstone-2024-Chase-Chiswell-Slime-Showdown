using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3.0f;
    public float playerKnockbackForce = 5.0f;
    public int damage = 1;

    public float pauseDuration = 2f;
    private bool isPaused = false;
    Rigidbody rb;

    private GameObject player;
    private List<GameObject> meatObjects = new List<GameObject>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateMeatObjects();
    }

    void Update()
    {
        // Regularly update the list of meat objects
        UpdateMeatObjects();

        if (!isPaused)
        {
            if (meatObjects.Count > 0)
            {
                // Chase the nearest meat object
                GameObject nearestMeat = GetNearestMeat();
                if (nearestMeat != null)
                {
                    MoveTowards(nearestMeat.transform.position);
                }
            }
            else
            {
                // Chase the player if no meat exists
                MoveTowards(player.transform.position);
            }
        }
    }

    void UpdateMeatObjects()
    {
        meatObjects.Clear();
        meatObjects.AddRange(GameObject.FindGameObjectsWithTag("Meat"));

        // Remove any null entries caused by destroyed objects
        meatObjects.RemoveAll(meat => meat == null);
    }

    GameObject GetNearestMeat()
    {
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject meat in meatObjects)
        {
            float distance = Vector3.Distance(transform.position, meat.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = meat;
            }
        }

        return nearest;
    }

    void MoveTowards(Vector3 target)
    {
        // Calculate direction but ignore the y-axis
        Vector3 direction = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z).normalized;

        // Move towards the target
        transform.position += direction * speed * Time.deltaTime;

        // Rotate to face the target with the correct front alignment
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Adjust rotation if the forward vector of the enemy isn't the default `z`
        targetRotation *= Quaternion.Euler(0, -90, 0); // Adjust this if your model's forward is aligned with `x`

        // Smoothly rotate to the target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Meat"))
        {
            // Destroy meat object on contact
            Destroy(collision.gameObject);
            UpdateMeatObjects();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Deal damage and knock back the player
            player.GetComponent<PlayerPickup>().TakeDamage(damage);

            Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.AddForce(knockbackDirection * playerKnockbackForce, ForceMode.Impulse);
            }

            StartCoroutine(PauseMovement());
        }
    }

    IEnumerator PauseMovement()
    {
        isPaused = true;
        rb.velocity = Vector3.zero; // Stop the enemy's movement
        yield return new WaitForSeconds(pauseDuration);
        isPaused = false;
    }
}
