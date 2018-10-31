using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDestructible : MonoBehaviour
{
    [SerializeField]private DadosDoPersonagem dados = new DadosDoPersonagem();

    public DadosDoPersonagem Dados
    {
        get { return dados; }
    }

    public virtual void InicieDestruicao(IGameEvent e)
    {
        Debug.Log("[Inicia Destruição] Deve ser implementado pla classe herdeira");
    }
}
