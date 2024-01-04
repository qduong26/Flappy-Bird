using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public FlappyControl flappyControl;
    public bool isGamePlay;

    public void Awake()
    {
        instance = this;
        isGamePlay = false;
    }
   
    
}
