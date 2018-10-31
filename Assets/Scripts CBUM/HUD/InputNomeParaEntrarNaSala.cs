using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNomeParaEntrarNaSala : MonoBehaviour {

    private InputField input;
    
    private void OnEnable()
    {
        input = GetComponent<InputField>();
        input.onEndEdit.RemoveAllListeners();
        input.onEndEdit.AddListener(Sacanagem);
        input.Select();

        /*input.text = System.Guid.NewGuid().ToString();*/
    }

    private void Update()
    {
        /*
        if (input.wasCanceled)
        {
            EventAgregator.Publish(EventKey.cancelInInputRoomName, null);
            //GerenciadorDasOpcoesIniciais.g.RetornarDoListRooms();
        }*/
    }

    public void Sacanagem(string x)
    { 
        
       // if (Input.GetKeyDown(KeyCode.Return))
        {
            if (string.IsNullOrEmpty(input.text.Trim()))
            {
                input.text = "nameless player";
            }

            //GlobalController.g.NomeDeJogador = input.text;

            if (Input.GetKeyDown(KeyCode.Escape))
                EventAgregator.Publish(EventKey.cancelInInputRoomName, null);
            else
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                    print("um enter");

                EventAgregator.Publish(EventKey.submitNameFronRoom, new StandardSendStringEvent(gameObject, input.text, EventKey.submitNameFronRoom));
            }
            //GerenciadorDasOpcoesIniciais.g.L.AposNomeacao(input.text);
        } 
            
    }
}
