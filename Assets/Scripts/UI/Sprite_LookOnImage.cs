using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_LookOnImage : MonoBehaviour
{
    [SerializeField]
    private Sprite _sprite = null;

    [SerializeField]
    private bool _isLookOn=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(Camera.main.gameObject.transform);
    }
}
