using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImpactoRefinadoBase : GolpeRefinadoBase
{
    [System.NonSerialized] protected CharacterController controle;

    protected bool addView = false;
    protected float VelocidadeDeGolpe { get; set; }
    protected PrefabListName prefabName = PrefabListName.tapaCollider;

    protected void ReiniciaAtualizadorDeImpactos()
    {
        TempoDecorrido = 0;
        addView = false;
    }

    public override void StartAttack(GameObject G, Transform alvo)
    {
        dono = G;
        controle = G.GetComponent<CharacterController>();

        ReiniciaAtualizadorDeImpactos();

        EventAgregator.Publish(new StandardSendStringEvent(G, NomeAnima, EventKey.startRefinedAttack));
    }

    public override bool UpdateAttack()
    {
        TempoDecorrido += Time.deltaTime;

        if (TempoDecorrido > TempoDeMoveMin)
        {
            UpdateGeral();
        }
        if (TempoDecorrido > TempoEmAnima)
        {
            if (TempoDecorrido > TempoDeVidaDoCollider)
            {
                EventAgregator.Publish(new StandardGameEvent(dono, EventKey.endRefinedAttack));
                return true;
            }
        }
        return false;
    }

    void UpdateGeral()
    {

        if (!addView)
        {
            TempoDecorrido = TempoDeMoveMin;
            DirDeREpulsao = dono.transform.forward;
            ColocaMeuHitCollider(dono);

            /*PUN
            RPC_Listener.RPC(NameOfRPC.AddRefinedCollider, PhotonTargets.All,
                dono.GetComponent<PhotonView>().viewID,
                BytesTransform.ToBytes(this)
                );*/

            RPC_Listener.AddRefinedCollider(dono,BytesTransform.ToBytes(this));

            addView = true;

        }

        if (TempoDecorrido < TempoEmAnima)
        {
            DirDeREpulsao = dono.transform.forward;

            if (!controle)
                controle = dono.GetComponent<CharacterController>();
            controle.Move(VelocidadeDeGolpe * dono.transform.forward * Time.deltaTime + Vector3.down * 9.8f);
        }
    }

    public override void ColocaMeuHitCollider(GameObject dono)
    {
        GameObject G = MonoBehaviour.Instantiate(
                    (GameObject)PrefabList.Load(prefabName),
                    dono.transform.position + 1.4f * dono.transform.forward + Vector3.up,
                    Quaternion.LookRotation(dono.transform.forward, Vector3.Cross(Camera.main.transform.forward, dono.transform.forward)));
        ColisorDeDano col = G.transform.GetChild(0).gameObject.AddComponent<ColisorDeDano>();

        G.transform.parent = dono.transform;
        col.EsseGolpe = this;
        col.Dono = dono;
        MonoBehaviour.Destroy(G, TempoDeVidaDoCollider);
    }
}
