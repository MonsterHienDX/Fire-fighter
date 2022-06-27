using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTruck : SingletonMonobehaviour<FireTruck>
{
    private Vector3 startPoint;
    private Vector3 endPoint;
    [SerializeField] private float moveSpeed;


    public void Init()
    {

    }


    void Start()
    {

    }

    void Update()
    {
        if (GameManager.instance.isPlaying)
            transform.Translate(endPoint.normalized * moveSpeed * Time.deltaTime);
    }
}
