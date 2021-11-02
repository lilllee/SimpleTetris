using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid : MonoBehaviour
{
    public static int w = 10;
    public static int h = 20;
    public static int count;


    public static Transform[,] grid = new Transform[w, h]; //이차원 배열 선언



    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }
    public static bool insideBoard(Vector2 pos)        // 좌표값안에 게임 영역이 있는지 확인
    {
        return (
                (int)pos.x >= 0 &&
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
    public static void deleteFullRows()    //줄이 가득찼으면 그줄을 지우고 위쪽 블록들을 모두 한줄씩 내림 이때 점수를 주면 되겠지?
    {
        for (int y = 0; y < h; ++y)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                decreaseRowsAbove(y + 1);
                --y;
                count += 10;
                
            }
        }
    }



}
