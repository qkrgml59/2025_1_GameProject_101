using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlyerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    public float invincibleTime = 1.0f;
    public bool islnvincible = false;

    private void Start()
    {
        currentLives = maxLives;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            currentLives--;
            Destroy(other.gameObject);

            if (currentLives == 0)
            {
                GameOver();
            }
        }

        void GameOver()
        {

            gameObject.SetActive(false);
            Invoke("RestartGame", 3.0f);
        }

        void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}


