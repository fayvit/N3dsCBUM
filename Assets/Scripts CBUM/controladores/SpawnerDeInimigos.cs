using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDeInimigos : MonoBehaviour {

    [SerializeField] private GameObject[] osSpawnaveis;
    [SerializeField] private Transform xMinZmin;
    [SerializeField] private Transform xMaxZmax;
    [SerializeField] private float intervaloDeSpawn = 5;
    [SerializeField] private int multiplicadorDeSpawnados = 2;
    [SerializeField] private int maxSpawnaveisInMoment = 6;
    [SerializeField] private float distanciaMin = 1;
    

    private float tempoDecorrido = 0;
    private float distanciaMax = 1;

    // Use this for initialization
    private void Start () {
        //if (PhotonNetwork.isMasterClient)
        //{
            distanciaMax = Mathf.Max(
                Vector3.Distance(transform.position, xMinZmin.position),
                Vector3.Distance(transform.position, xMinZmin.position),
                distanciaMin
                );

            SpawnaInimigos();
       // }
	}

    // Update is called once per frame
    void Update()
    {
       // if (PhotonNetwork.isMasterClient)
        //{
            tempoDecorrido += Time.deltaTime;

            if (tempoDecorrido > intervaloDeSpawn)
            {
                SpawnaInimigos();
                tempoDecorrido = 0;
            }
       // }

        transform.Rotate(new Vector3(0,1,0));
	}

    public void SpawnaInimigos()
    {
        int numSpawnados;

        if (GlobalController.g.Jogadores != null)
            numSpawnados = Mathf.Min(multiplicadorDeSpawnados * GlobalController.g.Jogadores.Count, maxSpawnaveisInMoment);
        else
            numSpawnados = maxSpawnaveisInMoment;

        Destroy(
        Instantiate(PrefabList.Load(PrefabListName.spawnerParticles), transform.position,Quaternion.identity),3);

        for (int i = 0; i < numSpawnados; i++)
        {
            int qual = Random.Range(0, osSpawnaveis.Length);
            Vector3 pos = SelecionaPosicao();


            //PUN GameObject G = PhotonNetwork.Instantiate(osSpawnaveis[qual].name,pos,Quaternion.identity,0);
            GameObject G = Instantiate(osSpawnaveis[qual], pos, Quaternion.identity);

            SpawnedEnemy(G, pos);

            EventAgregator.Publish(EventKey.addEnemyInManager, 
                new AddEnemyInManagerEvent(gameObject, new EnemyBase[1] { G.GetComponent<EnemyBase>()}));

            //PUN RPC_Listener.RPC(NameOfRPC.SpawnedEnemy, PhotonTargets.Others,G.GetComponent<PhotonView>().viewID,pos);
            RPC_Listener.SpawnedEnemy(G, pos);
        }
    }

    public static void SpawnedEnemy(GameObject G,Vector3 pos)
    {
        GameObject v =
                (GameObject)Instantiate(PrefabList.Load(PrefabListName.particulaDoCaraAparecendo), pos, Quaternion.identity);

        v.GetComponent<VisualizacaoDoInimigoSpawnando>().OSpawnado = G;

        G.SetActive(false);
    }

    Vector3 SelecionaPosicao()
    {
        float distancia = Random.Range(distanciaMin, distanciaMax);
        Vector2 dir = Random.insideUnitCircle;
        Vector3 myDir = new Vector3(dir.x, 0, dir.y);
        Vector3 pos = transform.position + distancia * myDir;

        pos = new Vector3(
            Mathf.Clamp(pos.x,xMinZmin.position.x,xMaxZmax.position.x),
            0,
            Mathf.Clamp(pos.z, xMinZmin.position.z, xMaxZmax.position.z)
            );

        return pos;
    }

    public void OnDestroy()
    {
        EventAgregator.Publish(new StandardGameEvent(gameObject, EventKey.enemyDefeated));
    }
}
