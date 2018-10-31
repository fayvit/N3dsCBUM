using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListRoomsManager : MonoBehaviour {

    /* PUN
    [SerializeField] private RoomsListPanel rooms;
    [SerializeField] private GameObject painelGif;
    [SerializeField] private Text txtGif;
    [SerializeField] private Image imgGif;
    [SerializeField] private GameObject painelDeNomeacao;

    private RoomInfo[] Rs;
    private string nomeJogador = "";
    private EstadoDoListRooms estado = EstadoDoListRooms.painelAberto;
    
    private enum EstadoDoListRooms
    {
        comunicacaoComServidor,
        painelAberto,
        modificavel
    }

    #region MonoBehaviourFunctions
    private void OnEnable()
    {
        painelDeNomeacao.SetActive(true);
    }

    private void Start()
    {
        EventAgregator.AddListener(EventKey.submitNameFronRoom, AposNomeacao);
        EventAgregator.AddListener(EventKey.joinRoomButtonClicked, MensagemDeEstouNaSala);

    }

    private void Update()
    {
        //  if(Rs!=null)
        //    Debug.Log(Rs.Length);

        //rooms.MudarOpcao();

        switch (estado)
        {
            case EstadoDoListRooms.modificavel:
                ButtonsFindTools.ModificaOpcao();

                if (CommandReader.ButtonDown(0))
                    ButtonsFindTools.ClickNoDestacado();

            break;
        }
    }
    #endregion

    #region MyButtonsFunctions
    public void AposNomeacao(IGameEvent e)
    {

        nomeJogador = (e as StandardSendStringEvent).StringContent;
        painelDeNomeacao.SetActive(false);
        painelGif.SetActive(true);
        txtGif.text = "Conectando ao Servidor";
        imgGif.enabled = true;

        PhotonNetwork.ConnectUsingSettings(Application.version);
    }


    void ListarSalas()
    {
        rooms.FinalizarHud();

        rooms.IniciarHud();
    } 

    void BtnRecarregarSalas()
    {
        if(PhotonNetwork.connected)
            Rs = PhotonNetwork.GetRoomList();

        OnConnectedToPhoton();
    }

    public void FinalizarListaDesalas()
    {
        estado = EstadoDoListRooms.painelAberto;
        rooms.FinalizarHud();
    }

    #endregion

    #region PhotonFunctions
    void OnMasterClientSwitched(PhotonPlayer pp)
    {
        txtGif.color = new Color(1,0.5f,0);
        txtGif.text = "O Master client desconectou\r\n procurando salas abertas";
        Invoke("CloseConnection", 2.5f);
    }

    void CloseConnection()
    {
        txtGif.color = Color.yellow;
        PhotonNetwork.CloseConnection((GlobalController.g.GetComponent<PhotonView>().owner));
    }

    void OnReceivedRoomListUpdate()
    {
        Rs = PhotonNetwork.GetRoomList();

        if (Rs != null)
        {
            Debug.Log(Rs.Length);
        }

        Debug.Log("recebi a lista de salas???");

        if (Rs != null)
        {
            if (Rs.Length > 0)
            {
                ListarSalas();

                painelGif.SetActive(false);
            }
            else
            {
                rooms.FinalizarHud();

                imgGif.enabled = false;

                painelGif.SetActive(true);

                txtGif.text = "Nenhuma sala aberta no servidor";
            }

            ButtonsFindTools.BuscarBotoes();

            estado = EstadoDoListRooms.modificavel;
        }
    }

    void OnPhotonCreateRoomFailed()
    {
        Debug.Log("a criação de sala falhou");
    }

    void OnConnectedToPhoton()
    {
        PhotonNetwork.automaticallySyncScene = true;
        txtGif.text = "Conexão bem sucedida \r\n baixando lista de salas";
    }

    void OnJoinedLobby()
    {
        PhotonNetwork.automaticallySyncScene = true;
        Debug.Log("list joined lobby");
        Rs = PhotonNetwork.GetRoomList();
    }

    public void MensagemDeEstouNaSala(IGameEvent e)
    {
        RoomInfo R = (e as JoinRoomButtonClickedEvent).InfoRoom;
        painelGif.SetActive(true);
        imgGif.enabled = false;
        txtGif.text = "na sala de: "+R.Name+"\r\n Aguardando inicio da partida";
        rooms.FinalizarHud();
    }

    void OnJoinedRoom()
    {

        int viewID = PhotonNetwork.Instantiate(
            "playerMark", 
            Vector3.zero, 
            Quaternion.identity, 
            0).GetComponent<PhotonView>().viewID;

        RPC_Listener.RPC(NameOfRPC.EuEntreiNasala, 
            PhotonTargets.MasterClient, 
            nomeJogador,
            viewID);

        Debug.Log("estou na sala" + PhotonNetwork.room.Name);
    }

    #endregion
    */
}
