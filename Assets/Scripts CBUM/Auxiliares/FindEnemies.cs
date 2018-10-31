using UnityEngine;
using System.Collections.Generic;

public class FindEnemies
{
    public static Transform ProcureUmBomAlvo(GameObject esseObjeto, float distancia = 40)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject alvo = null;
        Vector3 vendo = Vector3.zero;
        Vector3 c = Vector3.zero;
        Transform T = esseObjeto.transform;
        List<GameObject> inimigosPerto = new List<GameObject>();
        
        inimigosPerto.AddRange(enemies);
        enemies = GameObject.FindGameObjectsWithTag("Destructible");
        inimigosPerto.AddRange(enemies);
        enemies = inimigosPerto.ToArray();
        inimigosPerto = new List<GameObject>();



        foreach (GameObject enemy in enemies)
        {
          //  if (enemy != esseObjeto)
          //  {
                c = enemy.transform.position;
                vendo = c - T.position;

                if (vendo.sqrMagnitude < Mathf.Pow(distancia, 2))
                    inimigosPerto.Add(enemy);
            //}
        }



        if (inimigosPerto.Count != 0)
        {
            GameObject deMelhorVisao = null;
            GameObject maisPerto = null;

            foreach (GameObject enemy in inimigosPerto)
            {
                c = enemy.transform.position;
                maisPerto = maisPerto != null
                    ?
                        (c - T.position).sqrMagnitude
                        <
                        (maisPerto.transform.position - T.position).sqrMagnitude
                        ?
                        enemy
                        :
                        maisPerto
                        : enemy;

                deMelhorVisao = deMelhorVisao == null
                    ?
                        enemy
                        :
                        Vector3.Dot((c - T.position).normalized,
                                     T.forward)
                        >
                        Vector3.Dot(
                            (deMelhorVisao.transform.position - T.position)
                            .normalized,
                            T.forward
                            )
                        ?
                        enemy
                        :
                        deMelhorVisao;
            }

            if (deMelhorVisao == maisPerto
               &&
               Vector3.Dot(
                (deMelhorVisao.transform.position - T.position).normalized,
                T.forward) > 0)
            {
                alvo = Vector3.Dot((maisPerto.transform.position -
                                    T.position).normalized,
                                   T.forward) > -0.5
                    ? deMelhorVisao : null;
            }
            else
            {
                if (Vector3.Distance(maisPerto.transform.position, T.position) < 0.1f *
                    Vector3.Distance(deMelhorVisao.transform.position, T.position)
                    )
                    alvo = maisPerto;
                else
                    alvo = deMelhorVisao;
                /*
                if ((maisPerto.transform.position - T.position)
                   .sqrMagnitude < 25 &&
                   Vector3.Dot((maisPerto.transform.position -
                             T.position).normalized,
                            T.forward) > -0.5
                   )
                    alvo = maisPerto;
                else
                    alvo = Vector3.Dot((deMelhorVisao.transform.position -
                                        T.position).normalized,
                                       T.forward) > -0.5
                        ? deMelhorVisao : null;
                        */
            }
        }

        //procurouAlvo = true;


        AjudaAtaque(alvo, T);

        return alvo != null ? alvo.transform : null;
    }

    static void AjudaAtaque(GameObject alvo, Transform T)
    {

        Vector3 gira = Vector3.zero;
        if (alvo != null)
        {
            gira = alvo.transform.position - T.position;

            gira.y = 0;
            T.rotation = Quaternion.LookRotation(gira);

        }
    }

    public static List<GameObject> ProximosDoPonto(Vector3 pontoDeProximidade, float distancia = 40)
    {
        List<GameObject> retorno = new List<GameObject>();
        GameObject[] Gs = GameObject.FindGameObjectsWithTag("Enemy");

        retorno.AddRange(Gs);
        Gs = GameObject.FindGameObjectsWithTag("Destructible");
        retorno.AddRange(Gs);
        Gs = retorno.ToArray();
        retorno = new List<GameObject>();

        foreach (GameObject G in Gs)
        {
            if (Vector3.Distance(G.transform.position, pontoDeProximidade) < distancia &&
               Vector3.Distance(G.transform.position, pontoDeProximidade) > 0 &&
               G.GetComponent<EnemyBase>().Dados.PontosDeVida>0)
            {
                retorno.Add(G);
            }
        }
        return retorno;
    }

    public static List<GameObject> ProximosDeMim(GameObject eu, float distancia = 40)
    {
        return ProximosDoPonto(eu.transform.position);
    }
}
