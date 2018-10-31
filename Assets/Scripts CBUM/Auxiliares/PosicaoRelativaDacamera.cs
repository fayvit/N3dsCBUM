using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicaoRelativaDacamera : MonoBehaviour {

    Vector3 dirRelativa;
    Vector3 fRelativa;
    Vector3 uRelativa;
    Transform daCamera;

	// Use this for initialization
	void Start () {
        daCamera = Camera.main.transform;
        dirRelativa = transform.position - daCamera.position;
        dirRelativa = new Vector3(
            Vector3.Dot(dirRelativa, daCamera.right),
            Vector3.Dot(dirRelativa, daCamera.up),
            Vector3.Dot(dirRelativa, daCamera.forward)
            );
        fRelativa = new Vector3(
            Vector3.Dot(transform.forward, daCamera.right),
            Vector3.Dot(transform.forward, daCamera.up),
            Vector3.Dot(transform.forward, daCamera.forward)
            );
        uRelativa = new Vector3(
           Vector3.Dot(transform.up, daCamera.right),
           Vector3.Dot(transform.up, daCamera.up),
           Vector3.Dot(transform.up, daCamera.forward)
           );
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.LookRotation(
            fRelativa.x * daCamera.right + fRelativa.y * daCamera.up + fRelativa.z * daCamera.forward,
            uRelativa.x * daCamera.right + uRelativa.y * daCamera.up + uRelativa.z * daCamera.forward
            );
        transform.position = daCamera.position+ dirRelativa.x * daCamera.right + dirRelativa.y * daCamera.up + dirRelativa.z * daCamera.forward;
    }
}
