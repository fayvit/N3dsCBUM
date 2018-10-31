using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIruin : EnemyRefinedBase
{

    private void Awake()
    {
        ParaAwake();
    }

    // Use this for initialization
    protected override void Start()
    {
        ConstruirIa(new EnemyIaIruin(gameObject, carac));

        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
