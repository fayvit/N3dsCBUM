using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Coletavel : MonoBehaviour {

    [SerializeField] private int desligarSeNumeroDeJogadoresMenorQue = 0;

    //PUN   public abstract void ElementosDoColetavel(PhotonView P);

    public abstract void ElementosDoColetavel(GameObject G);

    protected virtual void Start()
    {
        EventAgregator.AddListener(EventKey.novoHeroiSpawnado, OnNewHerospawned);
    }

    protected virtual void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.novoHeroiSpawnado, OnNewHerospawned);
    }

    private void OnNewHerospawned(IGameEvent obj)
    {
        if (GlobalController.g.Jogadores.Count < desligarSeNumeroDeJogadoresMenorQue)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "hitCollider"/* && PhotonNetwork.isMasterClient*/)
        {
            ColisorDeDano c = other.GetComponent<ColisorDeDano>();
            /*PUN PhotonView doColetor = c.Dono.GetComponent<PhotonView>();

            RPC_Listener.RPC(NameOfRPC.PegueColetavel, 
                PhotonTargets.Others, 
                GetComponent<PhotonView>().viewID,
                doColetor.viewID
                );*/

            RPC_Listener.PegueColetavel(
                gameObject,
                c.Dono
                );
            ElementosDoColetavel(c.Dono);
            
        }
    }
}
