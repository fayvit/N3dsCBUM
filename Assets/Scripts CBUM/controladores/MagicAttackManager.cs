using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackManager
{
    private MagicBase essegolpe = new WaterMagic();
    private Animator animador;
    private int viewID = 0;

    private float tempoDecorrido = 0;
    private bool instanciou = false;
    

    public MagicAttackManager(Animator A,int viewID)
    {
        this.viewID = viewID;
        tempoDecorrido = 0;
        animador = A;
    }

    public MagicBase Essegolpe
    {
        get { return essegolpe; }
        set { essegolpe = value; }
    }

    public bool Update()
    {
        tempoDecorrido += Time.deltaTime;
        if (tempoDecorrido > essegolpe.TempoDeAtrasoParaCollider && !instanciou)
        {
            essegolpe.DirDeREpulsao = animador.transform.forward;
            instanciou = true;
            //PUN RPC_Listener.RPC(NameOfRPC.Coloca_ColisorDeMagia, PhotonTargets.All, BytesTransform.ToBytes(essegolpe), viewID);
            RPC_Listener.Coloca_ColisorDeMagia(BytesTransform.ToBytes(essegolpe), animador.gameObject);
        }

        if (tempoDecorrido > essegolpe.TempoDeAnima)
        {
            instanciou = false;
            tempoDecorrido = 0;
            return true;
        }
        

        return false;
    }

    public static void ColocaColisorDeMagia(byte[] bGolpe,GameObject dono)
    {
        //GameObject dono = PhotonView.Find(viewID).gameObject;
        IGolpeBase golpe = BytesTransform.ToObject<IGolpeBase>(bGolpe);

        float prolongue = golpe.TempoDeVidaDoCollider;
        float avante = golpe.DeslocamentoDoColisor;

        GameObject G = MonoBehaviour.Instantiate(
                    (GameObject)Resources.Load("ColisorDeMagia"),
                    dono.transform.position + 1.4f * avante * dono.transform.forward,
                    Quaternion.LookRotation(dono.transform.forward));
        ColisorDeDano col = G.transform.GetChild(0).gameObject.AddComponent<ColisorDeDano>();

        col.EsseGolpe = golpe;
        col.Dono = dono;

        G.GetComponent<ColisorDePoder>().Direcao = golpe.DirDeREpulsao;

        MonoBehaviour.Destroy(G, prolongue);
    }
}