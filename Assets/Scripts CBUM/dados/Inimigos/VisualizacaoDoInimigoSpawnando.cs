using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizacaoDoInimigoSpawnando : MonoBehaviour {

    [SerializeField] private float tempoDeVisualizacao = 3;
    [SerializeField] private GameObject oSpawnado;

    public GameObject OSpawnado
    {
        get { return oSpawnado; }
        set { oSpawnado = value; }
    }

    // Use this for initialization
    void Start () {
        Invoke("LigaSpawnado", tempoDeVisualizacao);
	}

    void LigaSpawnado()
    {
        if (oSpawnado != null)
        {
            
            oSpawnado.SetActive(true);

            //if(PhotonNetwork.isMasterClient)
                oSpawnado.GetComponent<EnemyBase>().IniciarIa();

            //RPC_Listener.RPC(NameOfRPC.EnableSpawnedEnemy, PhotonTargets.Others, E.MyView.viewID);
        }
        else
            Debug.Log("A visualização de Spawnado não encontrou o gameObject");

        Destroy(
        Instantiate(PrefabList.Load(PrefabListName.spawnerParticles), transform.position, Quaternion.identity),3);
        Destroy(gameObject);
    }


    

    // Update is called once per frame
    void Update () {
		
	}
}
