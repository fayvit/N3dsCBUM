using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedRoomEvent : IGameEvent
{
    private GameObject _sender;
    private EventKey _key;
    private Controlador _controlador;
    private string _nomeDoDono;

    public GameObject Sender
    {
        get { return _sender; }
    }

    public EventKey Key
    {
        get { return _key; }
    }

    public string NomeDoDono
    {
        get { return _nomeDoDono; }
    }

    public Controlador Controlador
    {
        get { return _controlador; }
        private set { _controlador = value; }
    }

    public CreatedRoomEvent(GameObject sender,string nomeDoDono,Controlador controlador = Controlador.teclado)
    {
        _sender = sender;
        _key = EventKey.novoHeroiSpawnado;
        _nomeDoDono = nomeDoDono;
        _controlador = controlador;
    }
}
