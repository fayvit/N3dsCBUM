using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedrasDestrutiveisIniciais : IDestructible {

    private MeshRenderer M;
    //PUN private PhotonView myView;
	// Use this for initialization
	void Start () {
        EventAgregator.AddListener(EventKey.destructibleDamageColliderContact, InicieDestruicao);
        M = GetComponent<MeshRenderer>();
      //PUN  myView = GetComponent<PhotonView>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void TransmutarMaterial()
    {
        int pv = Dados.PontosDeVida;
        int mPv = Dados.MaxVida;
        if (pv > 0.6f*mPv && pv <= 0.9f*mPv)
        {
            M.material = (Material)Resources.Load("ground 1");
        }
        else if (pv <= 0.6f*mPv && pv > 0.3f*mPv)
        {
            M.material = (Material)Resources.Load("ground 2");
        }
        else if (pv <=0.3f*mPv)
        {
            M.material = (Material)Resources.Load("ground 3");
        }
    }

    public override void InicieDestruicao(IGameEvent e)
    {
        if (e.Sender == gameObject)
        {
            DestructibleColliderContactEvent d = (DestructibleColliderContactEvent)e;

            /* PUN
            if (PhotonNetwork.isMasterClient)
            {*/
                Dados.AplicaDano(d.Golpe.ValorDeDano);
            if (Dados.PontosDeVida > 0)
                //PUN  PhotonNetwork.RPC(myView, "AtualizaDano", PhotonTargets.Others, false, Dados.PontosDeVida);
                AtualizaDano(Dados.PontosDeVida);
            else
                RPC_Listener.ElementosDaDestruicaoRockCube(transform.position);
                //PUN RPC_Listener.RPC(NameOfRPC.ElementosDaDestruicaoRockCube, PhotonTargets.Others, transform.position);
          //  }

            if (Dados.PontosDeVida > 0)
            {

                ElementosDoImpacto();
            }
            else
            {
                ElementosDaDestruicao(transform.position);

                //  if (PhotonNetwork.isMasterClient)
                // {
                //PUN PhotonNetwork.Destroy(gameObject);
                Destroy(gameObject);
                //}
            }
        }
    }

    
    public static void ElementosDaDestruicao(Vector3 pos)
    {
        Destroy(Instantiate((GameObject)Resources.Load("rockDestroyedSound")), 2);

        Destroy(Instantiate((GameObject)Resources.Load("poeiraDeImpacto"), pos, Quaternion.identity), 2);

        Destroy(Instantiate((GameObject)Resources.Load("pedrinhasDaDestruicao"), pos, Quaternion.identity), 2);

    }

    void ElementosDoImpacto()
    {
        Destroy(Instantiate((GameObject)Resources.Load("rockDestructibleSound")), 2);

        Destroy(Instantiate((GameObject)Resources.Load("poeiraDeImpacto"), transform.position, Quaternion.identity), 2);

        if (Dados.PontosDeVida % 3 == 0)
            Destroy(Instantiate((GameObject)Resources.Load("pedrinhasDoImpacto"), transform.position, Quaternion.identity), 2);

        TransmutarMaterial();
    }

    //[PunRPC]
    void AtualizaDano(int vida)
    {
        Dados.PontosDeVida = vida;
        Debug.Log(message: "sou um cliente tentando alternar o material: " + Dados.PontosDeVida + " : "+vida);
       // ElementosDoImpacto();

    }

    private void OnDestroy()
    {
        
        EventAgregator.RemoveListener(EventKey.destructibleDamageColliderContact, InicieDestruicao);
    }
}
