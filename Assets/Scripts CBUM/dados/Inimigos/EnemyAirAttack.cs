using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirAttack : EnemyBase
{
    [SerializeField] private float disRevide = 4;
    [SerializeField] private float disPosAtk = 3;
    // Use this for initialization
    protected override void Start()
    {
        ConstruirIa(new EnemyIaAirAttack(gameObject, carac, disRevide, disPosAtk));

        base.Start();
    }

    // Update is called once per frame[usando o base]
    //void Update () {

    //}
}
