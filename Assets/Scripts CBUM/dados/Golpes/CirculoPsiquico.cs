using System.Collections;
using UnityEngine;
public class CirculoPsiquico : ColisorDeDano
{
    //private GameObject emissor;
    [SerializeField] private Color cor;
    private Vector3[] dir = new Vector3[10];
    private Transform[] spawnados = new Transform[10];
    private float contadorDeTempo = 0;

    private void Start()
    {
        /*  Refazer     */

       // EsseGolpe = new GolpeEspecialDoFelixcat();
        //Dono = GameObject.FindWithTag("Enemy");
        
        /*******************************************/


        EventAgregator.AddListener(EventKey.psicoCircleCharacterHit, OnPsicoCharacterHit);
        //emissor = PrefabList.Load(PrefabListName.raioDeAtaque) as GameObject;
        Transform container = new GameObject().transform;
        for (int i = 0; i < 10; i++)
        {
            dir[i] = transform.forward;
            /*PUN GameObject G = PhotonNetwork.Instantiate(PrefabListName.raioDeAtaque.ToString(), transform.position + 2f * transform.forward, transform.rotation,0,
                new object[1] {ColorUtility.ToHtmlStringRGB(cor)}
                );*/

            GameObject G = Instantiate((GameObject)PrefabList.Load(PrefabListName.raioDeAtaque), 
                transform.position + 2f * transform.forward, transform.rotation);

        transform.Rotate(new Vector3(0, 36, 0));
            G.transform.Find("receptor").position = transform.position + 2f * transform.forward;
            spawnados[i] = G.transform;
            G.transform.parent = container;
        
            ParticleSystem.ColorOverLifetimeModule m = G.GetComponent<ParticleSystem>().colorOverLifetime;
            m.color = new ParticleSystem.MinMaxGradient(cor);
        }

        container.parent = transform;
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.psicoCircleCharacterHit, OnPsicoCharacterHit);
    }

    private void OnPsicoCharacterHit(IGameEvent obj)
    {
        StandardGameEvent s = obj as StandardGameEvent;
        FacaImpacto(s.Sender);
    }

    private void Update()
    {
        contadorDeTempo += Time.deltaTime;

        if (contadorDeTempo > 2 && spawnados[0]!=null)
        {
            EsseGolpe.BloquearMultHit = true;
            for (int i = 0; i < 10; i++)
            {
                //Debug.Log(spawnados[i] + ":" + dir[i]);
                spawnados[i].position += dir[i] * Time.deltaTime * 7;
                spawnados[i].Find("receptor").position += (dir[(i + 1) % 10] - dir[i]) * 7 * Time.deltaTime;
                Transform T = spawnados[i].Find("colisor");
                T.position = 0.5f * (spawnados[i].position + spawnados[i].Find("receptor").position);
                Quaternion Q = Quaternion.LookRotation(
                    Vector3.Cross((spawnados[i].position - spawnados[i].Find("receptor").position), Vector3.up)
                    );
                T.rotation = Q;
                T.localScale = new Vector3((spawnados[i].position - spawnados[i].Find("receptor").position).magnitude, 1, 1);

                if (contadorDeTempo > 10)
                {
                    //PUN PhotonNetwork.Destroy(spawnados[i].gameObject);]
                    Destroy(spawnados[i].gameObject);
                }
            }

            if (contadorDeTempo > 10)
            {

                if (transform.GetSiblingIndex() == 0)
                    StartCoroutine(DestruirPai());
                else
                    Destroy(gameObject);

            }
        }
    }

    IEnumerator DestruirPai()
    {
        yield return new WaitForEndOfFrame();
        Destroy(transform.parent.gameObject);
    }
}