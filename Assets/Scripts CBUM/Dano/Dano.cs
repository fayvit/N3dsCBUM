using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dano  {

    public static void VerificaDano(GameObject atacado, GameObject atacante, IGolpeBase golpe)
    {
        /*
        if (atacado.tag == "eventoComGolpe" && !GameController.g.estaEmLuta)
        {
            atacado.GetComponent<EventoComGolpe>().DisparaEvento(golpe.Nome);
        }
        */

        golpe.DirDeREpulsao = atacante.transform.forward;
        CharacterBase chB = atacado.GetComponent<CharacterBase>();
        if (chB /*&& !GameController.g.UsandoItemOuTrocandoCriature*/)
        {
            if (chB.Dados.PontosDeVida > 0)
            {
                AplicaDano(chB, atacante, golpe);
                //VerificaComportamentoDeIA(GdC, atacante); não tinha implementação no original
            }           
        }
    }

    public static void AplicaDano(CharacterBase doAtacado , GameObject atacante, IGolpeBase golpe)
    {
        doAtacado.TomaDano(golpe,atacante);        

        //CalculaDano(doAtacado, atacante, golpe);

//        InsereEstouEmDano(doAtacado, animatorDoAtacado, golpe);

 //       VerificaVida(atacante, doAtacado, animatorDoAtacado);

    }

    /*
    static void CalculaDano(CharacterBase doAtacado,GameObject atacante,IGolpeBase golpe)
    {

    }*/
}
