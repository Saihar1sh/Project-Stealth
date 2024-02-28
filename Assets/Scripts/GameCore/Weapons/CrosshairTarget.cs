using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{
    private Camera mainCamera;
    private Ray _ray;
    RaycastHit hitInfo;
    
    public bool CanFire { get;private set; }
    
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
        _ray.origin = mainCamera.transform.position;
        _ray.direction = mainCamera.transform.forward;
        CanFire = Physics.Raycast(_ray, out hitInfo);
        transform.position = hitInfo.point;
    }
}
