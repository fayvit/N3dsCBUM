using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollManager
{
    private Animator animador;
    private CharacterController controle;
    private Vector3 moveDir;
    private Vector3 centerRoll = Vector3.zero;
    private Vector3 standardCenter = Vector3.zero;
    private float vel = 10;
    private float tempoDecorrido = 0;
    private float heightRoll = 1;
    private float heightStandard = 1;

    public RollManager(Animator A,CharacterController C,float heightRoll,float yCenterRoll)
    {
        EventAgregator.AddListener(EventKey.heroDamage, FinalizaRollManager);

        animador = A;
        controle = C;
        this.heightRoll = heightRoll;
        this.centerRoll = new Vector3(0, yCenterRoll, 0);

        heightStandard = C.height;
        standardCenter = C.center;
    }

    public bool UpdateRoll()
    {
        tempoDecorrido += Time.deltaTime;

        if (tempoDecorrido > 0.53f)
        {
            controle.height = heightStandard;
            controle.center = standardCenter;
            return true;
        }

        controle.Move(moveDir * Time.deltaTime * vel);
        return false;
    }

    public void StartRoll(Vector3 moveDir)
    {
        this.moveDir = moveDir;
        StartRoll();
    }
    public void StartRoll(string dir = "f")
    {
        tempoDecorrido = 0;
        int action = 0;
        switch (dir)
        {
            case "f":
                dir = "rollForward";
                action = 2;
            break;
            case "b":
                dir = "rollBack";
                action = 0;
            break;
            case "l":
                dir = "rollLeft";
                action = 3;
            break;
            case "r":
                dir = "rollRight";
                action = 1;
            break;
        }

        /*PUN RPC_Listener.RPC(NameOfRPC.EventPublisher, PhotonTargets.All, EventKey.initRoll, 
            BytesTransform.ToBytes(
            new StandardSendIntAndStringEvent(controle.gameObject, animador.GetComponent<PhotonView>().viewID,dir, EventKey.initRoll)));*/
        RPC_Listener.EventPublisher(EventKey.initRoll,
        BytesTransform.ToBytes(
        new StandardSendIntAndStringEvent(controle.gameObject, 0, dir, EventKey.initRoll)));

        animador.SetInteger("action", action);
        animador.SetTrigger("Roll");
        controle.height = heightRoll;
        controle.center = centerRoll;
    }

    public void EventInitRoll(GameObject pv, string dir)
    {
        MonoBehaviour.Destroy(MonoBehaviour.Instantiate(PrefabList.Load(PrefabListName.rollsound)), 3);

       
        pv.GetComponent<Animator>().Play(dir);

        GameController.g.StartCoroutine(PoeiraDoMovimento(pv.transform));
    }

    /* PUN
    public void EventInitRoll(PhotonView pv,string dir)
    {
        MonoBehaviour.Destroy(MonoBehaviour.Instantiate(PrefabList.Load(PrefabListName.rollsound)), 3);
        
        if (!pv.isMine)
        {
            pv.GetComponent<Animator>().Play(dir);
            
        }

        GameController.g.StartCoroutine(PoeiraDoMovimento(pv.transform));
    }*/

    IEnumerator PoeiraDoMovimento(Transform T)
    {
        yield return new WaitForSeconds(0.15f);
        Quaternion Q = (moveDir!=Vector3.zero)?Quaternion.LookRotation(moveDir):T.rotation;
        
        MonoBehaviour.Destroy(
            MonoBehaviour.Instantiate(PrefabList.Load(PrefabListName.poeiraDoRolar),T.position,Q), 3
            );

        yield return new WaitForSeconds(0.1f);

        MonoBehaviour.Destroy(
            MonoBehaviour.Instantiate(PrefabList.Load(PrefabListName.poeiraDoRolar), T.position , Q), 3
            );

        yield return new WaitForSeconds(0.1f);

        MonoBehaviour.Destroy(
            MonoBehaviour.Instantiate(PrefabList.Load(PrefabListName.poeiraDoRolar), T.position,Q), 3
            );


    }

    public string SetarDirecao(Transform T,int numControl)
    {
        Vector3 dir = CommandReader.VetorDirecao(numControl);
        string retorno = "f";
        moveDir = T.forward;        

        if (Vector3.Angle(dir, T.forward) < 45)
        {
            retorno = "f";
        }
        else if (Vector3.Angle(dir, T.forward) > 135)
        {
            moveDir = -T.forward;
            retorno = "b";
        }
        else if (Vector3.Dot(dir, T.right) > 0)
        {
            moveDir = T.right;
            retorno = "r";
        }
        else if (Vector3.Dot(dir, T.right) < 0)
        {
            moveDir = -T.right;
            retorno = "l";
        }
        else retorno = "f";
        
        return retorno;
    }

    public void FinalizaRollManager(IGameEvent e)
    {
        if (e.Sender == animador.gameObject)
        {
            tempoDecorrido = 0;
            controle.height = heightStandard;
            controle.center = standardCenter;
        }
    }

    public void LimparEventos()
    {
        GameController.g.StartCoroutine(ELimpaEventos());
    }

    IEnumerator ELimpaEventos()
    {
        yield return new WaitForEndOfFrame();
        EventAgregator.RemoveListener(EventKey.heroDamage, FinalizaRollManager);
    }
}