using System.Collections;
using System.Collections.Generic;
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

        RetrunToOriginalPosition();
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
        }
    }

    bool IsOverArea(Transform area)
    {
        if(area == null)
        {
            return false;
        }
        
        //영역의 콜라이더를 가져옴
        Collider2D areaCollider = area.GetComponent<Collider2D>();
        if (areaCollider == null)
            return false;

        return areaCollider.bounds.Contains(transform.position);
    }

}
