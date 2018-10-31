using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamageEvent : IGameEvent
{
    private CharacterManager _sender;
    private EventKey key;

    public GameObject Sender
    {
        get
        {
            return _sender.gameObject;
        }
    }

    public CharacterManager ManagerSender
    {
        get { return _sender; }
    }

    public EventKey Key
    {
        get
        {
            return key;
        }
    }

    public CharacterDamageEvent(CharacterManager sender)
    {
        key = EventKey.heroDamage;
        _sender = sender;
    }
}