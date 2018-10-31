﻿using UnityEngine;
using System.Collections.Generic;

public static class CommandReader
{
    private static Dictionary<string, bool> zerados = new Dictionary<string, bool>();
    public static bool VerificaUsoDesseControle(int esse)
    {
        bool retorno = false;
        //Input.ResetInputAxes();
        if (Input.GetAxis("joy " + esse + " horizontal") != 0 ||
            Input.GetAxis("joy " + esse + " vertical") != 0 ||
            Input.GetAxis("joy " + esse + " triggers") != 0 ||
            Input.GetAxis("joy " + esse + " Xcam") != 0 ||
            Input.GetAxis("joy " + esse + " Ycam") != 0 ||
            Input.GetAxis("joy " + esse + " HDpad") != 0 ||
            Input.GetAxis("joy " + esse + " VDpad") != 0)
        {
            retorno = true;
        }
        else
        {
            for (int i = 0; i < 20; i++)
            {
                if (Input.GetKey("joystick " + esse + " button " + i))
                    retorno = true;
            }
        }
        return retorno;
    }

    public static bool SubmitButtonDown()
    {
        bool foi = false;
        for (int i = -1; i < 5; i++)
            if (i != 0)
                foi |= SubmitButtonDown((Controlador)i);

        return foi;
    }

    public static bool SubmitButtonDown(Controlador c)
    {
        if (c == Controlador.teclado)
            return ButtonDown(7, -1) || ButtonDown(0, -1);
        else
            return ButtonDown(0,(int)c);
    }

    public static int InputDoControle()
    {
        int retorno = 0;
        for (int i = 0; i < 4; i++)
        {
            if (VerificaUsoDesseControle(i))
                retorno = i;
        }
        return retorno;
    }

    public static bool PressionadoBotao(int numButton)
    {
        bool foi = false;
        for (int i = -1; i < 5; i++)
            if (i != 0)
                foi |= PressionadoBotao(numButton, i);

        return foi;
    }

    public static bool PressionadoBotao(int numButton, int numControl)
    {
        bool retorno = false;
        if (numControl == -1)
            retorno = Input.GetButton("joystick " + numControl + " button " + numButton);
        else
            retorno = Input.GetKey((KeyCode)(350 + (numControl - 1) * 20 + numButton));
        return retorno;
    }

    public static bool ButtonDown(int numButton)
    {
        bool foi = false;

        for (int i = -1; i < 5; i++)
            if (i != 0)
                foi |= ButtonDown(numButton, i);

        return foi;
    }

    public static bool ButtonDown(int numButton, int numControl)
    {
        bool retorno = false;
        if (numControl == -1)
            retorno = Input.GetButtonDown("joystick " + numControl + " button " + numButton);
        else
            retorno = Input.GetKeyDown((KeyCode)(350 + (numControl - 1) * 20 + numButton));
        return retorno;
    }

    public static float GetAxis(string esseGatilho, int numControlador)
    {
        return Input.GetAxis("joy " + numControlador + " " + esseGatilho);
    }

    public static Vector3 VetorDirecao(int numControlador)
    {
        Vector3 V = Vector3.zero;

        float h = Mathf.Clamp(GetAxis("horizontal",numControlador), -1, 1);
        float v = Mathf.Clamp(GetAxis("vertical",numControlador), -1, 1);

        Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward,Vector3.up);//new Vector3(1, 0, 0);
        /*
        if (AplicadorDeCamera.cam != null)
            if (AplicadorDeCamera.cam.Cdir != null)
                forward = AplicadorDeCamera.cam.Cdir.DirecaoInduzida(h, v);*/

        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        return (h * right + v * forward);

    }

    static void VerificaExisteZerado(string esseGatinlho)
    {
        if (!zerados.ContainsKey(esseGatinlho))
            zerados[esseGatinlho] = true;
    }

    static public int ValorDeGatilhos(string esseGatilho,int numControlador)
    {
        int retorno = 0;

        esseGatilho = "joy " + numControlador + " " + esseGatilho;

        VerificaExisteZerado(esseGatilho);

        float val = Input.GetAxisRaw(esseGatilho);
        if (zerados[esseGatilho])
        {
            if (val != 0)
            {
                zerados[esseGatilho] = false;

            }

            if (val > 0)
                retorno = 1;
            else if (val < 0)
                retorno = -1;

        }
        else
        {

            retorno = 0;
            if (val > -0.1f && val < 0.1f)
                zerados[esseGatilho] = true;

        }


        return retorno;

    }

    static public int ValorDeGatilhosTeclado(string esseGatilho,int numControlador)
    {
        int retorno = 0;
        esseGatilho = "joy " + numControlador + " " + esseGatilho;
        float val = Input.GetAxisRaw(esseGatilho);

        VerificaExisteZerado(esseGatilho);

        if (zerados[esseGatilho])
        {
            if (val != 0)
            {
                zerados[esseGatilho] = false;
            }

            if (val > 0)
                retorno = 1;
            else if (val < 0)
                retorno = -1;

        }
        else
        {

            retorno = 0;
            if (val > -0.01f && val < 0.01f)
                zerados[esseGatilho] = true;

        }

        return retorno;
    }
}
