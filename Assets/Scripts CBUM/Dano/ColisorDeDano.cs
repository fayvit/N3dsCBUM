using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisorDeDano : MonoBehaviour {

    private GameObject dono;
    private IGolpeBase esseGolpe;
    private List<GameObject> listaDeColididos = new List<GameObject>();

    public IGolpeBase EsseGolpe
    {
        get { return esseGolpe; }
        set { esseGolpe = value; }
    }

    public GameObject Dono
    {
        get { return dono; }
        set { dono = value; }
    }

    protected void FacaImpacto(GameObject emQ, bool destroiAqui = true, bool noTransform = false)
    {
        if (VerificaMultiHit(emQ))
        {
            bool foi = false;
            
            if (emQ.tag == dono.tag && esseGolpe.DanoEmAliado)
                foi = true;
            else if (emQ.tag != dono.tag)
                foi = true;

            if (foi)
            {
             //   if (PhotonNetwork.isMasterClient)
               // {
                    if (!noTransform)
                    {
                        /*O Gamecontroller ouve esse evento para inserir as particulas*/
                        EventAgregator.Publish(EventKey.globalDamageColliderContact,
                            new StandardSendStringEvent(
                                emQ, EsseGolpe.NoImpacto.ToString(),
                                EventKey.globalDamageColliderContact
                                ));
                    }

                    Dano.VerificaDano(emQ, Dono, EsseGolpe);
                // }

                /* PUN
                if ((dono.GetComponent<PhotonView>().isMine && dono.tag == "Player")
                    ||
                    (emQ.GetComponent<PhotonView>().isMine && emQ.tag == "Player"))
                    EventAgregator.Publish(EventKey.localDamageColliderContact, null);/* A camera ouve esse evento para tremer*
                    */
                if (dono.tag == "Player" || emQ.tag == "Player")
                    EventAgregator.Publish(EventKey.localDamageColliderContact, null);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if ((other.tag == "Enemy" || other.tag == "Player") && other.gameObject != dono)
            {
                //if(VerificaMultiHit(other.gameObject))
                FacaImpacto(other.gameObject);            
            }

            if (other.tag == "Destructible")
                EventAgregator.Publish(EventKey.destructibleDamageColliderContact,
                    new DestructibleColliderContactEvent(other.gameObject, esseGolpe));
            /* evento ouvido pelo gameObject tocado */
                
        }
    }

    bool VerificaMultiHit(GameObject G)
    {
        if (EsseGolpe.BloquearMultHit)
        {
            if (listaDeColididos.Contains(G))
                return false;
            else
            {
                listaDeColididos.Add(G);
                return true;
            }
        }
        else return true;
    }
}
