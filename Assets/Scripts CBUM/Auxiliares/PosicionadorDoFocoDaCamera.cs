using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionadorDoFocoDaCamera : MonoBehaviour {

    private Transform[] S;
    private float maxDistance = 20;

    public float MaxDistance
    {
        get { return maxDistance; }
        set { maxDistance = value; }
    }

    // Use this for initialization
    void Start() {
        AtualizaJogadores();
        EventAgregator.AddListener(EventKey.listaDeJogadoresAtualizada, OnPlayerListUpdate);
    }

    void OnPlayerListUpdate(IGameEvent e)
    {
        AtualizaJogadores();
    }

    void AtualizaJogadores()
    {
        S = new Transform[GlobalController.g.Jogadores.Count];
        for (int i = 0; i < GlobalController.g.Jogadores.Count; i++)
        {
            if(GlobalController.g.Jogadores[i].Manager!=null)
                S[i] = GlobalController.g.Jogadores[i].Manager.transform;
        }
        //S = GlobalController.g.Jogadores.ToArray();
    }

// Update is called once per frame
void Update () {
        float distX = 0;
        float distY = 0;
        float distZ = 0;
        float posX = 0;
        float posZ = 0;
        float posY = 0;
        float val = 0;

        
        for (int i = 0; i < S.Length; i++)
        {
            if (S[i] != null)
            {
                posX = S[i].position.x;
                posY = S[i].position.y;
                posZ = S[i].position.z;
            }
        }

        for (int i = 0; i < S.Length; i++)
        {
            for(int j=0;j<i;j++)
                if (S[i] != null && S[j]!=null)
                {
                    val = S[i].position.x - S[j].position.x;
                    if (Mathf.Abs(val) > distX && Mathf.Abs(val) < maxDistance)
                    {
                        distX = Mathf.Abs(val);
                        posX = S[j].position.x + 0.5f * val;
                    }
                    else if (Mathf.Abs(val) >= maxDistance)
                    {
                        distX = Mathf.Abs(val);
                        posX = transform.position.x;
                    }


                    val = S[i].position.z - S[j].position.z;
                    if (Mathf.Abs(val) > distZ)
                    {
                        distZ = Mathf.Abs(val);
                        posZ = S[j].position.z + 0.5f * val;
                        if (distX > maxDistance && transform.position.z < posZ)
                            posZ = transform.position.z;
                    }

                    val = S[i].position.y - S[j].position.y;
                    if (Mathf.Abs(val) > distY)
                    {
                        distY = Mathf.Abs(val);
                        posY = S[j].position.y + 0.5f * val;
                    }
                }
        }

        transform.position = Vector3.Lerp(transform.position,new Vector3(posX,posY,posZ),2.5f*Time.deltaTime);
	}
}
