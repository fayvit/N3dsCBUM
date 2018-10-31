using UnityEngine;

public class OpenMultiplayerHudManager : MonoBehaviour {
    public static SloteMultiplayer meuSloteMultiplayer;
    /*
    [SerializeField] ContainerDePaineis[] cp;

    private SloteMultiplayer[] sloteM = new SloteMultiplayer[3];
    private EstadoDoMenuMultiplayer estado = EstadoDoMenuMultiplayer.modificando;
    

    public enum EstadoDoMenuMultiplayer
    {
        modificando,
        editantoEspecifico,
        loadingNetOperation
    }

    #region monoBehaviourFunctions

    void OnEnable()
    {
        ButtonsFindTools.BusqueComUltimoDestacado();       
    }

    private void Start()
    {
        EventAgregator.AddListener(EventKey.createdRoomInNetMultiplayerMenu, OnCreateRoomInMenu);
        EventAgregator.AddListener(EventKey.clickedInAdvanceToCreateNetGame, PainelDaTentativaDeCarregar);
        EventAgregator.AddListener(EventKey.cancelRoomCreation, OnCancelRoomCreation);
        EventAgregator.AddListener(EventKey.joinedRoom, AlguemEntrouNaSala);
        EventAgregator.AddListener(EventKey.kickPlayerButtonClicked, ChutouAlguem);
        EventAgregator.AddListener(EventKey.cancelWithplayerInRoom, CancelouComAlguemNaSala);
        EventAgregator.AddListener(EventKey.buttonOwnerNameClicked, MudarParaEstadoEditandoEspecifico);
        EventAgregator.AddListener(EventKey.submitRoomName, (IGameEvent e)=> { estado = EstadoDoMenuMultiplayer.modificando; });
        EventAgregator.AddListener(EventKey.postCreatedRoom,PainelDoAguardandoJoin);
        EventAgregator.AddListener(EventKey.cancelLocalPlayerInGame, OnLocalPlayerCancel);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.createdRoomInNetMultiplayerMenu, OnCreateRoomInMenu);
        EventAgregator.RemoveListener(EventKey.clickedInAdvanceToCreateNetGame, PainelDaTentativaDeCarregar);
        EventAgregator.RemoveListener(EventKey.cancelRoomCreation, OnCancelRoomCreation);
        EventAgregator.RemoveListener(EventKey.joinedRoom, AlguemEntrouNaSala);
        EventAgregator.RemoveListener(EventKey.kickPlayerButtonClicked, ChutouAlguem);
        EventAgregator.RemoveListener(EventKey.cancelWithplayerInRoom, CancelouComAlguemNaSala);
        EventAgregator.RemoveListener(EventKey.buttonOwnerNameClicked, MudarParaEstadoEditandoEspecifico);
        EventAgregator.RemoveListener(EventKey.submitRoomName, (IGameEvent e) => { estado = EstadoDoMenuMultiplayer.modificando; });
        EventAgregator.RemoveListener(EventKey.postCreatedRoom, PainelDoAguardandoJoin);
        EventAgregator.RemoveListener(EventKey.cancelLocalPlayerInGame, OnLocalPlayerCancel);
        
    }

    void Update()
    {
        switch (estado)
        {
            case EstadoDoMenuMultiplayer.modificando:
                ButtonsFindTools.ModificaOpcao();

                if (CommandReader.SubmitButtonDown(GerenciadorDasOpcoesIniciais.g.MeuControlador))
                    ButtonsFindTools.ClickNoDestacado();

                for (int i = 0; i < 5; i++)
                {
                    if (i != 0 && i != (int)GerenciadorDasOpcoesIniciais.g.MeuControlador)
                        if (CommandReader.ButtonDown(7, i)&&estado==EstadoDoMenuMultiplayer.modificando)
                        {
                            AdicionaLocalPlayer(i);
                        }
                }
                break;
        }

    }

    #endregion

    public void RestaurarLanFechada()
    {
        for (int i = 0; i < cp.Length; i++)
        {
            cp[i].create.transform.parent.Find("containerDoJ").gameObject.SetActive(true);
            cp[i].create.gameObject.SetActive(false);
            cp[i].ptc.gameObject.SetActive(false);
            cp[i].painelAguardando.SetActive(false);
        }
    }

    void AdicionaLocalPlayer(int i)
    {
        int indiceDoAberto = SloteMultiplayer.PrimeiroDoEstado(sloteM, SloteMultiplayer.EstadoDoSlot.abertoParaLocal);
        if (indiceDoAberto > -1 && SloteMultiplayer.NaoControloNinguem(sloteM, i))
        {
            sloteM[indiceDoAberto] = new SloteMultiplayer()
            {
                Estado = SloteMultiplayer.EstadoDoSlot.ocupadoLocal,
                NomeNoJogo = ((Controlador)i).ToString(),
                Control = (Controlador)i,
                ViewOwner = PhotonNetwork.player
            };

            cp[indiceDoAberto].paineisLocalPlayer.gameObject.SetActive(true);
            cp[indiceDoAberto].paineisLocalPlayer.MeuControlador = (Controlador)i;
            cp[indiceDoAberto].paineisLocalPlayer.transform.parent.Find("containerDoJ").gameObject.SetActive(false);

            ButtonsFindTools.BuscarBotoes();
        }
    }

    #region ForEvents
    void OnCreateRoomInMenu(IGameEvent e)
    {
        CreatedRoomEvent s = e as CreatedRoomEvent;
        meuSloteMultiplayer = new SloteMultiplayer()
        {
            Estado = SloteMultiplayer.EstadoDoSlot.ocupadoLocal,
            NomeNoJogo = s.NomeDoDono,
            Control = s.Controlador
        };

        estado = EstadoDoMenuMultiplayer.loadingNetOperation;
        ButtonsFindTools.ActiveSelfButtons(false);
    }

    void OnLocalPlayerCancel(IGameEvent e)
    {
        e.Sender.SetActive(false);
        e.Sender.transform.parent.Find("containerDoJ").gameObject.SetActive(true);
        ButtonsFindTools.BuscarBotoes();
        for (int i = 0; i < cp.Length; i++)
        {
            if (e.Sender.transform.parent == cp[i].paineisLocalPlayer.transform.parent)
            {
                sloteM[i].Estado = SloteMultiplayer.EstadoDoSlot.abertoParaLocal;
            }            
        }
    }

    public void AlguemEntrouNaSala(IGameEvent e)
    {
        JoinedRoomEvent j = e as JoinedRoomEvent;
        if (SloteMultiplayer.PrimeiroDoEstado(sloteM, SloteMultiplayer.EstadoDoSlot.abertoParaRede) > -1)
        {
            int i = SloteMultiplayer.PrimeiroDoEstado(sloteM, SloteMultiplayer.EstadoDoSlot.abertoParaRede);
            sloteM[i] = new SloteMultiplayer()
            {
                ViewID = j.ViewID,
                ViewOwner = PhotonView.Find(j.ViewID).owner,
                NomeNoJogo = j.Nome,
                Control = Controlador.rede,
                Estado = SloteMultiplayer.EstadoDoSlot.ocupadoRede
            };
            cp[i].painelAguardando.SetActive(false);
            cp[i].paineisRedeAguardando.IniciarPainel(j.Nome);
            
        }
        else
            Debug.Log("não há vagas abertas");

        ButtonsFindTools.BuscarBotoes();
    }

    public void PainelDaTentativaDeCarregar(IGameEvent e)
    {
        for (int i = 0; i < cp.Length; i++)
        {
            if (e.Sender.transform.parent == cp[i].ptc.transform.parent)
                cp[i].ptc.gameObject.SetActive(true);
        }

        ButtonsFindTools.BuscarBotoes();
    }

    void OnCancelRoomCreation(IGameEvent e)
    {
        for (int i = 0; i < cp.Length; i++)
        {
            if (cp[i].create.gameObject == e.Sender)
            {
                sloteM[i].Estado = SloteMultiplayer.EstadoDoSlot.abertoParaLocal;
                Debug.Log(sloteM[i].Estado);
            }
        }
        RestaurarLanFechada();
        ButtonsFindTools.BuscarBotoes();

        EventAgregator.Publish(EventKey.negativeUiInput, null);
    }

    public void ChutouAlguem(IGameEvent e)
    {
        Transform T = e.Sender.transform;
        int guarde = -1;
        T.gameObject.SetActive(false);
        for (int i = 0; i < cp.Length; i++)
            if (cp[i].paineisRedeAguardando.transform == T)
            {
                cp[i].painelAguardando.gameObject.SetActive(true);
                guarde = i;
            }

        sloteM[guarde].Estado = SloteMultiplayer.EstadoDoSlot.abertoParaRede;
        
        if(PhotonView.Find(sloteM[guarde].ViewID).owner!=null)
            PhotonNetwork.CloseConnection(PhotonView.Find(sloteM[guarde].ViewID).owner);

        ButtonsFindTools.BuscarBotoes();

    }

    void MudarParaEstadoEditandoEspecifico(IGameEvent e)
    {
        estado = EstadoDoMenuMultiplayer.editantoEspecifico;
    }

    public void CancelouComAlguemNaSala(IGameEvent e)
    {
        Transform T = e.Sender.transform;
        int guarde = -1;
        for (int i = 0; i < cp.Length; i++)
            if (cp[i].paineisRedeAguardando.transform == T)
            {
                guarde = i;
                sloteM[guarde].Estado = SloteMultiplayer.EstadoDoSlot.fechado;
            }

        
        int esseNumero = SloteMultiplayer.PrimeiroDoEstado(sloteM, SloteMultiplayer.EstadoDoSlot.abertoParaRede);
        

        if (esseNumero <= -1)
        {
            RestaurarLanFechada();

            PhotonNetwork.Disconnect();

        }
        else
        {
            PhotonView c = PhotonView.Find(sloteM[guarde].ViewID);
            sloteM[guarde].Estado = SloteMultiplayer.EstadoDoSlot.desconexaoAgendada;
            PhotonNetwork.CloseConnection(c.owner);
        }

        T.gameObject.SetActive(false);

        ButtonsFindTools.BuscarBotoes();

        EventAgregator.Publish(EventKey.negativeUiInput, null);

    }

    void DesconexaoAgendada(PhotonPlayer p,int guarde)
    {         

        PhotonNetwork.room.MaxPlayers--;

        sloteM[guarde].Estado = SloteMultiplayer.EstadoDoSlot.abertoParaLocal;
        cp[guarde].create.transform.parent.Find("containerDoJ").gameObject.SetActive(true);

        ButtonsFindTools.BuscarBotoes();
    }



    public void PainelDoAguardandoJoin(IGameEvent e)
    {

        Transform T = e.Sender.transform;
        ButtonsFindTools.ActiveSelfButtons(true);
        for (int i = 0; i < cp.Length; i++)
        {
            if (T.parent == cp[i].painelAguardando.transform.parent || cp[i].create.gameObject.activeSelf)
            {
                if (cp[i].create.gameObject.activeSelf)
                {
                    PhotonNetwork.room.MaxPlayers++;
                    Debug.Log("maxPlayers: " + PhotonNetwork.room.MaxPlayers);
                }

                cp[i].create.gameObject.SetActive(false);
                cp[i].painelAguardando.SetActive(true);
                sloteM[i].Estado = SloteMultiplayer.EstadoDoSlot.abertoParaRede;
            }
        }

        ButtonsFindTools.BuscarBotoes();
        estado = EstadoDoMenuMultiplayer.modificando;
    }
    #endregion

    #region functionsForButtons

    public void BtnIniciar()
    {
        if (SloteMultiplayer.PrimeiroDoEstado(sloteM, SloteMultiplayer.EstadoDoSlot.ocupadoRede) > -1)
        {
            //GameController.g.RPC("carregueCenas", PhotonTargets.All);
            GlobalController.g.CarregueCenas(sloteM);
            
        }
        else if (SloteMultiplayer.PrimeiroDoEstado(sloteM, SloteMultiplayer.EstadoDoSlot.ocupadoLocal) > -1)
        {
            if (PhotonNetwork.connected)
                PhotonNetwork.Disconnect();

            PhotonNetwork.offlineMode = true;
            PhotonNetwork.CreateRoom("sala de festas");
            meuSloteMultiplayer = new SloteMultiplayer()
            {
                Control = GerenciadorDasOpcoesIniciais.g.MeuControlador,
                Estado = SloteMultiplayer.EstadoDoSlot.ocupadoLocal,
                NomeNoJogo = "Jogador 1",
                ViewOwner = PhotonNetwork.player
            };
            GlobalController.g.CarregueCenas(sloteM);
        }
        else
        {
            //single game
        }
    }

    public void BotaoAbrirParaLan(Transform T)
    {
        if (!PhotonNetwork.connected)
        {
            for (int i = 0; i < cp.Length; i++)
            {
                if (T.parent.parent == cp[i].create.transform.parent)
                {
                    T.parent.gameObject.SetActive(false);
                    cp[i].create.gameObject.SetActive(true);

                    sloteM[i].Estado = SloteMultiplayer.EstadoDoSlot.fechado;
                }
            }
        }
        else
        {
            PhotonNetwork.room.MaxPlayers++;
            T.parent.gameObject.SetActive(false);
            PainelDoAguardandoJoin(new StandardGameEvent(T.parent.gameObject,EventKey.postCreatedRoom));
        }

        ButtonsFindTools.BuscarBotoes();

        EventAgregator.Publish(EventKey.positiveUiInput, null);
    }

    public void CancelarVagaNaSala(Transform T)
    {
        EventAgregator.Publish(EventKey.negativeUiInput, null);
        PhotonNetwork.room.MaxPlayers--;

        if (PhotonNetwork.room.MaxPlayers == 1)
        {
            Debug.Log("desconectou por cancelar vaga na sala");
            PhotonNetwork.Disconnect();
        }

        T.parent.gameObject.SetActive(false);

        for (int i = 0; i < cp.Length; i++)
        {
            if (T.parent.parent == cp[i].create.transform.parent)
            {                 
                sloteM[i].Estado = SloteMultiplayer.EstadoDoSlot.fechado;

                if (SloteMultiplayer.PrimeiroDoEstado(sloteM, SloteMultiplayer.EstadoDoSlot.abertoParaRede) < 0)
                {
                    cp[i].create.gameObject.SetActive(true);
                }
                else
                {
                    cp[i].transform.Find("containerDoJ").gameObject.SetActive(true);
                }
            }
        }

        ButtonsFindTools.BuscarBotoes();
    }
    #endregion

    void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("Chamou...");
        for (int i = 0; i < sloteM.Length; i++)
        {
            Debug.Log("Não anulou: ownner = "+sloteM[i].ViewOwner+" other: "+other);
            if (sloteM[i].ViewOwner == other && sloteM[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoRede)
            {
                ChutouAlguem(new StandardGameEvent(cp[i].paineisRedeAguardando.gameObject, EventKey.cancelWithplayerInRoom));
            }
            else if (sloteM[i].ViewOwner == other && sloteM[i].Estado == SloteMultiplayer.EstadoDoSlot.desconexaoAgendada)
            {
                DesconexaoAgendada(other,i);
            }

        }
    }*/

}
