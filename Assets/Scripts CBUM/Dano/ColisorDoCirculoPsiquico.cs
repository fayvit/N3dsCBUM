using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisorDoCirculoPsiquico : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            EventAgregator.Publish(new StandardGameEvent(other.gameObject,EventKey.psicoCircleCharacterHit));
        }
    }
}
