using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class FinalizouChefe {

    [SerializeField] private GameObject canvasDoFim;
    [SerializeField] private Text textoDoPodeIrEmbora;

    private float contadorDeTempo = 0;
    private int fase = 0;

    public string ID;

    public void PararHerois()
    {
        SloteMultiplayer[] SM = GlobalController.g.Jogadores.ToArray();
        for (int i = 0; i < SM.Length; i++)
        {
            CharacterManager m = SM[i].Manager;
          //  if (m.MyView.isMine)
          //  {
                m.MudarParaMovimentoDeFora();
          //  }
        }
    }
    // Use this for initialization
    public void Start()
    {

        //PUN RPC_Listener.RPC(NameOfRPC.EventPublisher, PhotonTargets.Others, EventKey.stopMovementForEndBoss,
        //    BytesTransform.ToBytes(new StandardSendStringEvent(canvasDoFim,ID,EventKey.stopMovementForEndBoss)));

        RPC_Listener.EventPublisher(EventKey.stopMovementForEndBoss, 
            BytesTransform.ToBytes(new StandardSendStringEvent(canvasDoFim, ID, EventKey.stopMovementForEndBoss)));

        PararHerois();
    }
	
	// Update is called once per frame
	public void Update () {
        contadorDeTempo += Time.deltaTime;

        if (fase == 0)
        {
            if (contadorDeTempo > 1)
            {
                canvasDoFim.SetActive(true);
                contadorDeTempo = 0;
                fase = 1;
            }
        }
        else if (fase == 1)
        {
            if (contadorDeTempo > 3)
            {
                textoDoPodeIrEmbora.enabled = true;

                if (Input.anyKeyDown)
                {
                   // if (PhotonNetwork.connected)
                   //     PhotonNetwork.Disconnect();

                    MonoBehaviour.Destroy(GameController.g.gameObject);
                    MonoBehaviour.Destroy(GlobalController.g.gameObject);

                    EventAgregator.ClearListeners();

                    SceneManager.LoadScene("SampleScene");
                }
            }
        }
        

        
	}
}
