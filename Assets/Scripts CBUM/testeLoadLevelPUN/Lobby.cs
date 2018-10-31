using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Lobby : MonoBehaviour {

    /* PUN
    bool criando = false;
    [SerializeField] private Text status;
    
    public void AbrirSala()
    {
        PhotonNetwork.ConnectUsingSettings("0.0.0");
        criando = true;
    }

    public void EntrarNaSalaCriada()
    {
        PhotonNetwork.ConnectUsingSettings("0.0.0");
        criando = false;
    }

    public void CarregarCena()
    {
        PhotonNetwork.LoadLevel(1);
    }

    private void OnConnectedToPhoton()
    {        
        Debug.Log("conectado ao photon");
        
    }

    private void OnConnectedToMaster()
    {
        Debug.Log("Conectado ao master");
    }

    private void OnJoinedLobby()
    {
        PhotonNetwork.automaticallySyncScene = true;
        Debug.Log("Me juntei ao lobby");

        if (criando)
            PhotonNetwork.CreateRoom("sala de festas");
        else
        {
            PhotonNetwork.GetRoomList();
        }
    }

    void OnReceivedRoomListUpdate()
    {
        RoomInfo[] R = PhotonNetwork.GetRoomList();
        if (!PhotonNetwork.inRoom && R.Length > 0 && !criando)
        {
            PhotonNetwork.JoinRoom(R[0].Name);
        }
    }

    void OnJoinedRoom()
    {
        Debug.Log("Entrei na sala criada");
    }

    private void OnCreatedRoom()
    {
        Debug.Log("Eu criei a sala");
    }*/
}
