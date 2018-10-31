using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyXuash : EnemyRefinedBase {

    private void Awake()
    {
        ParaAwake();
    }

    // Use this for initialization
    protected override void Start()
    {
        ConstruirIa(new EnemyIaXuash(gameObject, carac));

        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // Update is called once per frame [usando da clase base]
    //void Update () {

    //}
}
