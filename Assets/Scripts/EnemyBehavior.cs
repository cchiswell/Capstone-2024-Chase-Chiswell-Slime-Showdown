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
    private Rigidbody rb;

    private GameObject player;
    private List<GameObject> meatObjects = new List<GameObject>();

    private Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateMeatObjects();
    }

    void FixedUpdate()
    {
        if (isPaused) return;

        UpdateMeatObjects();

        if (meatObjects.Count > 0)
        {
            // Chase the nearest meat object
            GameObject nearestMeat = GetNearestMeat();
            if (nearestMeat != null)
            {
                targetPosition = nearestMeat.transform.position;
            }
        }
        else
        {
            // Chase the player if no meat exists
            targetPosition = player.transform.position;
        }

        MoveTowards(targetPosition);
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
        Vector3 direction = (new Vector3(target.x, transform.position.y, target.z) - transform.position).normalized;

        // Use Rigidbody for movement
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        // Rotate to face the target smoothly
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Adjust rotation if the forward vector of the enemy isn't the default `z`
            targetRotation *= Quaternion.Euler(0, -90, 0); // Adjust if the model's forward is aligned differently

            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, speed * Time.fixedDeltaTime);
        }
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
