using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable ]
public class GolpeEspecialDoFelixcat:GolpeRefinadoBase
{
    private bool[] foiOsAtaques = new bool[4];
    private int cont = 0;
    private Estadodaqui estado = Estadodaqui.parado;
    private PrefabListName[] prefabDosAtaques = new PrefabListName[3]
        {
            PrefabListName.ataqueEmCirculoPuloAlto,
            PrefabListName.ataqueEmCirculoPuloBaixo,
            PrefabListName.ataqueEmCirculoPuloRolar
        };
    [System.NonSerialized] private GameObject enemy;
    
    private enum Estadodaqui
    {
        parado,
        iniciando,
        spawnandoAtaques,
        finalizando
    }

    public GolpeEspecialDoFelixcat()
    {
        //ID = GolpeId.jab;
        ValorDeDano = 2;
        //TempoHit = .5f;
        //ExitHitTime = .25f;
        TempoDeAtrasoParaCollider = 0;
        TempoDeVidaDoCollider = 5f;
        DeslocamentoDoColisor = 0;
        DistanciaDeRepulsao = 0;
        VelocidadeDeRepulsao = 10;
        TempoNoDano = .9f;
        DanoEmAliado = false;
        //NomeAnimaGolpe = "atk1";
        //IDAnimaGolpe = "punch_20";
        NoImpacto = NoImpactoDeDano.impactoDeEnergia;
        TempoEmAnima = 1f;
        NomeAnima = "emissor";
        BloquearMultHit = false;
        //nomeProjetil = PrefabListName.projetilDeAgua.ToString();
    }

    public override void StartAttack(GameObject G, Transform alvo)
    {
        this.enemy = G;
        cont = 0;
        TempoDecorrido = 0;
        DadosDoPersonagem dados = enemy.GetComponent<EnemyBase>().Dados;

        for (int i = 0; i < 4; i++)
        {
            if (!foiOsAtaques[i] && dados.PontosDeVida < (0.8f - 0.2f * i) * dados.MaxVida)
                foiOsAtaques[i] = true;

            estado = Estadodaqui.iniciando;
        }
    }

    public override bool UpdateAttack()
    {

        switch (estado)
        {
            case Estadodaqui.iniciando:
                /*PUN
                RPC_Listener.RPC(
                    NameOfRPC.EventPublisher, 
                    PhotonTargets.Others,
                    EventKey.bossSpecialAttackStart,
                    BytesTransform.ToBytes(
                    new StandardSendIntEvent(enemy, enemy.GetComponent<PhotonView>().viewID, EventKey.bossSpecialAttackStart)));
                    */
                EventAgregator.Publish(
                EventKey.bossSpecialAttackStart,
                new StandardSendIntEvent(enemy, 1, EventKey.bossSpecialAttackStart));
                StartSpecialAttack(enemy.GetComponent<Animator>());

                estado = Estadodaqui.parado;
                GameController.g.StartCoroutine(MudaParaSpawnandoAtaque());
            break;
            case Estadodaqui.spawnandoAtaques:
              //  if (PhotonNetwork.isMasterClient)
                {
                    int intervaloDeCirculos = 7;
                    TempoDecorrido += Time.deltaTime;
                    if (TempoDecorrido > intervaloDeCirculos && cont < 3)
                    {
                        int qual = Random.Range(0, 3);
                        RaycastHit hit;
                        Vector3 pos;
                        if (Physics.Raycast(enemy.transform.position, Vector3.down, out hit))
                        {
                            pos = hit.point;
                        }
                        else
                        {
                            pos = enemy.transform.position;
                        }

                        GameObject G = (GameObject)MonoBehaviour.Instantiate(PrefabList.Load(prefabDosAtaques[qual]), pos, Quaternion.identity);
                        CirculoPsiquico[] cs = G.GetComponentsInChildren<CirculoPsiquico>();
                        foreach (CirculoPsiquico c in cs)
                        {
                            c.Dono = enemy;
                            c.EsseGolpe = this;
                        }
                        cont++;
                        TempoDecorrido = 0;
                    }
                    else if (TempoDecorrido > intervaloDeCirculos && cont >= 3)
                    {
                        MonoBehaviour.Destroy(GameObject.Find("teletransporte"));
                        return true;
                    }

                }
            break;
        }
        return false;
	}

    IEnumerator MudaParaSpawnandoAtaque()
    {
        yield return new WaitForSeconds(0.75f);
        estado = Estadodaqui.spawnandoAtaques;
        TempoDecorrido = 10;
    }

    public static void StartSpecialAttack(Animator A)
    {
        A.SetInteger("action", 0);
        A.Play("saltoDoLevitar");

        GameController.g.StartCoroutine(EndStartAnimation(A));
    }

    static IEnumerator EndStartAnimation(Animator A)
    {
        yield return new WaitForSeconds(0.25f);
        A.SetInteger("action", 1);
        RaycastHit hit = new RaycastHit();
        GameObject G;
        Vector3 pos;
        if (Physics.Raycast(A.transform.position, Vector3.down, out hit))
        {
            pos = hit.point;
        }
        else
        {
            pos = A.transform.position;
        }

        G = (GameObject)MonoBehaviour.Instantiate(PrefabList.Load(PrefabListName.teletransporte), pos, Quaternion.identity);
        G.name = "teletransporte";

        MonoBehaviour.Destroy(
        MonoBehaviour.Instantiate(PrefabList.Load(PrefabListName.enfaseDoAtkEspecial), A.transform.position + 4 * Vector3.up, Quaternion.identity), 5);
        
        AplicadorDeCamera.c.ShakeCam(15,1);

    }

    public bool VerificaGolpeEx(DadosDoPersonagem dados)
    {
        for (int i = 0; i < 4; i++)
        {
            if (!foiOsAtaques[i] && dados.PontosDeVida < (0.8f - 0.2f * i) * dados.MaxVida)
                return true;
        }

        return false;
    }
}
