﻿using UnityEngine;
using System.Collections;

public class RetaDelimitadora
{
    public static bool RelativoAReta(float x1, float z1, float x2, float z2, bool maior, float x, float z)
    {
        bool retorno = false;

        if (z < (z2 - z1) / (x2 - x1) * (x - x1) + z1)
        {
            if (maior)
                retorno = false;
            else
                retorno = true;
        }
        else if (z > (z2 - z1) / (x2 - x1) * (x - x1) + z1)
        {
            if (maior)
                retorno = true;
            else
                retorno = false;
        }

        return retorno;
    }

    public static bool RelativoAReta(Vector3 P1, Vector3 P2, bool maior, Vector3 pAlvo)
    {
        return RelativoAReta(P1.x, P1.z, P2.x, P2.z, maior, pAlvo.x, pAlvo.z);
    }
}
