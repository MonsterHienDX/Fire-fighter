using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCurveMapDir : MonoBehaviour
{
    private WorldCurver curveShader;

    [SerializeField] private float minTimeChangeDir;
    [SerializeField] private float maxTimeChangeDir;

    [SerializeField] private MapDirection mapDirection = MapDirection.Straight;

    [SerializeField] private float minHorizontalRatio;
    [SerializeField] private float maxHorizontalRatio;
    [SerializeField] private float minVerticalRatio;
    [SerializeField] private float maxVerticalRatio;

    void Start()
    {
        curveShader = GetComponent<WorldCurver>();
    }



}
