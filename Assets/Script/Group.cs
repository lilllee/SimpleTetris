using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Group : MonoBehaviour
{
    float lastFall = 0;
    float clickTime;
    float clickTimes = 2f;
    bool firstState = false;
 //   public int count = 0;
   // public Text countText;
    bool isValidGridPos() //블록들이 Border안에 있나? 블록들이 있는 grid가 비어 있나?(다른 블록과 겹치지 않나?)
    {
        foreach (Transform child in transform)   //나의 
        {
            Vector2 v = Grid.roundVec2(child.position); //

            if (!Grid.insideBoard(v))
                return false;

            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
      

        }
        return true;
    }
    //이동/회전/낙하   Group이 이동하면 자신의 child를 모두 삭제 업데이트 된 새로운 위치에 child를 넣어줌
    void updateGrid()
    {
        for (int y = 0; y < Grid.h; ++y)
            for (int x = 0; x < Grid.w; ++x)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == transform)
                        Grid.grid[x, y] = null;
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.roundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }
    void Start()
    {
        if (!isValidGridPos())
        {
          //  GetComponent<Gameover>().SendMessage("Fuck", true);
            //Destroy(gameObject);
        }
    }
    IEnumerator EasyTouch()
    {
        firstState = true;
        if (Input.GetMouseButton(0))
        {
            yield return new WaitForSeconds(2f);  //2초 동안 기다리면 
            clickTime += clickTimes;              //clickTime 값이 2
            Vector3 saveFirstPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,Input.mousePosition.z);  //현재 마우스 위치를 저장
            Vector3 movePoint = Camera.main.WorldToScreenPoint(saveFirstPosition);                                        //값을 받았다잇 
          
            //값을 저장 시켜야 한다. 그게 아니면 
        }
        if (Input.GetMouseButtonUp(0) && clickTime == 2) 
        {
            
        }
        
        //마우스 버튼 올렸을때 2초 동안 움직인 값과 전 값을 비교해서 오른쪽인지 왼쪽인지 확인

        firstState = false;      //다시 false로 바꾼다.
    }
    void Update()
    {
            if (!firstState)
            {
                StartCoroutine(EasyTouch());
            }  
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            if (isValidGridPos())
                updateGrid();
            else
                transform.position += new Vector3(1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (isValidGridPos())
                updateGrid();
            else
                transform.position += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            if (isValidGridPos())
                updateGrid();
            else
                transform.Rotate(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)|| Time.time - lastFall >= 1)
        {
            Down();
        }
        /*함수로 만들어서 내려주는데 아래 아무것도 없을때 마다 계속 내려준다. 그러면 그리드의 y축이 0이 될때 까지 내리면 된다 
         * 만약 미리 블록이 존재한다면? isRowFull 함수
           줄이 (블록으로) 가득 찼는지 체크
          하나라도 없으면(null) false


           */
    }
    private void Down()
    {
        transform.position += new Vector3(0, -1, 0); //y축으로 -1
        if (isValidGridPos())
        {
            updateGrid();
        }
        else
        {
            transform.position += new Vector3(0, 1, 0);
            Grid.deleteFullRows();
          //  count += 10;    //10점씩 증가.
          //  countText.text = "Score" + count;
            FindObjectOfType<Controller>().nextStart();
            enabled = false;
        }
        lastFall = Time.time;
    }
}
