using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public bool isDragging = false;        //드래그인지 판별하는 bool값
    public Vector3 startPosition;     //드래그 시작 위치
    public Transform startParent;       //드래그 시작시 있던 영역

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startParent = transform.parent;

        gameManager = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging)              //드래그 중이면 마우스 위치로 카드 이동
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }

    private void OnMouseDown()     //마우스 클릭 시 드래그 시작
    {
        isDragging =  true;

        startPosition = transform.position;
        startParent = transform.parent;

        GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    void OnMouseUp()            //마우스 버튼 놓을 때
    {
        isDragging = false;
        GetComponent<SpriteRenderer>().sortingOrder = 1;

        if(gameManager == null)
        {
            RetrunToOriginalPosition();
            return;
        }

        bool waslnMergeArea = startParent == gameManager.mergeArea;

        if(IsOverArea(gameManager.handArea))
        {
            Debug.Log("손패 영역으로 이동");

            if(waslnMergeArea)
            {
                for(int i = 0; i <gameManager.mergeCount; i++)
                {
                    if(gameManager.mergeCards[i] == gameObject)
                    {
                        for(int j = i; j < gameManager.mergeCount -1; j++)
                        {
                            gameManager.mergeCards[j] = gameManager.mergeCards[j + 1];
                        }
                        gameManager.mergeCards[gameManager.mergeCount - 1] = null;
                        gameManager.mergeCount--;

                        transform.SetParent(gameManager.handArea);
                        gameManager.handCards[gameManager.handCount] = gameObject;
                        gameManager.handCount++;

                        gameManager.ArrangeHand();
                        gameManager.ArrangeMerge();
                        break;
                    }
                }
            }
            else
            {
                gameManager.ArrangeHand();
            }
        }
        else if(IsOverArea(gameManager.mergeArea))
        {
            if(gameManager.mergeCount >= gameManager.maxMergeSize)
            {
                Debug.Log("머지 영역이 가득 찼습니다.");
                RetrunToOriginalPosition();
            }
            else
            {
                gameManager.MoveCardToMerge(gameObject);
            }
        }
        else
        {
            RetrunToOriginalPosition();
        }
        if(waslnMergeArea)
        {
            if(gameManager.mergeButton != null)
            {
                bool canMerge = (gameManager.mergeCount == 2 || gameManager.mergeCount == 3);
                gameManager.mergeButton.interactable = canMerge;
            }
        }
        
    }

    //원래 위치로 돌아가는 함수

    void RetrunToOriginalPosition()
    {
        transform.position = startPosition;
        transform.SetParent(startParent);

        if(gameManager != null)
        {
            if(startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand();
            }
            if (startParent == gameManager.mergeArea)
            {
                gameManager.ArrangeMerge();
            }
        }
    }

    bool IsOverArea(Transform area)
    {
        if(area == null)
        {
            return false;
        }
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider != null && hit.collider.transform == area)
            {
                Debug.Log(area.name + "영역 감지됨");
                return true;
            }
        }

        return false;
    }
    
   
    
}
