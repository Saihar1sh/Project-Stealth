using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    Material material;

    Color colorStart;
    Color colorEnd;
    float rate = 1;  // Number of times per second new colour is chosen
    float i = 0; // Counter to control lerp

    void Update()
    {
        // Blend towards the current target colour
        i += Time.deltaTime * rate;
        material.color = Color.Lerp(colorStart, colorEnd, i);

        // If we've got to the current target colour, choose a new one
        if (i >= 1)
        {
            i = 0;
            colorStart = material.color;
            colorEnd = new Color(Random.value, Random.value, Random.value);
        }
    }
}
