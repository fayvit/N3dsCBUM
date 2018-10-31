using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageEvent : IGameEvent {

    private EnemyBase _sender;
    private EventKey key;

    public GameObject Sender
    {
        get
        {
            return _sender.gameObject;
        }
    }

    public EventKey Key
    {
        get
        {
            return key;
        }
    }

    public EnemyDamageEvent(EnemyBase sender)
    {
        key = EventKey.enemyDamage;
        _sender = sender;
    }

    
}
