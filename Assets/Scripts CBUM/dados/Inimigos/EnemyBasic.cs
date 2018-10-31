using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : EnemyBase {

	// Use this for initialization
	protected override void Start () {

        ConstruirIa(new EnemyIaBasic(gameObject, carac));

        base.Start();
    }
	
	// Update is called once per frame[usando o base]
	//void Update () {
		
	//}
}
