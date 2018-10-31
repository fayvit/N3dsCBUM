using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDasOpcoesIniciais : MonoBehaviour {

    public static GerenciadorDasOpcoesIniciais g;

    [SerializeField] private MenuBasico menuBasico;
    [SerializeField] private OpenMultiplayerHudManager o;
    [SerializeField] private ListRoomsManager l;
    [SerializeField] private Controlador controlador = Controlador.nulo;
    
    bool menuEstavaAtivo = false;

    public Controlador MeuControlador
    {
        get { return controlador; }
        private set { controlador = value; }
    }

    void Start()
    {
        // PUN EventAgregator.AddListener(EventKey.cancelInInputRoomName, RetornarDoListRooms);
        VerificaSavesE_InsereMenus();
        g = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (menuBasico.EstaAtivo && menuEstavaAtivo)
        {
            menuBasico.MudarOpcao();
            if (CommandReader.SubmitButtonDown())
            {
                for (int i = -1; i < 5; i++)
                {
                    if (i != 0)
                        if (CommandReader.SubmitButtonDown((Controlador)i))
                            MeuControlador = (Controlador)i;
                }

                EscolhaDoMenuInicial(menuBasico.OpcaoEscolhida);
                EventAgregator.Publish(EventKey.positiveUiInput, null);
            }
        }

        menuEstavaAtivo = menuBasico.EstaAtivo;
    }

    void VerificaSavesE_InsereMenus()
    {
        menuBasico.FinalizarHud();

        menuBasico.IniciarHud(EscolhaDoMenuInicial, new string[3] {
                "Iniciar Jogo Solo","Abrir Multiplayer","Juntar-se a jogo"
            });
    }

    void EscolhaDoMenuInicial(int escolha)
    {
        menuBasico.FinalizarHud();

        if (MeuControlador == Controlador.nulo)
            MeuControlador = Controlador.teclado;


        switch (escolha)
        {
            case 0:
                //PhotonNetwork.offlineMode = true;
                //PhotonNetwork.CreateRoom("salinha oooo");
                OpenMultiplayerHudManager.meuSloteMultiplayer = new SloteMultiplayer()
                {
                    Control = controlador,
                    Estado = SloteMultiplayer.EstadoDoSlot.ocupadoLocal,
                    NomeNoJogo = "Jogador 1"
                };

                SloteMultiplayer[] s = new SloteMultiplayer[3];
                GlobalController.g.CarregueCenas(s);
            break;
                /* PUN
            case 1:
                o.gameObject.SetActive(true);
            break;
            case 2:
                l.gameObject.SetActive(true);
            break;*/
        }
    }

    /* PUN
    public void RetornarDoListRooms(IGameEvent e)
    {
        if (PhotonNetwork.connected)
            PhotonNetwork.Disconnect();

        l.gameObject.SetActive(false);
        l.FinalizarListaDesalas();
        VerificaSavesE_InsereMenus();

        EventAgregator.Publish(EventKey.negativeUiInput, null);
    }

    public void RetornarDoMultiJogo()
    {
        if (PhotonNetwork.connected)
            PhotonNetwork.Disconnect();

        o.RestaurarLanFechada();

        o.gameObject.SetActive(false);
        VerificaSavesE_InsereMenus();

        //GlobalController.g.PlayersInGame = new List<PlayerInGame>();
        EventAgregator.Publish(EventKey.negativeUiInput, null);
    }

    public void RetornarDoListRooms()
    {
        if (PhotonNetwork.connected)
            PhotonNetwork.Disconnect();

        l.gameObject.SetActive(false);
        l.FinalizarListaDesalas();
        VerificaSavesE_InsereMenus();
    }*/
}
