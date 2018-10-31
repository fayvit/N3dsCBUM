using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController g;
    [SerializeField] private HudContainer hudM;

    /* PUN
    private PhotonView myView;

    public PhotonView MyView
    {
        get { return myView; }
    }*/

    public HudContainer HudM
    {
        get { return hudM; }
        private set { hudM = value; }
    }

    private void Awake()
    {
        if (g == null)
            g = this;

       // myView = GetComponent<PhotonView>();
    }

    // Use this for initialization
    void Start()
    {
        GameController[] g = FindObjectsOfType<GameController>();

        if (g.Length>1)
            Destroy(gameObject);

        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        EventAgregator.AddListener(EventKey.globalDamageColliderContact, OnDamageColliderContact);

    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.globalDamageColliderContact, OnDamageColliderContact);
    }

    void OnDamageColliderContact(IGameEvent e)
    {
        StandardSendStringEvent d = (StandardSendStringEvent)e;
        ColocaParticulas(d.StringContent, d.Sender.transform.position);
        RPC_Listener.ColocaParticulaNosClientes(
            d.StringContent,
            d.Sender.transform.position
            );

        /* PUN
        RPC_Listener.RPC(
            NameOfRPC.ColocaParticulaNosClientes,
            PhotonTargets.Others,
            d.StringContent,
            d.Sender.transform.position
            );*/
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public static void ColocaParticulas(string nomeGolpe, Vector3 position)
    {
        GameObject G =
        Instantiate((GameObject)Resources.Load(nomeGolpe), position, Quaternion.identity);
       
        GameObject[] GS = GameObject.FindGameObjectsWithTag("cenario");
        int indice = 0;
        foreach (GameObject g in GS)
            if (g.layer == 9)
            {
                G.GetComponent<ParticleSystem>().collision.SetPlane(indice, g.transform);
                indice++;
            }

        Destroy(Instantiate((GameObject)PrefabList.Load(PrefabListName.hitSound)), 3);

        Vector2 dir = Random.insideUnitCircle;
        position += new Vector3(dir.x, 1.5f, dir.y);
        Destroy(Instantiate((GameObject)PrefabList.Load(PrefabListName.impactoPositivo), position, Quaternion.identity), 3);

        Destroy(G, 2);
    }

    void QueroACena(Scene c, LoadSceneMode l)
    {
        if (c.name == "testeMapa")
        {
            SceneManager.SetActiveScene(c);
            SceneManager.UnloadSceneAsync("SampleScene");
            //PhotonNetwork.automaticallySyncScene = true;
        }
    }
    public void ColocandoPlayersNaCena()
    {
        int numJ = 0;
        SloteMultiplayer[] jogadores = GlobalController.g.Jogadores.ToArray();


        GameObject G = Instantiate(Resources.Load("Ink"), new Vector3(1105, 2, 44), Quaternion.identity) as GameObject;
        
                Controlador C = GerenciadorDasOpcoesIniciais.g.MeuControlador;

                if (jogadores[0].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoLocal)
                    C = jogadores[0].Control;

        jogadores[0].ViewID = 0;//G.GetComponent<PhotonView>().viewID;

                G.GetComponent<CharacterManager>().Controlador = C;

                GlobalController.g.Jogadores[0] = jogadores[0];

                EventAgregator.Publish(EventKey.novoHeroiSpawnado, new StandardGameEvent(G, EventKey.novoHeroiSpawnado));
        InformeJogadorAdicionado(0, BytesTransform.ToBytes(jogadores[0]));
                
            

        StartCoroutine(ColocaCamera(numJ));
    }
    /* PUN
    public void ColocandoPlayersNaCena()
    {
        int numJ = 0;
        SloteMultiplayer[] jogadores = GlobalController.g.Jogadores.ToArray();
        for (int i = 0; i < jogadores.Length; i++)
        {

            if (
                jogadores[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoLocal
                ||
                jogadores[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoRede)
                numJ++;

                if (jogadores[i].ViewOwner == PhotonNetwork.player
                && (
                jogadores[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoLocal
                ||
                jogadores[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoRede))
            {
                GameObject G = null;
                switch (i)
                {
                    case 0:
                        G = PhotonNetwork.Instantiate("Ink", new Vector3(505, 2, 44), Quaternion.identity, 0);
                    break;
                    case 1:
                        G = PhotonNetwork.Instantiate("Daniely Gold", new Vector3(103, 2, 43), Quaternion.identity, 0);
                    break;
                    case 2:
                        G = PhotonNetwork.Instantiate("CharlesHeart", new Vector3(103, 2, 45), Quaternion.identity, 0);
                    break;
                    case 3:
                        G = PhotonNetwork.Instantiate("Billy", new Vector3(105, 2, 46), Quaternion.identity, 0);
                    break;
                }

                Controlador C = GerenciadorDasOpcoesIniciais.g.MeuControlador;

                if (jogadores[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoLocal)
                    C = jogadores[i].Control;

                jogadores[i].ViewID = G.GetComponent<PhotonView>().viewID;

                G.GetComponent<CharacterManager>().Controlador = C;

                GlobalController.g.Jogadores[i] = jogadores[i];

                EventAgregator.Publish(EventKey.novoHeroiSpawnado, new StandardGameEvent(G, EventKey.novoHeroiSpawnado));                

                PhotonNetwork.RPC(myView, "InformeJogadorAdicionado", PhotonTargets.Others, false, i, BytesTransform.ToBytes(jogadores[i]));
            }            
        }

        StartCoroutine(ColocaCamera(numJ));
    }*/

    public IEnumerator ColocaCamera(int  numJ)
    {
        yield return new WaitForSeconds(0.15f);

        if (numJ == 1)
        {
            AplicadorDeCamera cam = Camera.main.GetComponent<AplicadorDeCamera>();
            //PUN cam.CamB.Alvo = PhotonView.Find(GlobalController.g.Jogadores[0].ViewID).transform;
            cam.CamB.Alvo = GameObject.FindWithTag("Player").transform;
            cam.enabled = true;

            yield return new WaitForSeconds(0.25f);
            EmparedarCamera.Emparede(Camera.main);
        }
        else
        {
            //PhotonNetwork.RPC(myView, "ConfiguraCameraEmRede", PhotonTargets.All, false);

            GameObject G = GameObject.CreatePrimitive(PrimitiveType.Cube);
            G.AddComponent<PosicionadorDoFocoDaCamera>();

            for (int i = 0; i < GlobalController.g.Jogadores.Count; i++)
                if (GlobalController.g.Jogadores[i].Manager != null)
                    G.transform.position = GlobalController.g.Jogadores[i].Manager.transform.position;

            Destroy(G.GetComponent<BoxCollider>());

            AplicadorDeCamera cam = Camera.main.GetComponent<AplicadorDeCamera>();
            cam.CamB.Alvo = G.transform;
            cam.enabled = true;

            yield return new WaitForSeconds(0.25f);
            EmparedarCamera.Emparede(Camera.main);
        }
    }

    //[PunRPC]
    void InformeJogadorAdicionado(int indice, byte[] j)
    {
        GlobalController.g.Jogadores[indice] = BytesTransform.ToObject<SloteMultiplayer>(j);
        EventAgregator.Publish(EventKey.listaDeJogadoresAtualizada,null);
    }
}
