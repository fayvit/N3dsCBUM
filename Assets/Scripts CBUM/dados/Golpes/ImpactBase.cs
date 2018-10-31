using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImpactBase:IGolpeBase
{
    public float TempoHit { get; set; }
    public float ExitHitTime { get; set; }

    public string NomeAnimaGolpe { get; set; }
    public string IDAnimaGolpe { get; set; }

    public GolpeId ID { get; protected set; }
}