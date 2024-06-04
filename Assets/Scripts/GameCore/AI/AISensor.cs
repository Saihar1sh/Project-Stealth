using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    public float distance = 10f;

    [Range(0, 180f), Tooltip("Field of view Angle")]
    public float angle = 30f;

    public float height = 1f;
    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask visibleLayers;
    public LayerMask occlusionLayers;

    public List<GameObject> ObjectsInFOV
    {
        get
        {
            objectsInFOV.RemoveAll(obj => !obj);
            return objectsInFOV;
        }
    }

    private Collider[] colliders = new Collider[50];
    private List<GameObject> objectsInFOV = new List<GameObject>();
    private Mesh mesh;
    private int count;
    private float scanInterval;
    private float scanTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        scanTimer = 0;
        scanInterval = (float)1 / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            ScanSurroundings();
        }
    }

    private void ScanSurroundings()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, visibleLayers,
            QueryTriggerInteraction.Collide);

        objectsInFOV.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                objectsInFOV.Add(obj);
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 destination = obj.transform.position;
        Vector3 direction = destination - origin;

        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        destination.y = origin.y;

        if (Physics.Linecast(origin, destination, occlusionLayers))
        {
            return false;
        }

        return true;
    }

    Mesh CreatWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vertexIndex = 0;

        //left
        vertices[vertexIndex++] = bottomCenter;
        vertices[vertexIndex++] = bottomLeft;
        vertices[vertexIndex++] = topLeft;

        vertices[vertexIndex++] = topLeft;
        vertices[vertexIndex++] = topCenter;
        vertices[vertexIndex++] = bottomCenter;

        //right side

        vertices[vertexIndex++] = bottomCenter;
        vertices[vertexIndex++] = topCenter;
        vertices[vertexIndex++] = topRight;

        vertices[vertexIndex++] = topRight;
        vertices[vertexIndex++] = bottomRight;
        vertices[vertexIndex++] = bottomCenter;

        float currentAngle = -angle;
        float angleDelta = (angle * 2) / segments;

        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + angleDelta, 0) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;


            //far side;
            vertices[vertexIndex++] = bottomLeft;
            vertices[vertexIndex++] = bottomRight;
            vertices[vertexIndex++] = topRight;

            vertices[vertexIndex++] = topRight;
            vertices[vertexIndex++] = topLeft;
            vertices[vertexIndex++] = bottomLeft;

            //top
            vertices[vertexIndex++] = topCenter;
            vertices[vertexIndex++] = topLeft;
            vertices[vertexIndex++] = topRight;

            //bottom    
            vertices[vertexIndex++] = bottomCenter;
            vertices[vertexIndex++] = bottomLeft;
            vertices[vertexIndex++] = bottomRight;

            currentAngle += angleDelta;
        }

        for (int i = 0; i < numVertices; i++)
        {
            if (!triangles.Contains(i))
                triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreatWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        Gizmos.color = Color.red;
        for (int i = 0; i < count; ++i)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, .2f);
        }

        Gizmos.color = Color.green;
        foreach (var item in objectsInFOV)
        {
            Gizmos.DrawSphere(item.transform.position, .2f);
        }
    }
}