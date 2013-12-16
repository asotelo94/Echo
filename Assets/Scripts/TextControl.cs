using UnityEngine;
using System.Collections;

public class TextControl : MonoBehaviour {
	
	private bool fadeInText=false;

	private string text;
	private TextMesh uit;
	private Color c;

	void Start () {

		uit = gameObject.GetComponentInChildren<TextMesh>();
		text = uit.text;
		c = uit.color;
		c.a =0f;
		uit.color = c;
	}
	
	// Update is called once per frame
	void Update () {

		if(fadeInText)
			uit.color = Color.Lerp(uit.color,Color.white,Time.deltaTime);

	}
	void SetText()
	{
		uit.text = text;
		fadeInText=true;
	}
}
/*
enum event
{
	floor=0,
	echo=1,
}
class message{
	var text:String;
	var trigger:event;
}
var fadeSpeed:float = 1f;
var messages:message[];
private var messageInt:int=-1;

private var fadeIn:boolean;
private var fadeOut:boolean;

function Start () {
guiText.material.color.a =0f;
}

function Update () {

	if(fadeIn && guiText.material.color.a >= 0)
	{
		guiText.material.color.a = Mathf.Lerp(guiText.material.color.a,1.5,Time.deltaTime*fadeSpeed);
		if(guiText.material.color.a >= 1f)
		{
			fadeIn=false;guiText.material.color.a =1f;
			//fadeOut=true;
		}
	}
	
	if(fadeOut && guiText.material.color.a <= 1f)
	{
		guiText.material.color.a = Mathf.Lerp(guiText.material.color.a,-0.5,Time.deltaTime*fadeSpeed);
		if(guiText.material.color.a <= 0f)
			fadeOut=false;
	}
}
function SetMessage(trigger:event)
{
	if(player.lastGround > messageInt && trigger == messages[player.lastGround].trigger)
	{
		messageInt = player.lastGround;
		guiText.text = messages[messageInt].text;
		guiText.material.color.a =0f;
		fadeIn=true;
	}
}
*/