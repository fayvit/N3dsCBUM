using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour {

    [SerializeField] private DadosDoPersonagem dados = new DadosDoPersonagem();
    private EstouEmDano emDano;
    private Animator animator;
    //private PhotonView myView;


    public DadosDoPersonagem Dados
    {
        get { return dados; }
        set { dados = value; }
    }

    public EstouEmDano EmDano
    {
        get { return emDano; }
        set { emDano = value; }
    }

    public Animator _Animator
    {
        get { if (animator != null)
                return animator;
            else return GetComponent<Animator>();
        }
        set { animator = value; }
    }

    /* PUN
    public PhotonView MyView
    {
        get {if (myView == null)
                myView = GetComponent<PhotonView>();
            return myView; }
        protected set { myView = value; }
    }*/

    public bool VerificaDerrota()
    {
        if (Dados.PontosDeVida <= 0)
            return true;
        else
            return false;
    }

    public virtual void TomaDano(IGolpeBase golpe,GameObject atacante)
    {
        Debug.Log("implementação externa");
    }
}
