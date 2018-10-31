using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisorDePoder : MonoBehaviour {

    private Vector3 direcao;

    [SerializeField]private GameObject particle;
    [SerializeField] private float vel = 5;
    float tempoDecorrido = 0;
    float tempoDeInstance = 0.15f;

    public Vector3 Direcao
    {
        get { return direcao; }
        set { direcao = value; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		tempoDecorrido+=Time.deltaTime;

        transform.position += Direcao * Time.deltaTime*vel;

        if (tempoDecorrido > tempoDeInstance)
        {
            tempoDecorrido = 0;
            Vector2 dir = Random.insideUnitCircle;

            Vector3 pos = transform.position + new Vector3(dir.x ,0 ,dir.y) * 1f+Vector3.down*0.5f;

            pos = AuxiliarDeInstancia.NovaPos(pos, 1);

            pos += Vector3.up;
            Destroy(Instantiate(particle,pos,Quaternion.identity),5);
        }
	}
}
