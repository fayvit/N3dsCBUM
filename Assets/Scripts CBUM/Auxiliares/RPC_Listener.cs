using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPC_Listener : MonoBehaviour {

    /*PUN
    private static PhotonView myView;

    private void Start()
    {
        myView = GetComponent<PhotonView>();
    }

    public static void RPC(NameOfRPC metodo, PhotonTargets targets, params object[] p)
    {
        PhotonNetwork.RPC(myView, metodo.ToString(), targets, false, p);
    }

    public static void RPC(NameOfRPC metodo, PhotonPlayer target, params object[] p)
    {
        PhotonNetwork.RPC(myView, metodo.ToString(), target, false, p);
    }*/

    //[PunRPC]
    public static void BasicEnemyDefeated(int viewID)
    {
        EventAgregator.Publish(EventKey.lanEnemyDefeated, new StandardSendIntEvent(
            GameController.g.gameObject, viewID, EventKey.lanEnemyDefeated));
    }

    //[PunRPC]
    public static void ColocandoPlayerNaCena()
    {
        GameController.g.ColocandoPlayersNaCena();
    }

    /*[PunRPC]
    void Coloca_HitCollider(byte[] bGolpe, int idDono)
    {
        GameObject dono = PhotonView.Find(idDono).gameObject;
        IGolpeBase golpe = BytesTransform.ToObject<IGolpeBase>(bGolpe);
        float prolongue = golpe.TempoDeVidaDoCollider;
        float avante = golpe.DeslocamentoDoColisor;

        GameObject G = MonoBehaviour.Instantiate(
                    (GameObject)Resources.Load("hitCollider"),
                    dono.transform.position + 1.4f * avante * dono.transform.forward + Vector3.up,
                    Quaternion.LookRotation(dono.transform.forward, Vector3.Cross(Camera.main.transform.forward, dono.transform.forward)));
        ColisorDeDano col = G.transform.GetChild(0).gameObject.AddComponent<ColisorDeDano>();

        golpe.DirDeREpulsao = dono.transform.forward;
        G.transform.parent = dono.transform;
        col.EsseGolpe = golpe;
        col.Dono = dono;

        Destroy(Instantiate((GameObject)Resources.Load("attackSound")), 3);
        Destroy(G, prolongue);
    }*/

    public static void Coloca_HitCollider(byte[] bGolpe, GameObject dono)
    {
        IGolpeBase golpe = BytesTransform.ToObject<IGolpeBase>(bGolpe);
        float prolongue = golpe.TempoDeVidaDoCollider;
        float avante = golpe.DeslocamentoDoColisor;

        GameObject G = MonoBehaviour.Instantiate(
                    (GameObject)Resources.Load("hitCollider"),
                    dono.transform.position + 1.4f * avante * dono.transform.forward + Vector3.up,
                    Quaternion.LookRotation(dono.transform.forward, Vector3.Cross(Camera.main.transform.forward, dono.transform.forward)));
        ColisorDeDano col = G.transform.GetChild(0).gameObject.AddComponent<ColisorDeDano>();

        golpe.DirDeREpulsao = dono.transform.forward;
        G.transform.parent = dono.transform;
        col.EsseGolpe = golpe;
        col.Dono = dono;

        Destroy(Instantiate((GameObject)Resources.Load("attackSound")), 3);
        Destroy(G, prolongue);
    }

    //[PunRPC]
    public static void ColocaParticulaNosClientes(string nomeGolpe, Vector3 position)
    {
        GameController.ColocaParticulas(nomeGolpe, position);
    }

    /*[PunRPC]
    void AnimaPlay(string anima, int viewID)
    {
        PhotonView.Find(viewID).GetComponent<Animator>().Play(anima, 0, 0);
    }*/

    public static void AnimaPlay(string anima, GameObject G)
    {
        G.GetComponent<Animator>().Play(anima, 0, 0);
    }

    /*[PunRPC]
    void AnimaPlayAndSound(string anima, int viewID, string sound)
    {
        Destroy(Instantiate(Resources.Load(sound)), 3);
        AnimaPlay(anima, viewID);
    }*/

    public static void AnimaPlayAndSound(string anima, GameObject G, string sound)
    {
        Destroy(Instantiate(Resources.Load(sound)), 3);
        AnimaPlay(anima, G);
    }

    /*
    [PunRPC]
    void AnimaMorteDoPlayer(string anima, int viewID)
    {
        PhotonView view = PhotonView.Find(viewID);
        view.GetComponent<CapsuleCollider>().enabled = false;
        view.GetComponent<CharacterController>().enabled = false;
        view.GetComponent<Animator>().Play(anima, 0, 0);
    }*/

    /*[PunRPC]
    void AnimaAtk(int viewID, byte[] bGolpe)
    {
        PhotonView.Find(viewID).GetComponent<CharacterManager>().AnimaAtk(BytesTransform.ToObject<IGolpeBase>(bGolpe));
    }*/

    public static void AnimaAtk(GameObject G, byte[] bGolpe)
    {
        G.GetComponent<CharacterManager>().AnimaAtk(BytesTransform.ToObject<IGolpeBase>(bGolpe));
    }

    /*[PunRPC]
    void SpawnarInimigo()
    {
        Vector2 dir = Random.insideUnitCircle;
        Vector3 pos = GameObject.FindWithTag("Player").transform.position + 7.5f * new Vector3(dir.x, 0, dir.y);
        PhotonNetwork.Instantiate("cavaleiro", pos, Quaternion.identity, 0);
    }*/

    public static void SpawnarInimigo()
    {
        Vector2 dir = Random.insideUnitCircle;
        Vector3 pos = GameObject.FindWithTag("Player").transform.position + 7.5f * new Vector3(dir.x, 0, dir.y);
        Instantiate(Resources.Load("cavaleiro"), pos, Quaternion.identity);
    }

    /*[PunRPC]
    void SwitchLifeBarView(int viewID, bool view, float amount)
    {
        PhotonView.Find(viewID).GetComponent<EnemyBase>().SwitchViewLifeBar(view, amount);
    }*/

    public static void SwitchLifeBarView(GameObject G, bool view, float amount)
    {
        G.GetComponent<EnemyBase>().SwitchViewLifeBar(view, amount);
    }

    /*[PunRPC]
    void DestruirEnemyBase(int viewID)
    {
        PhotonView.Find(viewID).GetComponent<EnemyBase>().DestruirEnemyBase();
    }*/

     public static void DestruirEnemyBase(GameObject G)
    {
        G.GetComponent<EnemyBase>().DestruirEnemyBase();
    }

    //[PunRPC]
    public static void Coloca_ColisorDeMagia(byte[] bGolpe,GameObject dono)
    {
        MagicAttackManager.ColocaColisorDeMagia(bGolpe, dono);
    }

    //[PunRPC]
    public static void ModificaHudMana(int numJogador, float quantidade)
    {
        HUDManager.h.AtualizaMagicBar(numJogador, quantidade);
    }

    //[PunRPC]
    public static void ModificaHudLife(int numJogador, float quantidade,float quantidadeDois)
    {
        HUDManager.h.AtualizaLifeBar(numJogador, quantidade);
        HUDManager.h.AtualizaMagicBar(numJogador, quantidadeDois);

    }

    /*[PunRPC]
    void HudDoHeroiSpawnado(int viewID)
    {
        GameObject G = PhotonView.Find(viewID).gameObject;

        EventAgregator.Publish(EventKey.novoHeroiSpawnado, new StandardGameEvent(G, EventKey.novoHeroiSpawnado));
    }*/

    public static void HudDoHeroiSpawnado(GameObject G)
    {
        EventAgregator.Publish(EventKey.novoHeroiSpawnado, new StandardGameEvent(G, EventKey.novoHeroiSpawnado));
    }

    //[PunRPC]
    public static void EuEntreiNasala(string nome, int viewID)
    {
        GlobalController.g.EuEntreiNasala(nome, viewID);
    }

    //[PunRPC]
    public static void AtualizaPlayersInGameForClients(byte[] players)
    {
        Debug.LogWarning("precisa atualizar a lista de jogadores na rede");
        //GlobalController.g.PlayersInGame = BytesTransform.ToObject<List<PlayerInGame>>(players);
    }

//    [PunRPC]
    public static void ElementosDaDestruicaoRockCube(Vector3 pos)
    {
        PedrasDestrutiveisIniciais.ElementosDaDestruicao(pos);
    }

    //[PunRPC]
    public static void ColocandoListaDeJogadoresNaRede(byte[] b)
    {
        Debug.Log("Quero a lista de jogadores");
        GlobalController.g.Jogadores = BytesTransform.ToObject<List<SloteMultiplayer>>(b);
    }

    //[PunRPC]
    public static void EnemyTriggerIaCollided(string ID)
    {
        EventAgregator.Publish(EventKey.enemyTriggerIaCollided, new StandardSendStringEvent(
            GameController.g.gameObject, ID, EventKey.enemyTriggerIaCollided));
    }

    /*[PunRPC]
    void SpawnedEnemy(int viewId,Vector3 pos)
    {
        SpawnerDeInimigos.SpawnedEnemy(PhotonView.Find(viewId).gameObject,pos);
    }*/

    public static void SpawnedEnemy(GameObject G, Vector3 pos)
    {
        SpawnerDeInimigos.SpawnedEnemy(G, pos);
    }

    /*[PunRPC]
    void AddRefinedCollider(int id,byte[] b)
    {
        BytesTransform.ToObject<GolpeRefinadoBase>(b).ColocaMeuHitCollider(PhotonView.Find(id).gameObject);
    }*/

    public static void AddRefinedCollider(GameObject G, byte[] b)
    {
        BytesTransform.ToObject<GolpeRefinadoBase>(b).ColocaMeuHitCollider(G);
    }

    //[PunRPC]
    public static void RequestExternalMovement(string ID)
    {
        EventAgregator.Publish(new StandardSendStringEvent(
            GameController.g.gameObject,ID,EventKey.startControlledMovement));
    }

    //[PunRPC]
    public static void ExternalMovementPositionOk(string ID,int qual)
    {
        EventAgregator.Publish(new StandardSendIntAndStringEvent(
            GameController.g.gameObject,qual, ID, EventKey.externalMovementPositionOk));
    }

    //[PunRPC]
    public static void FinalTalkEvent(string ID)
    {
        EventAgregator.Publish(new StandardSendStringEvent(
            GameController.g.gameObject,ID,EventKey.finallyTalk));
    }

    //[PunRPC]
    public static void LiberarCameraEmX()
    {
        AplicadorDeCamera.c.CamB.LiberarX(false);
    }

    /*[PunRPC]
    void PegueColetavel(int viewIdColetavel,int viewIdColetor)
    {
        PhotonView.Find(viewIdColetavel).GetComponent<Coletavel>().ElementosDoColetavel(
            PhotonView.Find(viewIdColetor)
            );
    }*/

    public static void PegueColetavel(GameObject viewIdColetavel, GameObject viewIdColetor)
    {
        viewIdColetavel.GetComponent<Coletavel>().ElementosDoColetavel(
            viewIdColetor
            );
    }

    //[PunRPC]
    public static void FinallyPresentation(string IDtrigger)
    {
        EventAgregator.Publish(new StandardSendStringEvent(
            GameController.g.gameObject, IDtrigger, EventKey.finallyPresentation));
    }

    /*[PunRPC]
    void FinallyBossWalk(string IDtrigger, string anima, int viewID)
    {
        AnimaPlay(anima, viewID);
        EventAgregator.Publish(new StandardSendStringEvent(gameObject, IDtrigger, EventKey.finallyBosswalk));
    }*/
    public static void FinallyBossWalk(string IDtrigger, string anima, GameObject G)
    {
        AnimaPlay(anima, G);
        EventAgregator.Publish(new StandardSendStringEvent(
            GameController.g.gameObject, IDtrigger, EventKey.finallyBosswalk));
    }


    //[PunRPC]
    public static void EventPublisher(EventKey key,byte[] e)
    {
        EventAgregator.Publish(key,BytesTransform.ToObject<IGameEvent>(e));
    }

    /*
    [PunRPC]
    void CarregueCenas()
    {
        Destroy(GameObject.Find("Directional Light"));
        Destroy(GameObject.Find("Main Camera"));
        Destroy(GameObject.Find("Canvas"));

        //PhotonNetwork.LoadLevelAsync("testeMapa");

        SceneManager.LoadSceneAsync("comunsDeCena", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("testeMapa", LoadSceneMode.Additive);
        SceneManager.sceneLoaded += QueroACena;

    }*/
}

public enum NameOfRPC
{
    ElementosDaDestruicaoRockCube,
    DestruirEnemyBase,
    SwitchLifeBarView,
    AnimaPlay,
    SpawnarInimigo,
    ModificaHudLife,
    ModificaHudMana,
    Coloca_ColisorDeMagia,
    AnimaMorteDoPlayer,
    HudDoHeroiSpawnado,
    AnimaPlayAndSound,
    ColocaParticulaNosClientes,
    EuEntreiNasala,
    Coloca_HitCollider,
    ColocandoPlayerNaCena,
    ColocandoListaDeJogadoresNaRede,
    AnimaAtk,
    BasicEnemyDefeated,
    EnemyTriggerIaCollided,
    SpawnedEnemy,
    EnableSpawnedEnemy,
    AddRefinedCollider,
    RequestExternalMovement,
    ExternalMovementPositionOk,
    FinalTalkEvent,
    LiberarCameraEmX,
    PegueColetavel,
    FinallyPresentation,
    FinallyBossWalk,
    EventPublisher
}
