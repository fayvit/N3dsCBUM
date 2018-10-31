using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DadosDoPersonagem {

    [SerializeField] private int pontosDeVida = 20;
    [SerializeField] private int maxVida = 20;
    [SerializeField] private int pontosDeMana = 20;
    [SerializeField] private int maxMana = 20;

    public int PontosDeVida
    {
        get { return pontosDeVida; }
        set { pontosDeVida = value; }
    }

    public int MaxVida
    {
        get { return maxVida; }
        set { maxVida = value; }
    }

    public int PontosDeMana
    {
        get { return pontosDeMana; }
        set { pontosDeMana = value; }
    }

    public int MaxMana
    {
        get { return maxMana; }
        set { maxMana = value; }
    }

    public void ConsomeMana(int valor)
    {
        if (valor > 0)
        {
            if (pontosDeMana - valor >= 0)
                pontosDeMana -= valor;
            else pontosDeMana = 0;
        }
    }

    public void AplicaDano(int valor)
    {
        if (valor > 0)
        {
            if (pontosDeVida - valor >= 0)
                pontosDeVida -= valor;
            else pontosDeVida = 0;
        }
    }

    public void SetarVidaMax()
    {
        pontosDeVida = maxVida;
    }

    public void SetarManaMax()
    {
        pontosDeMana = maxMana;
    }
}