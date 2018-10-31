using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjetilRefinedBase : GolpeRefinadoBase {

    protected string nomeProjetil;

    public override void StartAttack(GameObject G, Transform alvo)
    {
        TempoDecorrido = 0;
        //golpeAtual = golpe;
        dono = G;
        dono.transform.rotation = Quaternion.LookRotation(
                            Vector3.Dot(Vector3.right, alvo.transform.position - dono.transform.position) > 0
                            ? Vector3.right : Vector3.left);

        EventAgregator.Publish(new StandardSendStringEvent(G, NomeAnima, EventKey.startRefinedAttack));

        DirDeREpulsao = G.transform.forward;

        /*PUN
        GameObject g = PhotonNetwork.Instantiate(nomeProjetil,
            G.transform.position + G.transform.forward + Vector3.up * 0.75f,
            Quaternion.LookRotation(G.transform.forward), 0
            );*/
        GameObject g = MonoBehaviour.Instantiate((GameObject)Resources.Load(nomeProjetil),
        G.transform.position + G.transform.forward + Vector3.up * 0.75f,
        Quaternion.LookRotation(G.transform.forward));
        ColisorDeProjetil cp = g.GetComponent<ColisorDeProjetil>();
        cp.EsseGolpe = this;
        cp.Dono = G;
    }

    public override bool UpdateAttack()
    {
        TempoDecorrido += Time.deltaTime;

        if (TempoDecorrido > TempoEmAnima)
        {
            EventAgregator.Publish(new StandardGameEvent(dono, EventKey.endRefinedAttack));
            return true;
        }

        return false;
    }
}
