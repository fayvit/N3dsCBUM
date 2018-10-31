using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisorDeProjetil : ColisorDeDano {

    private float tempoDecorrido = 0;

    // Use this for initialization
    void Start () {
        /*if (!PhotonNetwork.isMasterClient)
            enabled = false;*/
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += EsseGolpe.DirDeREpulsao * EsseGolpe.VelocidadeDeRepulsao*Time.deltaTime;

        tempoDecorrido += Time.deltaTime;

        if (tempoDecorrido > EsseGolpe.TempoDeVidaDoCollider)
            //PUN PhotonNetwork.Destroy(gameObject);
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (/*PUN PhotonNetwork.isMasterClient && */other.gameObject.layer==11)
            FacaImpacto(other.gameObject);
    }
}
