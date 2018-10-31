using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinedRoomEvent : IGameEvent {
    private GameObject _sender;
    private EventKey _key;
    private int _viewID;
    private string _nome;

   

    public EventKey Key
    {
        get { return _key; }
    }

    public int ViewID
    {
        get { return _viewID; }
        private set { _viewID = value; }
    }

    public GameObject Sender
    {
        get { return _sender; }
        private set { _sender = value; }
    }

    public string Nome
    {
        get { return _nome; }
        private set { _nome = value; }
    }

    public JoinedRoomEvent(GameObject sender,int viewID,string nome)
    {
        _key = EventKey.joinedRoom;
        _sender = sender;
        _viewID = viewID;
        _nome = nome;

    }
}
