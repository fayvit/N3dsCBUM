using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomManager : MonoBehaviour {
    /* PUN
    [SerializeField] private InputField doNomeDoDono;
    [SerializeField] private InputField daSenhaDeEntrada;
    private EstadoDoCreateRoom estado = EstadoDoCreateRoom.selecao;

    private enum EstadoDoCreateRoom
    {
        selecao,
        editandoTexto
    }

    private void Awake()
    {
            
    }

    // Use this for initialization
    void Start () {
		
	}

    IEnumerator Zuado ()
    {
        yield return new WaitForEndOfFrame();
        EventAgregator.Publish(EventKey.submitRoomName, null);
    }
	
	// Update is called once per frame
	void Update () {
        switch (estado)
        {
            case EstadoDoCreateRoom.editandoTexto:
                if (CommandReader.ButtonDown(7, (int)GerenciadorDasOpcoesIniciais.g.MeuControlador))
                {
                    StartCoroutine(Zuado());
                    doNomeDoDono.interactable = false;
                    estado = EstadoDoCreateRoom.selecao;
                    GameObject.Find("btnIniciar").GetComponent<Button>().Select();
                }
                    
            break;
            case EstadoDoCreateRoom.selecao:
               if (doNomeDoDono.isFocused)
                    BotaoNomeDoDono();
            break;
        }
	}

   

    public void BotaoNomeDoDono()
    {
        doNomeDoDono.interactable = true;
        doNomeDoDono.ActivateInputField();

        EventAgregator.Publish(EventKey.buttonOwnerNameClicked, null);
        estado = EstadoDoCreateRoom.editandoTexto;

    }


    public void BotaoSenhaDaSala()
    {

    }

    public void BotaoAvancar()
    {
        if (!PhotonNetwork.connected)
        {

            if (PhotonNetwork.connectionState == ConnectionState.Disconnected)
            {
                gameObject.SetActive(false);
                PhotonNetwork.ConnectUsingSettings(Application.version);

                if (string.IsNullOrEmpty(doNomeDoDono.text))
                {
                    doNomeDoDono.text = "Nameless master client" + Random.Range(0, 1000);
                }

                
                EventAgregator.Publish(EventKey.createdRoomInNetMultiplayerMenu, 
                    new CreatedRoomEvent(gameObject,doNomeDoDono.text,GerenciadorDasOpcoesIniciais.g.MeuControlador));
            }
        }

        EventAgregator.Publish(EventKey.clickedInAdvanceToCreateNetGame,new StandardGameEvent(gameObject,EventKey.clickedInAdvanceToCreateNetGame));
        EventAgregator.Publish(EventKey.positiveUiInput, null);
    }

    void OnConnectedToPhoton()
    {
        Debug.Log("Conectado");
    }

    void OnFailedToConnectToPhoton()
    {
        Debug.Log("Falhou");
    }

    public void BtnCancelarCriacaoDeSala()
    {
        EventAgregator.Publish(EventKey.cancelRoomCreation, new StandardGameEvent(gameObject,EventKey.cancelRoomCreation));
    }
    */
}
