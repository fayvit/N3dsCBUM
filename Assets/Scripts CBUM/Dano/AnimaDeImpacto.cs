using UnityEngine;
using System.Collections;

public class AnimaDeImpacto : MonoBehaviour {

    private  float tempoDecorrido = 0;
	void Start()
	{
		Destroy(gameObject,0.5f);
	}
	
	// Update is called once per frame
	void Update () {
        tempoDecorrido +=Time.deltaTime;
		transform.localScale = Vector3.Lerp(
			transform.localScale,
			0.75f*new Vector3(1,1,1),
			tempoDecorrido/0.6f);

		transform.LookAt(Camera.main.transform);
	}

}
