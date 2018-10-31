using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour {

    public static GlobalController g;
    [SerializeField] private MusicaDeFundo music;

    private List<SloteMultiplayer> jogadores = new List<SloteMultiplayer>();

    public List<SloteMultiplayer> Jogadores
    {
        get { return jogadores; }
        set { jogadores = value; }
    }

    public MusicaDeFundo Music
    {
        get { return music; }
        private set { music = value; }
    }

    // Use this for initialization

    void Awake () {
        GlobalController[] g = FindObjectsOfType<GlobalController>();

        if (g.Length>1)
            Destroy(gameObject);

        transform.parent = null;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        GlobalController.g = this;

        //EventAgregator.AddListener(EventKey.novoHeroiSpawnado, OnNewHeroSpawned);
        EventAgregator.AddListener(EventKey.UiDeOpcoesChange,OnChangeOptionUi);
        EventAgregator.AddListener(EventKey.positiveUiInput, OnPositiveUiInput);
        EventAgregator.AddListener(EventKey.negativeUiInput, OnNegativeUiInput);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.UiDeOpcoesChange, OnChangeOptionUi);
        EventAgregator.RemoveListener(EventKey.positiveUiInput, OnPositiveUiInput);
        EventAgregator.RemoveListener(EventKey.negativeUiInput, OnNegativeUiInput);
    }

    void OnNegativeUiInput(IGameEvent e)
    {
        Destroy(Instantiate(Resources.Load("cancelMenuSound")), 2);
    }

    void OnPositiveUiInput(IGameEvent e)
    {
        Destroy(Instantiate(Resources.Load("selectMenuSound")), 2);
    }

    void OnChangeOptionUi(IGameEvent e)
    {
        Destroy(Instantiate(Resources.Load("changeMenuSound")),2);
    }

    public void CarregueCenas(SloteMultiplayer[] s)
    {
        //PUN OpenMultiplayerHudManager.meuSloteMultiplayer.ViewOwner = PhotonNetwork.player;
        Jogadores = new List<SloteMultiplayer> { OpenMultiplayerHudManager.meuSloteMultiplayer };
        for (int i = 0; i < 3; i++)
        {
            /*
            if (s[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoLocal)
                s[i].ViewOwner = PhotonNetwork.player;*/

            if (s[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoLocal || s[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoRede)
                Jogadores.Add(s[i]);
        }

        /*
        Jogadores = new List<SloteMultiplayer>
        {
            OpenMultiplayerHudManager.meuSloteMultiplayer,
            s[0],s[1],s[2]
        };*/

        //PUN RPC_Listener.RPC(NameOfRPC.ColocandoListaDeJogadoresNaRede, PhotonTargets.All, BytesTransform.ToBytes(Jogadores));
        RPC_Listener.ColocandoListaDeJogadoresNaRede(BytesTransform.ToBytes(Jogadores));

        GameObject G = new GameObject();
        SceneLoader loadScene = G.AddComponent<SceneLoader>();
        loadScene.CenaDoCarregamento(0);
    }

        //PhotonNetwork.LoadLevel("testeMapa");

        /*
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("testeMapa");
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += x;
    }

    void x(UnityEngine.SceneManagement.Scene s, UnityEngine.SceneManagement.LoadSceneMode l)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("SampleScene");
    }*/

    void OnNewHeroSpawned(IGameEvent e)
    {
        /*
        GameObject p = e.Sender.GetComponent<PhotonView>();
        if (IndiceEntreJogadores(p.viewID)==-1)
            Jogadores.Add(
                new SloteMultiplayer()
                {
                    NomeNoJogo = "Jogador: " + Jogadores.Count,
                    Control = Controlador.teclado,
                    ViewID = p.viewID,
                    ViewOwner = p.owner
                });
              */
        Debug.Log("função eliminada chamada");
    }

    // Update is called once per frame
    void Update () {
        Music.Update();
	}

    
    public void EuEntreiNasala(string nome, int viewID)
    {
        EventAgregator.Publish(EventKey.joinedRoom, new JoinedRoomEvent(gameObject,viewID,nome));

    }

    public int IndiceEntreJogadores(int viewId)
    {
        int retorno = -1;

        for (int i = 0; i < jogadores.Count; i++)
        {

            if (jogadores[i].ViewID == viewId)
                retorno = i;
        }

        return retorno;
    }

}
