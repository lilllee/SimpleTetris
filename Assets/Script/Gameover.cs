using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameover : MonoBehaviour
{
    public Image image;
    // Update is called once per frame
      void Fuck(bool yes) { 
        if (yes == true)
        {
            image.gameObject.SetActive(true);
        }
    }

}
