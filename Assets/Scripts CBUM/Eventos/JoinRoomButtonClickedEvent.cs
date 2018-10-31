using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomButtonClickedEvent : IGameEvent
{

    private GameObject _sender;
    private EventKey _key;
    //PUN private RoomInfo _infoRoom;

    public GameObject Sender
    {
        get { return _sender; }
    }

    public EventKey Key
    {
        get { return _key; }
    }

    /* PUN
    public RoomInfo InfoRoom
    {
        get { return _infoRoom; }
        private set { _infoRoom = value; }
    }*/

    public JoinRoomButtonClickedEvent(GameObject sender/*PUN,RoomInfo R*/)
    {
        _sender = sender;
        _key = EventKey.joinRoomButtonClicked;
     //   InfoRoom = R;
    }
}
