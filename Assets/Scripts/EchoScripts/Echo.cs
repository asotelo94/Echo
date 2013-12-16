using UnityEngine;
using System.Collections;

public class Echo : MonoBehaviour {
	
	private float radio = 0.1f;
	private GameObject[] bgObstacles;
	
	public float radioMax;
	public float growSpeed;

	void Start () {
		bgObstacles = GameObject.FindGameObjectsWithTag("echoView");
	}

	void OnEnabled () {
		//bgObstacles = GameObject.FindGameObjectsWithTag("echoView");
		radio += 0.05f;
	}

	void FixedUpdate () {
	
		transform.localScale = new Vector3(1,1,1)*radio*2f;
		radio += 0.05f*growSpeed;
		
		
		if(gameObject.name == "Echo0")
		{
			foreach(GameObject r in bgObstacles)
				r.renderer.material.SetFloat("_Radius",radio);
		}
		
		if(radio >= radioMax)
		{
			if(gameObject.name == "Echo2")
			{
				foreach(GameObject r in bgObstacles)
					r.GetComponent<EchoFader>().SetFadeOut();
			}
			Destroy(gameObject);
		}

	}
}
