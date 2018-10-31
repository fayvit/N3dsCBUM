using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class SceneLoader : MonoBehaviour
{

    [SerializeField] private LoadBar loadBar;

    //private SaveDates S;
    private AsyncOperation[] a2;
    private FasesDoLoad fase = FasesDoLoad.emEspera;
    private bool podeIr = false;
    //private int indiceDoJogo = 0;
    private float tempo = 0;

    private const float tempoMin = 0.25f;

    private enum FasesDoLoad
    {
        emEspera,
        carregando,
        escurecendo,
        clareando
    }

    public void CenaDoCarregamento(int indice)
    {
        DontDestroyOnLoad(gameObject);
        //indiceDoJogo = indice;
        
        SceneManager.LoadSceneAsync("CenaDeCarregamento");
        
        SceneManager.sceneLoaded += IniciarCarregamento;
    }

    void IniciarCarregamento(Scene cena, LoadSceneMode mode)
    {
        
        loadBar = FindObjectOfType<LoadBar>();
        loadBar.Init();

        /*
        SceneManager.LoadSceneAsync("testeMapa");
        SceneManager.sceneLoaded -= IniciarCarregamento;
        SceneManager.sceneLoaded += CarregouComuns;
        */

        ComunsCarregado();
        SceneManager.sceneLoaded -= IniciarCarregamento;
    }

    void CarregouComuns(Scene cena, LoadSceneMode mode)
    {
        ComunsCarregado();
    }

    void ComunsCarregado()
    {

        fase = FasesDoLoad.carregando;


        a2 = new AsyncOperation[1];
        a2[0] = SceneManager.LoadSceneAsync("testeMapa");
  //      a2[1] = SceneManager.LoadSceneAsync(NomesCenas.katidsVsTempleZone.ToString(), LoadSceneMode.Additive);

        Time.timeScale = 0;

        SceneManager.sceneLoaded -= CarregouComuns;
        SceneManager.sceneLoaded += SetarCenaPrincipal;

    }

    void SetarCenaPrincipal(Scene scene, LoadSceneMode mode)
    {
        /*RPC_Listener.RPC(NameOfRPC.ColocandoListaDeJogadoresNaRede, PhotonTargets.All, BytesTransform.ToBytes(
            GlobalController.g.Jogadores
            ));*/

        RPC_Listener.ColocandoListaDeJogadoresNaRede(BytesTransform.ToBytes(
            GlobalController.g.Jogadores
            ));

        podeIr = true;
        InvocarSetScene(scene);
        SceneManager.sceneLoaded -= SetarCenaPrincipal;

        /*
            O fim do carregamento (Setar cena proncipal) é um evento

        Estão interessados nesse evento:

        CharacterManager ---> posiciona o personagem e preenche os espaços com os dados
        AplicadorDeCamera ---> posiciona a camera de acordo com o manager
        GameController ---> preenche variaveis chaves do jogo

        **** Todos eles baseados no SaveDates que tras essas informações ***

        */
        
           
    }

    IEnumerator SetarScene(Scene scene)
    {
        yield return new WaitForSeconds(0.5f);
        InvocarSetScene(scene);
    }

    public void InvocarSetScene(Scene scene)
    {
        
        SceneManager.SetActiveScene(scene);
        
        if (SceneManager.GetActiveScene() != scene)
            StartCoroutine(SetarScene(scene));

    }

    public void Update()
    {
        switch (fase)
        {
            case FasesDoLoad.carregando:

                tempo += Time.fixedDeltaTime;

                float progresso = 0;

                for (int i = 0; i < a2.Length; i++)
                {
                    progresso += a2[i].progress;
                }

                progresso /= a2.Length;

                //Debug.Log(progresso + " : " + (tempo / tempoMin) + " : " + Mathf.Min(progresso, tempo / tempoMin, 1));

                
                loadBar.ValorParaBarra(Mathf.Min(progresso, tempo / tempoMin, 1));

                if (podeIr && tempo >= tempoMin)
                {
                  //  GameObject go = GameObject.Find("EventSystem");
                    //if (go)
                      //  SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneByName("comunsDeFase"));

                    FadeView pm = GameController.g.gameObject.AddComponent<FadeView>();
                    pm.vel = 2;
                    fase = FasesDoLoad.escurecendo;
                    tempo = 0;
                }

                break;
            case FasesDoLoad.escurecendo:
                tempo += Time.fixedDeltaTime;
                if (tempo > 0.95f)
                {
                    /*
                        O fim do escurecimento é um evento

                        estão interessados nesse evento:

                        InformaçõesDoEvento[*talvez pois é estatico*] --> modifica elementos comuns de cena de acordo com a cena
                        GameController --> Salva o inicio do jogo, pode setar a cena principal
                     */

                    //RPC_Listener.RPC(NameOfRPC.ColocandoPlayerNaCena, PhotonTargets.All);
                    RPC_Listener.ColocandoPlayerNaCena();

                    GameObject.FindObjectOfType<FadeView>().entrando = false;
                    GameObject.Find("LoadCanvas").GetComponent<Canvas>().enabled = false;
                    fase = FasesDoLoad.clareando;
                    Time.timeScale = 1;
                    //SceneManager.UnloadSceneAsync("CenaDeCarregamento");
                    tempo = 0;
                }
            break;
            case FasesDoLoad.clareando:
                tempo += Time.fixedDeltaTime;
                if (tempo > 0.5f)
                {
                    //PUn PhotonNetwork.Destroy(loadBar.transform.parent.gameObject);
                    Destroy(loadBar.transform.parent.gameObject);
                    Destroy(gameObject);
                }
            break;
        }
    }
}
