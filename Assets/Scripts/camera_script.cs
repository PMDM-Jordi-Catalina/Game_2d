using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject heroe;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.x = heroe.transform.position.x;
        position.y = heroe.transform.position.y;
        transform.position = position;
    }
}
