using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase
{
    private Animator animator;

    public Animator Animador
    {
        get { return animator; }
        protected set { animator = value; }
    }

    protected IEnumerator ColocaHitCollider(IGolpeBase esseGolpe)
    {
        yield return new WaitForSeconds(esseGolpe.TempoDeAtrasoParaCollider);
        byte[] golpe = BytesTransform.ToBytes(esseGolpe);

        RPC_Listener.Coloca_HitCollider(golpe, Animador.gameObject);
        /*PUN
        RPC_Listener.RPC(NameOfRPC.Coloca_HitCollider,
            PhotonTargets.All,
            golpe,
            Animador.GetComponent<PhotonView>().viewID);
            */
    }
}