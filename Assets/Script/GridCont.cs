using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridCont : MonoBehaviour
{
    public static int w = 10;
    public static int h = 20;
    float lastFall = 0;

    public static Transform[,] grid = new Transform[w, h]; //이차원 배열 선언

    private void Start()
    {
        if (!isValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }
    public static Vector2 roundVec2(Vector2 v)
    {

        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }
    public static bool insideBoard(Vector2 pos)        //좌표값안에 게임 영역이 있는지 확인
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < w &&
                (int)pos.y >= 0
               );
    }
    public static void deleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            Destroy(grid[x, y].gameObject); //한줄을 모두 지워주고 
            grid[x, y] = null;              //그리드를 null로 바꿔줌
        }
    }
    public static void decreaseRow(int y) //그리드 상에서 블록을 한 줄씩 내려줌                                      
    {                                     //실제 블록의 위치도 내려줌
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                //Move one towards bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                //UPdate Block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }
    public static void decreaseRowsAbove(int y) //y축 이상 모두 한칸씩 내려줌
    {
        for (int i = y; i < h; ++i)
            decreaseRow(i);
    }
    public static bool isRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }
    public static void deleteFullRows()
    {
        for (int y = 0; y < h; ++y)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                decreaseRowsAbove(y + 1);
                --y;

            }
        }
    }
    bool isValidGridPos() // 블록들이 Border안에 있나? 블록들이 있는 grid가 비어 있나?(다른 블록과 겹치지 않나?)
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.roundVec2(child.position);

            if (!Grid.insideBoard(v))
            {
                return false;

                if (Grid.grid[(int)v.x, (int)v.y] != null &&
                    Grid.grid[(int)v.x, (int)v.y].parent != transform)
                    return false;
                /** 같은 Group의 블록끼리는 겹침 허용
                    Grid.grid[(int)v.x, (int)v.y].parent != transform

                 */
            }
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

                        foreach (Transform child in transform)
                        {
                            Vector2 v = Grid.roundVec2(child.position);
                            Grid.grid[(int)v.x, (int)v.y] = child;
                        }
    }
    void Update()
    {
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
            transform.position += new Vector3(-1, 0, 0);

            if (isValidGridPos())
                updateGrid();
            else
                transform.position += new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            if (isValidGridPos())
                updateGrid();
            else
                transform.Rotate(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, -1, 0);
            if (isValidGridPos())
            {
                updateGrid();
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                Grid.deleteFullRows();
                FindObjectOfType<Controller>().nextStart();
                enabled = false;
            }
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow)|| Time.time - lastFall >= 1)
        {
            transform.position += new Vector3(0, -1, 0);
            if (isValidGridPos())
            {
                updateGrid();
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                Grid.deleteFullRows();
                FindObjectOfType<Controller>().nextStart();
                enabled = false;
            }
            lastFall = Time.time;
        }
    }

}
