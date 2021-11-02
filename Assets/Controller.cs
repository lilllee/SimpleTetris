using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject[] group;

    // Start is called before the first frame update
    void Start()
    {
        nextStart();
    }

   public void nextStart()
    {
        int i = Random.Range(0, group.Length);  //0부터 배열의 길이까지 랜덤으로 i 값에 넣는다.
        Instantiate(group[i], transform.position, Quaternion.identity);  //배열에 있는 오브젝트를 생성
    }
}
