using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PainelJogadorEmRedeAguardando : MonoBehaviour {

    [SerializeField] private Text nomeDoConectado;

    public void IniciarPainel(string nome)
    {
        gameObject.SetActive(true);
        nomeDoConectado.text = nome;
    }

    public void BtnChutar()
    {
        EventAgregator.Publish(EventKey.kickPlayerButtonClicked,new StandardGameEvent(gameObject,EventKey.kickPlayerButtonClicked));
        //GerenciadorDasOpcoesIniciais.g.O.ChutouAlguem(transform);
    }

    public void BtnCancelar()
    {
        EventAgregator.Publish(EventKey.cancelWithplayerInRoom, new StandardGameEvent(gameObject,EventKey.cancelWithplayerInRoom));
        //GerenciadorDasOpcoesIniciais.g.O.CancelouComAlguemNaSala(transform);
    }
}
