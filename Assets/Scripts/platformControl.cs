using UnityEngine;
using System.Collections;

public class platformControl : MonoBehaviour {

	public bool bucle;
	public bool drawLine;
	public Vector3 movePos;
	public float speed = 1f;

	private Vector3 iniPos;
	private Vector3 targetPos;
	private float t;
	private LineRenderer line;

	// Use this for initialization
	void Start () {
		iniPos = transform.position;
		targetPos = iniPos + movePos;

		if (drawLine) {
			line = this.GetComponent<LineRenderer> ();
			line.SetPosition (0, iniPos);
			line.SetPosition (1, targetPos);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (bucle && t >= 0.99f) {
			t = 0f;
			Vector3 aux = iniPos;
			iniPos = targetPos;
			targetPos = aux;
		}
		t += Time.deltaTime * speed;
		transform.position = Vector3.Lerp (iniPos,targetPos,t);
	}
	
}
