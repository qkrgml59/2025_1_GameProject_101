using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    //������ ���ҽ�
    public GameObject cardPrefab;
    public Sprite[] cardImages;
    //���� ���
    public Transform deckArea;
    public Transform handArea;
    //UI ���
    public Button drawButton;
    public TextMeshProUGUI deckCountText;
    //���� ��
    public float cardSpacing = 2.0f;
    public int maxHandSize = 6;

    //�迭 ����
    public GameObject[] deckCards;
    public int deckcount;

    public GameObject[] handCards;
    public int handCount;

    //�̸� ���ǵ� �� ī�� ��� (���ڸ�)
    public int[] prefedinedDeck = new int[]
    {
        1,1,1,1,1,1,1,1, //1�� 8�� 
        2,2,2,2,2,2,     //2�� 6��
        3,3,3,3,    //3�� 4��
        4,4   //4�� 2�� 
    };

    public Transform mergeArea;
    public Button mergeButton;
    public int maxMergeSize = 3;

    public GameObject[] mergeCards;
    public int mergeCount;


    // Start is called before the first frame update
    void Start()
    {
        //�迭 �ʱ�ȭ
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];
        mergeCards = new GameObject[maxMergeSize];  

        InitializeDeck();
        ShuffleDeck();

        if(drawButton != null)
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked);
        }

        if(mergeButton != null)
        {
            mergeButton.onClick.AddListener(OnMergeButtonClicked);
            mergeButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�� ����

    void ShuffleDeck()        //Fisher-Yates ���� �˰���
    {
        for (int i = 0; i < deckcount - 1; i++)
        {
            int j = Random.Range(i, deckcount);
            //�迭 �� ī�� ��ȯ
            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }
    }


    //�� �ʱ�ȭ - ������ ī�� ����
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

            //ī�� ������Ʈ ���� (����ġ)
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

    public void ArrangeMerge()
    {
        if (mergeCount == 0)
            return;

        float startX = -(mergeCount - 1) * cardSpacing / 2;

        for (int i = 0; i < mergeCount; i++)
        {
            if (mergeCards[i] != null)
            {
                Vector3 newPos = mergeArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                mergeCards[i].transform.position = newPos;
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
            Debug.Log("���а� ���� á���ϴ�.!");
            return;
        }

        if (deckcount <= 0)
        {
            Debug.Log("���� �� �̻� ī�尡 �����ϴ�.");
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

    public void MoveCardToMerge(GameObject card)
    {
        if (mergeCount >= maxMergeSize)
        {
            Debug.Log("���� ������ ���� á���ϴ�.!");
            return;
        }


        for (int i = 0; i < handCount - 1; i++)
        {
            if (handCards[i] == card)
            {
                for(int j = i; j < handCount - 1; j++)
                {
                    handCards[j] = handCards[j + 1];
                }
                handCards[handCount - 1] = null;
                handCount--;

                ArrangeHand();
                break;
            }
        }
      
        mergeCards[mergeCount] = card;
        mergeCount++;

        card.transform.SetParent(mergeArea);
        ArrangeMerge();
        UpdateMergeButtonState();

    }

    void UpdateMergeButtonState()
    {
        if (mergeButton != null)
        {
            mergeButton.interactable = (mergeCount == 2 || mergeCount == 3);
        }
    }

    void MergeCards()
    {
        if(mergeCount !=2 && mergeCount !=3)
        {
            Debug.Log("������ �Ϸ��� ī�尡 2�� �Ǵ� 3�� �ʿ��մϴ�.");
            return;
        }

        int firstCard = mergeCards[0].GetComponent<Card>().cardValue;
        for (int i = 1;i< mergeCount; i++)
        {
            Card card = mergeCards[i].GetComponent<Card>();
            if(card == null || card.cardValue != firstCard)
            {
                Debug.Log("���� ������ ī�常 ���� �� �� �ֽ��ϴ�.");
                return;
            }
        }

        int newValue = firstCard + 1;

        if(newValue > cardImages.Length)
        {
            Debug.Log("�ִ� ī�� ���� ���� �߽��ϴ�.");
            return ;
        }

        for (int i = 0; i < mergeCount;i++)
        {
            if (mergeCards[i] != null)
                {
                mergeCards[i].SetActive(false);
            }
        }

        GameObject newCard = Instantiate(cardPrefab, mergeArea.position, Quaternion.identity);

        Card newCardTemp = newCard.GetComponent<Card>();
        if(newCardTemp != null)
        {
            int imageIndex = newValue - 1;
            newCardTemp.InitCard(newValue, cardImages[imageIndex]);
        }

        for(int i = 0; i < maxMergeSize; i++)
        {
            mergeCards[i] = null;
        }

        mergeCount = 0;

        UpdateMergeButtonState();

        handCards[handCount] = newCard;
        handCount++;
        newCard.transform.SetParent(handArea);

        ArrangeHand();
    }

    void OnMergeButtonClicked()
    {
        MergeCards();
    }
   
}
