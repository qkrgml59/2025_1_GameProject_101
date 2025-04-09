using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100;
    public float Timer = 1.0f;
    public int AttackPoint = 10;

    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        CharacterHealthUp();
        CheckDeath();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterHit(AttackPoint);
        }


        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Timer = 1.0f;
            Health += 20;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Health -= AttackPoint;
        }

        CheckDeath();
    }

       public void CharacterHit(int Damage)
        {
            Health -= Damage;
        }

        void CheckDeath()
        {
            if (Health <= 0)
                Destroy(gameObject);
        }

        void CharacterHealthUp()
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                Timer = 1.0f;
                Health += 20;
            }
        }
    }

