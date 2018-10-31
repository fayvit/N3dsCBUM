using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PainelTentandoCarregar : MonoBehaviour
{
    /* PUN
    [SerializeField] private Text txtDoCarregamento;
    // Use this for initialization
    void OnEnable()
    {
        txtDoCarregamento.text = "Conectando Ao Servidor";
        txtDoCarregamento.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnJoinedLobby()
    {
        PhotonNetwork.automaticallySyncScene = true;

        PhotonNetwork.CreateRoom(OpenMultiplayerHudManager.meuSloteMultiplayer.NomeNoJogo,
                new RoomOptions() { MaxPlayers = 2 },
                new TypedLobby() { Type = LobbyType.Default }
                );
    }

    void OnConnectedToPhoton()
    {
        txtDoCarregamento.text = "Conexão bem sucedida.\n \r Criando sala";
    }

    

    IEnumerator TempoDeSalaCriada()
    {
        yield return new WaitForSeconds(0);

        gameObject.SetActive(false);
        EventAgregator.Publish(EventKey.postCreatedRoom, new StandardGameEvent(gameObject, EventKey.postCreatedRoom));
        
        Debug.Log("estou na sala??? " + PhotonNetwork.inRoom);
    }

    void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.Log("falha ao conectar,causa do Photon: "+cause);
    }

    void OnCreatedRoom()
    {
        txtDoCarregamento.text = "Sala criada com sucesso";
        StartCoroutine(TempoDeSalaCriada());
    }   

    void OnPhotonCreateRoomFailed()
    {
        Debug.Log("a criação de sala falhou");
    }
    */
}
