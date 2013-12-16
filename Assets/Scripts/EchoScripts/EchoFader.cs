using UnityEngine;
using System.Collections;

public class EchoFader : MonoBehaviour {
	
	private bool fadeOut = false;
	private Color iniColor;
	private Color endColor;

	void Start()
	{
		iniColor = renderer.material.color;
		endColor = new Color(iniColor.r, iniColor.g,iniColor.b,-0.1f);
	}
	void Update () {

		if(fadeOut && renderer.material.color.a <= 1f)
		{
			//renderer.material.color = new Color(1f,1f,1f,Mathf.Lerp(renderer.material.color.a,-0.5f,Time.deltaTime*2));
			renderer.material.color = Color.Lerp(renderer.material.color,endColor,Time.deltaTime*1.5f);
			if(renderer.material.color.a <= 0f)
			{
				playerControl.isEcho = false;
				fadeOut=false;
				//renderer.material.color = new Color(1f,1f,1f,0f);
				renderer.material.SetFloat("_Radius",0.0f);
				renderer.material.color = iniColor;
			}
		}
	}

	public void SetFadeOut()
	{ 
		if(!fadeOut)
			fadeOut = true;
	}
}
