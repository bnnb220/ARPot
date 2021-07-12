using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSceneManager : MonoBehaviour
{
    ARSceneManager instance;

    private List<GameObject> targetObjList = new List<GameObject>();

    private GameObject selectedTarget;

    private Vector2 camCenterPos;

    private Color tpColor;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

    }
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
