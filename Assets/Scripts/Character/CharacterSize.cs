using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSize : MonoBehaviour
{
    [Range(0.01F, 0.1F)] public float sizeFactor = 0.05F;

    Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void Update()
    {
        transform.localScale = new Vector3(
            startScale.x - (sizeFactor * transform.position.y),
            startScale.y - (sizeFactor * transform.position.y),
            transform.localScale.z);
    }
}
