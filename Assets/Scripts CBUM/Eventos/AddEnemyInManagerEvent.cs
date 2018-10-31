using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemyInManagerEvent : IGameEvent
{
    private GameObject _sender;
    private EventKey key;
    private EnemyBase[] _ids;

    public GameObject Sender
    {
        get
        {
            return _sender;
        }
    }

    public EnemyBase[] IDs
    {
        get { return _ids; }
    }

    public EventKey Key
    {
        get
        {
            return key;
        }
    }

    public AddEnemyInManagerEvent(GameObject sender,EnemyBase[] ids)
    {
        key = EventKey.addEnemyInManager;
        _sender = sender;
        _ids = ids;
    }
}