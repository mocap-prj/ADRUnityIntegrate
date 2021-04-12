using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public int ID;
    public Vector3 trackPos;
    public Quaternion trackRot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = trackPos;
        this.transform.rotation = trackRot;
    }
}
