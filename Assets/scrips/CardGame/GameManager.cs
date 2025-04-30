using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    //프리팹 리소스
    public GameObject cardPrefab;
    public Sprite[] cardImages;
    //영역 요소
    public Transform deckArea;
    public Transform handArea;
    //UI 요소
    public Button drawButton;
    public TextMeshProUGUI deckCountText;
    //설정 값
    public float cardSpacing = 2.0f;
    public int maxHandSize = 6;

    //배열 선언
    public GameObject[] deckCards;
    public int deckcount;

    public GameObject[] handCards;
    public int handCount;

    //미리 정의된 덱 카드 목록 (숫자만)
    public int[] prefedinedDeck = new int[]
    {
        1,1,1,1,1,1,1,1, //1이 8장 
        2,2,2,2,2,2,     //2가 6장
        3,3,3,3,    //3이 4장
        4,4   //4가 2장 
    };

    // Start is called before the first frame update
    void Start()
    {
        //배열 초기화
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];

        InitializeDeck();
        ShuffleDeck();

        if(drawButton != null)
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //덱 셔플

    void ShuffleDeck()        //Fisher-Yates 셔플 알고리즘
    {
        for (int i = 0; i < deckcount - 1; i++)
        {
            int j = Random.Range(i, deckcount);
            //배열 내 카드 교환
            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }
    }


    //덱 초기화 - 정해진 카드 생성
    private void InitializeDeck()
    {
        deckcount = prefedinedDeck.Length;

        for (int i = 0; i < prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];

            int imageIndex = value - 1;
            if(imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0; 
            }

            //카드 오브젝트 생성 (덱위치)
            GameObject newCardObj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(false);

            Card cardComp = newCardObj.GetComponent<Card>();
            if (cardComp != null )
            {
                cardComp.InitCard(value, cardImages[imageIndex]);
            }
            deckCards[i] = newCardObj;

        }

    }




    public void ArrangeHand()
    {
        if (handCount == 0)
            return;

        float startX = -(handCount - 1) * cardSpacing / 2;

        for (int i = 0; i <handCount; i++)
        {
            if (handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handCards[i].transform.position = newPos;
            }

        }

    }

    void OnDrawButtonClicked()
    {
        DrawCardToHand();
    }

    public void DrawCardToHand()
    {
        if (handCount >= maxHandSize)
        {
            Debug.Log("손패가 가득 찼습니다.!");
            return;
        }

        if (deckcount <= 0)
        {
            Debug.Log("덱에 더 이상 카드가 없습니다.");
            return;
        }

        GameObject drawnCard = deckCards[0];

        for (int i = 0; i < deckcount -1; i++) 
            {
            deckCards[i] = deckCards[i + 1];
            }
        deckcount--;

        drawnCard.SetActive(true);
        handCards[handCount] = drawnCard;
        handCount++;

        drawnCard.transform.SetParent(handArea);

        ArrangeHand();
    }

   
}
