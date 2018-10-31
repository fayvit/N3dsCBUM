using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmparedarCamera
{
    public static void Emparede(Camera cam)
    {
        //GameObject[] Gs = new GameObject[2];

        ParedeDenovo(cam, new Vector3(0, cam.pixelHeight - 1, 0.1f), Vector3.zero,true);
        ParedeDenovo(cam, new Vector3(cam.pixelWidth - 1, cam.pixelHeight - 1, 0.1f), new Vector3(cam.pixelWidth - 1, 0, 0.1f),false);

        // Gs[0] = Parede(cam, new Vector3(0, cam.pixelHeight - 1, 0.1f), Vector3.zero);
        //Gs[1] = Parede(cam, new Vector3(cam.pixelWidth-1, cam.pixelHeight - 1, 0.1f), new Vector3(cam.pixelWidth - 1,0, 0.1f));

        
    }

    static void ParedeDenovo(Camera cam, Vector3 V1, Vector3 V2,bool pos)
    {
        Ray ray1 = cam.ScreenPointToRay(V1);
        Ray ray2 = cam.ScreenPointToRay(V2);
        Vector3 pos1 = Vector3.zero;
        RaycastHit hit;

        if (Physics.Raycast(ray1, out hit, 10000, 1024))
        {
            pos1 = hit.point;
        }

        if (Physics.Raycast(ray2, out hit, 10000, 1024))
        {
            if (pos1!=Vector3.zero)
            {
                Vector3 dir = (pos1 - hit.point);

                GameObject G = GameObject.CreatePrimitive(PrimitiveType.Quad);

                G.transform.position = hit.point + 0.5f * dir+(pos?1:-1)*2*Vector3.right;
                G.transform.localScale = new Vector3(1000, 100, 1000);
                G.layer = 10;

                G.transform.rotation = Quaternion.LookRotation((pos?1:-1)*Vector3.Cross(dir,Vector3.up));

                G.AddComponent<PosicaoRelativaDacamera>();

                MonoBehaviour.Destroy(G.GetComponent<MeshRenderer>());
            }
        }
    }

    static GameObject Parede(Camera cam,Vector3 V1,Vector3 V2)
    {

        GameObject G = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Ray ray1 = cam.ScreenPointToRay(V1);
        Ray ray2 = cam.ScreenPointToRay(V2);

        Vector3 dir = -(ray1.direction + ray2.direction);

        G.transform.position = ray1.GetPoint(0) + 0.5f * dir;
        G.transform.localScale = new Vector3(1, 100, 100);
//        G.transform.rotation = Quaternion.LookRotation(dir);
        G.transform.rotation = Quaternion.LookRotation(dir, -ray1.GetPoint(10) + ray2.GetPoint(10));

        G.AddComponent<PosicaoRelativaDacamera>();


        //G.transform.position = new Vector3(G.transform.position.x, GlobalController.g.Jogadores[0].Manager.transform.position.y, G.transform.position.z);
        //G.transform.rotation = Quaternion.LookRotation(dir);
        //G.transform.SetParent(cam.transform);
        
        //G.GetComponent<MeshRenderer>().enabled = false;

        return G;
    }
}