using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{
    private Camera mainCamera;
    private Ray _ray;
    RaycastHit hitInfo;
    void Awake()
    {
        mainCamera = Camera.main;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
