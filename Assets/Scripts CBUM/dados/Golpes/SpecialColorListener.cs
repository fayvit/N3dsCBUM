using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialColorListener : MonoBehaviour {

    // Update is called once per frame
    void Update () {
		
	}

    /* PUN
    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
       PhotonView pv = GetComponent<PhotonView>();
        Color cor;
        ColorUtility.TryParseHtmlString("#"+(pv.instantiationData[0] as string), out cor);
        ParticleSystem.ColorOverLifetimeModule m = GetComponent<ParticleSystem>().colorOverLifetime;
        m.color = new ParticleSystem.MinMaxGradient(cor);
    }*/
}
