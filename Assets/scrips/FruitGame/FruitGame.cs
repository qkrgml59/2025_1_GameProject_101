using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitGame : MonoBehaviour
{

    public GameObject[] fruitPrefabs;

    public float[] fruitSizes = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f };

    public GameObject currentFruit;
    public int currentFruitType;

    public float fruitStartHeight = 6.0f;
    public float gameWidth = 5.0f;
    public bool isGameOver = false;
    public Camera mainCamera;

    public float fruitTimer;

    public float gameHeight;
           
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewFruit();
        fruitTimer = -3.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        if (fruitTimer >=0)
        {
            fruitTimer -= Time.deltaTime;
        }

        if (fruitTimer < 0 && fruitTimer > -2)
        {
            CheckGameOver();
            SpawnNewFruit();
            fruitTimer = -3.0f;
        }

        if (currentFruit != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 newPosition = currentFruit.transform.position;
            newPosition.x = worldPosition.x;

            float halfFruitSize = fruitSizes[currentFruitType] / 2f;
            if (newPosition.x < -gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }
            if (newPosition.x > gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = gameWidth / 2 + halfFruitSize;
            }

            currentFruit.transform.position = newPosition;

        }

        if (Input.GetMouseButtonDown(0) && fruitTimer == -3.0f)
        {
            DropFruit();
        }
    }

    void SpawnNewFruit()
    {
        if(! isGameOver )
        {
            currentFruitType = Random.Range(0, 3);

            Vector3 mousePositon = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePositon);

            Vector3 spawnPosition = new Vector3(worldPosition.x, fruitStartHeight, 0);

            float halfFruitSize = fruitSizes[currentFruitType] / 2;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

            currentFruit = Instantiate(fruitPrefabs[currentFruitType], spawnPosition, Quaternion.identity);
            currentFruit.transform.localScale = new Vector3(fruitSizes[currentFruitType], fruitSizes[currentFruitType], 1);

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if ( rb != null )
            {
                rb.gravityScale = 0f;
            }
        
        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.gravityScale = 1f;

            currentFruit = null;

            fruitTimer = 1.0f;
        }
    }

    public void MergeFruits (int fruitType, Vector3 position)
    {
        if(fruitType < fruitPrefabs.Length -1)
        {
            GameObject newFruit = Instantiate(fruitPrefabs[fruitType + 1], position, Quaternion.identity);
            newFruit.transform.localScale = new Vector3(fruitSizes[fruitType + 1], fruitSizes[fruitType + 1], 1.0f);


        }
    }

    public void CheckGameOver()
    {
        Fruit[] allFruits = FindObjectsOfType<Fruit>();

        float gameOverHeight = gameHeight;

        for(int i = 0; i < allFruits.Length; i++)
        {
            if (allFruits[i] != null)
            {
                Rigidbody2D rb = allFruits[i].GetComponent<Rigidbody2D>();

                if (rb != null && rb.velocity.magnitude < 0.1f && allFruits[i].transform.position.y > gameOverHeight)
                {
                    isGameOver = true;
                    Debug.Log("게임 오버");

                    break;
                }
            }
        }
    }
}
