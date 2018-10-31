using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoSpawnner : TriggerDisparaInimigos {

    [SerializeField] private SpawnerDeInimigos meuSpawner;

	// Use this for initialization
	protected override  void Start () {
        base.Start();
	}

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
    }

    // Update is called once per frame
    void Update () {
		
	}

    protected override void OnAddEnemyInManager(IGameEvent obj)
    {
        if (obj.Sender == meuSpawner.gameObject)
        {
            AddEnemyInManager(obj as AddEnemyInManagerEvent);
        }
    }

    protected override void OnEnemyDefeatedOrDisabled(IGameEvent e)
    {
        bool foiTuto = VerificaSpawnadosVivos(e.Sender);

        bool vamos = false;

        if (meuSpawner != null)
            vamos = (e.Sender == meuSpawner.gameObject) && foiTuto;
        else
            vamos = foiTuto;

        if (vamos)
        {
            //PUN RPC_Listener.RPC(NameOfRPC.LiberarCameraEmX, PhotonTargets.Others);
            RPC_Listener.LiberarCameraEmX();
            AplicadorDeCamera.c.CamB.LiberarX(false);
            Destroy(gameObject);            
        }

        
    }

    protected override void TriggerActivate()
    {
        meuSpawner.gameObject.SetActive(true);
        base.TriggerActivate();
    }
}
