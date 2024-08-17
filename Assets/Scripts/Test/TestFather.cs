using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFather : MonoBehaviour
{
    protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
}
