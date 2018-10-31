using UnityEngine;
using UnityEngine.UI;

public class MultiplayerRoomOption : UmaOpcao
{
    [SerializeField] private Text nomeSala;
    [SerializeField] private Text numJogadores;
    
    public virtual void SetarOpcao(System.Action<int> acaoDaOpcao/*PUN ,RoomInfo info*/)
    {
        /* PUN
            nomeSala.text = "sala de " + info.Name;
            numJogadores.text = info.PlayerCount + " /" + info.MaxPlayers;
            Acao += acaoDaOpcao;
        */
    }
}
