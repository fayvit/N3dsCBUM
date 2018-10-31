using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDisparaInimigos : MonoBehaviour {

    [SerializeField] private List<EnemyBase> enemies;
    [SerializeField] private int xAlvo;
    [SerializeField] private string ID;

    private bool[] derrotados;
    private bool foiDisparado = false;

	// Use this for initialization
	protected virtual void Start () {
        derrotados = new bool[enemies.Count];
        
        EventAgregator.AddListener(EventKey.enemyDefeated, OnEnemyDefeatedOrDisabled);
        EventAgregator.AddListener(EventKey.enemyDisable, OnEnemyDefeatedOrDisabled);
        EventAgregator.AddListener(EventKey.addEnemyInManager, OnAddEnemyInManager);
        EventAgregator.AddListener(EventKey.enemyTriggerIaCollided, OnTriggerActivateForLan);
    }

    protected virtual void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.enemyDefeated, OnEnemyDefeatedOrDisabled);
        EventAgregator.RemoveListener(EventKey.enemyDisable, OnEnemyDefeatedOrDisabled);
        EventAgregator.RemoveListener(EventKey.addEnemyInManager, OnAddEnemyInManager);
        EventAgregator.RemoveListener(EventKey.enemyTriggerIaCollided, OnTriggerActivateForLan);
    }

    protected virtual void OnValidate()
    {
        BuscadorDeID.Validate(ref ID,this);
    }

    // Update is called once per frame
    void Update () {
		
	}

    protected virtual void OnAddEnemyInManager(IGameEvent obj)
    {
        Debug.Log("Não Contem");
        if (enemies.Contains(obj.Sender.GetComponent<EnemyBase>()))
        {
            Debug.Log("Contem");
            AddEnemyInManagerEvent a = obj as AddEnemyInManagerEvent;
            AddEnemyInManager(a);


        }
    }

    protected void AddEnemyInManager(AddEnemyInManagerEvent a)
    {
        
        EnemyBase[] plusEnemies = new EnemyBase[enemies.Count + a.IDs.Length];
        bool[] plusDerrotados = new bool[enemies.Count + a.IDs.Length];

        for (int i = 0; i < enemies.Count; i++)
        {
            plusEnemies[i] = enemies[i];
            plusDerrotados[i] = derrotados[i];
        }

        for (int j = 0; j < a.IDs.Length; j++)
        {
            plusEnemies[j + enemies.Count] = a.IDs[j];
        }

        enemies = new List<EnemyBase>();
        enemies.AddRange(plusEnemies);
        derrotados = plusDerrotados;
    }

    protected bool VerificaSpawnadosVivos(GameObject actor)
    {
        bool foiTuto = true;
        // bool SenderDaqui = false;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                if (actor == enemies[i].gameObject)
                {
                    derrotados[i] = true;
                    //SenderDaqui = true;
                }
            }

            foiTuto &= derrotados[i];
        }

        return foiTuto;
    }

    protected virtual void OnEnemyDefeatedOrDisabled(IGameEvent e)
    {

        bool foiTuto = VerificaSpawnadosVivos(e.Sender);
        /*
        if (SenderDaqui)
        {
            for(int i=0;i<enemies.Count;i++)
                Debug.Log(i + " : " + derrotados[i] + " : Enemy name: " + ((enemies[i]!=null)?enemies[i].gameObject.name:"foi destruido"));
        }

        if(SenderDaqui)
            Debug.Log("numero no vetor de bool: "+derrotados.Length+" numero no vetor de enemies: "+enemies.Count+" Sender: "+e.Sender.name);
*/
        if (foiTuto)
        {
            //PUN RPC_Listener.RPC(NameOfRPC.LiberarCameraEmX, PhotonTargets.Others);
            RPC_Listener.LiberarCameraEmX();
            AplicadorDeCamera.c.CamB.LiberarX(false);
            Destroy(gameObject);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !foiDisparado)
        {
            //PUN RPC_Listener.RPC(NameOfRPC.EnemyTriggerIaCollided, PhotonTargets.Others, ID);
            RPC_Listener.EnemyTriggerIaCollided(ID);
            TriggerActivate();
        }
    }

    protected virtual void TriggerActivate()
    {
       // if (PhotonNetwork.isMasterClient)
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].IniciarIa();
            }

        foiDisparado = true;

        if (xAlvo != 0)
        {
            AplicadorDeCamera.c.CamB.FixarEmX(xAlvo);
        }
    }

    void OnTriggerActivateForLan(IGameEvent e)
    {
        StandardSendStringEvent s = e as StandardSendStringEvent;
        if (s.StringContent == ID)
        {
            TriggerActivate();
        }
    }
}
