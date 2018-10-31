using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerInGame : MonoBehaviour {

    private Controlador controlador = Controlador.joystick1;

    public Controlador MeuControlador
    {
        get { return controlador; }
        set { controlador = value; }
    }
	
	// Update is called once per frame
	void Update () {
        if (CommandReader.ButtonDown(1, (int)controlador)||CommandReader.ButtonDown(6,(int)controlador))
        {
            BtnCancelar();
        }
	}

    public void BtnCancelar()
    {
        EventAgregator.Publish(EventKey.cancelLocalPlayerInGame, new StandardGameEvent(gameObject,EventKey.cancelLocalPlayerInGame));
    }
}
