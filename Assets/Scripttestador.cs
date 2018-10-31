using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripttestador : MonoBehaviour {

   


    /*
    public bool vai = false;
    Scripttestador s;
	// Use this for initialization
	void Start () {
        s = this;
	}

    public void Blocar(float rr)
    {
        Camera cam = Camera.main;
        Vector3 reference = Vector3.zero;

        if (rr == 1)
            reference = new Vector3(cam.pixelWidth-1,0,0.1f);
        if (rr == 2)
            reference = new Vector3(cam.pixelWidth - 1, cam.pixelHeight-1, 0.1f);
        if (rr == 3)
            reference = new Vector3(0, cam.pixelHeight - 1, 0.1f);

        Ray r = cam.ScreenPointToRay(reference);

        RaycastHit hit;

        if (Physics.Raycast(r, out hit))
        {
            GameObject G = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            G.transform.position = hit.point;

            //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Capsule),hit.point,Quaternion.identity);
        }
    }

    void TentativaDeParede()
    {
        Camera cam = Camera.main;
        Ray ray1 = cam.ScreenPointToRay(new Vector3(0, cam.pixelHeight - 1, 0.1f));
        Ray ray2 = cam.ScreenPointToRay(Vector3.zero);

        Vector3 dir =-Vector3.ProjectOnPlane(ray1.GetPoint(0) - ray2.GetPoint(10),Vector3.up);

        GameObject G = GameObject.CreatePrimitive(PrimitiveType.Cube);

        G.transform.position = ray1.GetPoint(0) + 0.5f * dir;
        G.transform.localScale = new Vector3(1, 100, 100);
        G.transform.rotation = Quaternion.LookRotation(dir);
        G.transform.rotation = Quaternion.LookRotation(dir, -ray1.GetPoint(10) + ray2.GetPoint(10));
        G.transform.SetParent(cam.transform);


        ray1 = cam.ScreenPointToRay(new Vector3(cam.pixelWidth-1, cam.pixelHeight - 1, 0.1f));
        ray2 = cam.ScreenPointToRay(new Vector3(cam.pixelWidth - 1, 0, 0.1f));

        dir = -(ray1.direction + ray2.direction);
        G = GameObject.CreatePrimitive(PrimitiveType.Cube);
        G.transform.position = ray1.GetPoint(10);

        G = GameObject.CreatePrimitive(PrimitiveType.Cube);
        G.transform.position = ray2.GetPoint(10);

        G = GameObject.CreatePrimitive(PrimitiveType.Cube);

        G.transform.position = ray1.GetPoint(0) + 0.5f * dir;
        G.transform.localScale = new Vector3(1, 100, 100);
        G.transform.rotation = Quaternion.LookRotation(dir, -ray1.GetPoint(10) + ray2.GetPoint(10));
        G.transform.SetParent(cam.transform);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //TentativaDeParede();
            EmparedarCamera.Emparede(Camera.main);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Blocar(0);
            Blocar(2);
            Blocar(1);
            Blocar(3);
        }
    }*/
}
