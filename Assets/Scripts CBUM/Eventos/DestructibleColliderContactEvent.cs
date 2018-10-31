using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleColliderContactEvent : IGameEvent {

    private EventKey _key;
    private GameObject _sender;
    private IGolpeBase _golpe;

    public GameObject Sender
    {
        get { return _sender; }
    }

    public EventKey Key {
        get { return _key; }
    }

    public IGolpeBase Golpe
    {
        get { return _golpe; }
    }
    

    public DestructibleColliderContactEvent(GameObject G,IGolpeBase golpe)
    {
        _key = EventKey.destructibleDamageColliderContact;
        _golpe = golpe;
        _sender = G;
    }
}
