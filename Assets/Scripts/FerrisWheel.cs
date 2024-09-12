using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.rotation = Quaternion.identity;
        }
    }
}
