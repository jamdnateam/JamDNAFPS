using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float playerHealth = 100;

    public void Start()
    {
        playerHealth = 100;
    }
    public void TakeDamage(float amount)
    {
        playerHealth -= amount;

        if (playerHealth <= 0f)
        {
            playerHealth = 0f;
            Die();
        }

    }

    void Die()
    {
        SceneManager.LoadScene("MainMenu");
        // Destroy(gameObject);
        // end or start map over

    }
}
