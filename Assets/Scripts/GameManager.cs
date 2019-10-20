using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//進行管理クラス
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        InputController.SetDefault();

    }

    // Update is called once per frame
    void Update()
    {
        InputController.Update();
    }
}
