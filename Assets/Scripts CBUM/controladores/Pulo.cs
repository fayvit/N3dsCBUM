using UnityEngine;
using System;

public class Pulo
{
    CaracteristicasDePulo caracteristicas;
    ElementosDeMovimentacao elementos;
    Vector3 movimentoVertical = Vector3.zero;
    float ultimoYFundamentado = 0;
    float tempoDePulo = 0;
    bool estouSubindo = false;

    public Pulo(CaracteristicasDePulo caracteristicas, ElementosDeMovimentacao elementos)
    {
        this.caracteristicas = caracteristicas;
        this.elementos = elementos;
    }

    public bool EstouPulando
    {
        get { return caracteristicas.estouPulando; }
    }

    public bool EstouSubindo
    {
        get { return estouSubindo; }
    }

    public float UltimoYFundamentado
    {
        get { return ultimoYFundamentado; }
    }

    public bool AlcancouTempoMin
    {
        get
        {             
            if (!EstouPulando)
                return true;
            else if (tempoDePulo < 0.25f*caracteristicas.tempoMaxPulo)
                return false;
            else return true;
        }
    }

    public void IniciaAplicaPulo(float ultimoYFundamentado)
    {
        this.ultimoYFundamentado = ultimoYFundamentado;
        caracteristicas.estouPulando = true;
        estouSubindo = true;
        elementos.Controle.Move(Vector3.up * caracteristicas.impulsoInicial);
        elementos.Animador.Play("pulando");
        elementos.Animador.SetBool("pulo", true);
    }

    public void VerificaPulo(Vector3 direcaoMovimento)
    {
        if (direcaoMovimento.magnitude > 1)
            direcaoMovimento.Normalize();

        if (caracteristicas.estavaPulando == false && caracteristicas.estouPulando == true)
        {
            tempoDePulo = 0;
            estouSubindo = true;
        }

        caracteristicas.estavaPulando = caracteristicas.estouPulando;
        tempoDePulo += Time.deltaTime;

        if(elementos.Controle.gameObject.name== "esperandoTeste")
        Debug.Log(
            EstouSubindo
            + " : " +
            elementos.transform.position.y + " : " + UltimoYFundamentado + " : " + caracteristicas.alturaDoPulo
         + " : " +
         tempoDePulo + " : " + caracteristicas.tempoMaxPulo
         );


        if (
            EstouSubindo == true
            &&
            elementos.transform.position.y - UltimoYFundamentado < caracteristicas.alturaDoPulo
         &&
         tempoDePulo < caracteristicas.tempoMaxPulo
         )
        {
            movimentoVertical = (direcaoMovimento * caracteristicas.velocidadeDuranteOPulo
                + Vector3.up * caracteristicas.velocidadeSubindo);

            if (elementos.Controle.velocity.y < 0)
                movimentoVertical.y -= elementos.Controle.velocity.y;

            elementos.Controle.Move(movimentoVertical * Time.deltaTime);

        }
        else if (
          (elementos.transform.position.y - UltimoYFundamentado >= caracteristicas.alturaDoPulo
       ||
       tempoDePulo >= caracteristicas.tempoMaxPulo
       )
          &&
          EstouSubindo == true)
        {
            estouSubindo = false;
            elementos.Controle.Move(movimentoVertical * Time.deltaTime);
        }
        else if (EstouSubindo == false)
        {
            /*
            velocidadeDescendo = Mathf.Lerp(velocidadeDescendo, velMax, amortecimento * Time.deltaTime);
            elementos.Controle.Move((direcaoMovimento * targetSpeed + velocidadeDescendo * Vector3.down) * Time.deltaTime);
            */

            movimentoVertical = Vector3.Lerp(movimentoVertical,
                                             (
                                              Vector3.down * caracteristicas.velocidadeDescendo),
                                             caracteristicas.amortecimentoNaTransicaoDePulo * Time.deltaTime);
            elementos.Controle.Move((direcaoMovimento * caracteristicas.velocidadeDuranteOPulo+movimentoVertical) * Time.deltaTime);

        }


        if (elementos.Controle.gameObject.name == "esperandoTeste")
            Debug.Log(elementos.Controle.collisionFlags);

        if (elementos.Controle.collisionFlags == CollisionFlags.CollidedAbove)
            estouSubindo = false;

    }

    public void NaoEstouPulando()
    {
        
        if (caracteristicas.estouPulando)
            elementos.Animador.SetBool("pulo", false);


        //else
        //elementos.animador.ResetaTriggerDeTocandoOChao();

        caracteristicas.estouPulando = false;
        caracteristicas.estavaPulando = false;
        movimentoVertical = Vector3.zero;
    }
}