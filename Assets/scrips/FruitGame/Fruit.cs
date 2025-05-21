using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitType;

    public bool hasMered = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMered)
            return;

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        if (otherFruit != null && !otherFruit.hasMered && otherFruit.fruitType == fruitType)
        {
            hasMered = true;
            otherFruit.hasMered = true;

            Vector3 mergeposition = (transform.position + otherFruit.transform.position) / 2f;

            FruitGame gameManager = FindObjectOfType<FruitGame>();
            if (gameManager != null)
            {
                gameManager.MergeFruits(fruitType, mergeposition);
            }

           Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }
}
