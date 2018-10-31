using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementosDoDeslocamentoControlado {

    [SerializeField] private Transform[] alvos;

    private ControlledMoveForCharacter[] controlls = new ControlledMoveForCharacter[4];
    private bool[] movimentoAlcancado = new bool[4];
    
    public void OnPositionOk(IGameEvent e,string ID)
    {
        StandardSendIntAndStringEvent s = e as StandardSendIntAndStringEvent;

        if (s.MyString == ID)
        {
            movimentoAlcancado[s.MyInt] = true;
        }
    }

    public void IniciandoMovimentoControlado()
    {
        SloteMultiplayer[] SM = GlobalController.g.Jogadores.ToArray();
        for (int i = 0; i < SM.Length; i++)
        {
            CharacterManager m = SM[i].Manager;
            //if (m.MyView.isMine)
           // {
                m.MudarParaMovimentoDeFora();
                controlls[i] = new ControlledMoveForCharacter(new CaracteristicasDeMovimentacao()
                {
                    caracPulo = new CaracteristicasDePulo(),
                    caracPulo2 = new CaracteristicasDePulo()
                }
                    , m.transform);
                controlls[i].ModificarOndeChegar(alvos[i].position, 6);
            //}
        }

        //state = TalkManagerState.movimentando;
    }

    /*
    public void OnStartControlledMovement(IGameEvent e,string ID)
    {
        StandardSendStringEvent s = e as StandardSendStringEvent;

        if (s.StringContent == ID)
        {
            IniciandoMovimentoControlado();
        }
    }*/


    /// <summary>
    /// Quando o personagem alcança a posição alvo é enviada uma mensagem à rede,
    /// a rede retorna para os clientes com o evento externalMovementPositionOk,
    /// o trigger deve se inscrever nesse evento para que todos os clientes avancem para o proximo estado.
    /// Quando todos os personagens alcaçarem 
    /// a posição retorna true.
    /// </summary>
    /// <param name="ID"> identificação do trigger para mensagens em rede</param>
    /// <returns></returns>
    public bool Movimentando(string ID)
    {
        bool foi = true;
        SloteMultiplayer[] SM = GlobalController.g.Jogadores.ToArray();
        for (int i = 0; i < SM.Length; i++)
        {
            CharacterManager m = SM[i].Manager;
           // if (m.MyView.isMine)
            //{
                if (controlls[i].UpdatePosition())
                {
                    movimentoAlcancado[i] = true;
                //PUN RPC_Listener.RPC(NameOfRPC.ExternalMovementPositionOk, PhotonTargets.Others, ID, i);
                RPC_Listener.ExternalMovementPositionOk( ID, i);
            }
           // }

            foi &= movimentoAlcancado[i];
        }

        return foi;
    }
}
