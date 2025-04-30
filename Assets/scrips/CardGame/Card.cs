using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public int cardValue;                          //카드 값 (카드 단계)
    public Sprite cardImage;                      //카드 이미지
    public TextMeshPro cardText;                 //카드 텍스트

    //카드 정보 초기화 함수

    public void InitCard(int calue, Sprite image)
    {
        cardValue = calue;
        cardImage = image;

        //카드 이미지 설정
        GetComponent<SpriteRenderer>().sprite = image;

        //카드 텍스트 설정 (있는경우)
        if (cardText != null)
        {
            cardText.text = cardValue.ToString();
        }
        
    }


   
}
