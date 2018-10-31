using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotoesDeTeste : MonoBehaviour {

    [SerializeField] private Vector3 pos  = new Vector3(12,2,-45);
    bool criando = true;

    private void Start()
    {
        /*
        if(SceneManager.GetActiveScene().name=="montandoJogabilidade")
            pos  = new Vector3(12, 2, -45);
        else if (SceneManager.GetActiveScene().name == "testeMapa")
            pos = new Vector3(105, 2, 44);*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            BotaoPhoton();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            BotaoEntrarNaSala();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            BotaoSpawnarHeroi();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            BotaoSpawnarInimigo();

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BotaoCenaInicial();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            
        }
    }



    public void BotaoCenaInicial()
    {
        /*
        if (PhotonNetwork.connected)
            PhotonNetwork.Disconnect();*/

        Destroy(GameController.g.gameObject);
        Destroy(GlobalController.g.gameObject);

        EventAgregator.ClearListeners();

        SceneManager.LoadScene("SampleScene");

        
    }

    public void BotaoMostraEscondeTestes()
    {
        gameObject.SetActive(!gameObject.activeSelf);

    }

    public void BotaoSpawnarInimigo()
    {
        // RPC_Listener.RPC(NameOfRPC.SpawnarInimigo,PhotonTargets.MasterClient);
        RPC_Listener.SpawnarInimigo();
    }

    public void BotaoSpawnarHeroi()
    {
        if (SceneManager.GetActiveScene().name == "montandoJogabilidade")
            pos = new Vector3(12, 2, -45);
        //else if (SceneManager.GetActiveScene().name == "testeMapa")
        //    pos = new Vector3(posDeTeste, 2, 44);

        /*
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.offlineMode = true;
            PhotonNetwork.CreateRoom("sala de festas");
        }*/

        Vector2 dir = Random.insideUnitCircle;
        Vector3 poss =pos+ 5 * new Vector3(dir.x, 0, dir.y);
        //PUN GameObject G = PhotonNetwork.Instantiate("Ink", poss, Quaternion.identity, 0);
        GameObject G = Instantiate(Resources.Load("Ink"), poss, Quaternion.identity) as GameObject;
        AplicadorDeCamera cam = Camera.main.GetComponent<AplicadorDeCamera>();
        cam.CamB.Alvo = G.transform;
        cam.enabled = true;

        GlobalController.g.Jogadores.Add(
            new SloteMultiplayer() {
                Control = GlobalController.g.Jogadores.Count==0?(Controlador)(-1):(Controlador)GlobalController.g.Jogadores.Count,
                Estado = SloteMultiplayer.EstadoDoSlot.ocupadoLocal,
                NomeNoJogo = "mais um jogador",
                ViewID = 0//G.GetComponent<PhotonView>().viewID,
                //ViewOwner = PhotonNetwork.player
            }
            );

        //PUN RPC_Listener.RPC(NameOfRPC.ColocandoListaDeJogadoresNaRede, PhotonTargets.AllBuffered, BytesTransform.ToBytes(GlobalController.g.Jogadores));
        RPC_Listener.ColocandoListaDeJogadoresNaRede(BytesTransform.ToBytes(GlobalController.g.Jogadores));

        StartCoroutine(ColocaHudEmRede(G));

        int indice = GlobalController.g.Jogadores.Count - 1;

        G.GetComponent<CharacterManager>().Controlador = GlobalController.g.Jogadores[indice].Control;

        /*
        HUDManager.h.MostrarHud(indice);        

        HUDManager.h.AtualizaLifeBar(indice,1);
        HUDManager.h.AtualizaMagicBar(indice, 1);*/

        StartCoroutine(GameController.g.ColocaCamera(GlobalController.g.Jogadores.Count));

    }

    IEnumerator ColocaHudEmRede(GameObject G)
    {
        yield return new WaitForSeconds(1);
        //Debug.Log(G.GetComponent<PhotonView>().viewID);
        //PUN RPC_Listener.RPC(NameOfRPC.HudDoHeroiSpawnado, PhotonTargets.AllBuffered, G.GetComponent<PhotonView>().viewID);
        RPC_Listener.HudDoHeroiSpawnado( G);
    }

    public void BotaoPhoton()
    {
        criando = true;
        /*
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings("1");
        }*/
    }

    public void BotaoEntrarNaSala()
    {
        criando = false;
        /*
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings("1");
        }*/
    }

    /*PUN
    private void OnConnectedToPhoton()
    {
        Debug.Log("Me conectei ao photon");
    }

    void OnJoinedLobby()
    {
        
        Debug.Log("Me juntei ao lobby");

        if (criando)
        {
            Debug.Log("tentando crir sala");
            PhotonNetwork.CreateRoom("sala do teste");
        }
      //  else
      //      PhotonNetwork.GetRoomList();
    }

    void OnReceivedRoomListUpdate()
    {
        Debug.Log("recebi a lista de salas");
        if (!criando)
        {
            RoomInfo[] Rs = PhotonNetwork.GetRoomList();
            if (Rs.Length > 0)
                PhotonNetwork.JoinRoom(Rs[0].Name);
            else
                Debug.Log("A lista de salas está vazia");
        }
    }

    void OnJoinedRoom()
    {
        if (!criando)
            Debug.Log("Me juntei a sala");
        else
            Debug.Log("Estou na sala");
    }

    void OnCreateRoom()
    {
        Debug.Log("A sala foir Criada");
    }*/

    
}
