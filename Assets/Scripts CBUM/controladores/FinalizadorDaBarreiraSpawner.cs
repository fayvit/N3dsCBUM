using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalizadorDaBarreiraSpawner : MonoBehaviour {

    [SerializeField] private GameObject particulaDaAcao;
    [SerializeField] private GameObject particulaDaFinalizacao;
    [SerializeField] private GameObject spawnerDeInteresse;
    [SerializeField] private GameObject barreiraDestrutivel;

	// Use this for initialization
	void Start () {
        EventAgregator.AddListener(EventKey.enemyDefeated,OnSpawnerDefeated);
	}

    void OnSpawnerDefeated(IGameEvent e)
    {
        if (e.Sender == spawnerDeInteresse)
        {
            particulaDaAcao.SetActive(true);

            Invoke("ParticulaFinalizadora", 2.5f);
            Invoke("InsereSomDeDestruicao", 1.25f);
        }
    }

    void InsereSomDeDestruicao()
    {
        Destroy(
        Instantiate(PrefabList.Load(PrefabListName.rockDestroyedSound), transform.position, Quaternion.identity), 3);
    }

    void ParticulaFinalizadora()
    {
        InsereSomDeDestruicao();
        particulaDaFinalizacao.SetActive(true);
        Destroy(barreiraDestrutivel);
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
