using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomsListPanel : UiDeOpcoes
{

    /* PUN
    RoomInfo[] opcoes;
    

    public void IniciarHud()
    {
        sr.gameObject.SetActive(true);
        opcoes = PhotonNetwork.GetRoomList();
        IniciarHUD(opcoes.Length, TipoDeRedimensionamento.vertical);
    }

    public override void SetarComponenteAdaptavel(GameObject G, int indice)
    {
        G.GetComponent<MultiplayerRoomOption>().SetarOpcao(BotaoEntrarNaSala,opcoes[indice]);
    }

    protected override void FinalizarEspecifico()
    {
        
    }

    void BotaoEntrarNaSala(int indice)
    {
        PhotonNetwork.JoinRoom(opcoes[indice].Name);
        EventAgregator.Publish(EventKey.joinRoomButtonClicked, new JoinRoomButtonClickedEvent(GlobalController.g.gameObject, opcoes[indice]));
        //GerenciadorDasOpcoesIniciais.g.L.MensagemDeEstouNaSala(opcoes[indice]);
    }

    */
    public override void SetarComponenteAdaptavel(GameObject G, int indice)
    {
        throw new System.NotImplementedException();
    }

    protected override void FinalizarEspecifico()
    {
        throw new System.NotImplementedException();
    }
}
