#pragma strict

var myskin:GUISkin;
var score:GUIText;

private var sInt:int;
private var fInt:int;
private var lastmusic:boolean;
private var scoreMSG:String = "";

function Start ()
{
	PlayOptions.Get().unlock[0] = true;
	lastmusic = PlayOptions.Get().music;
}
function OnGUI() 
{
	GUI.skin = myskin;
	
	GUI.color = Color.clear;
	GUI.Window(PlayOptions.Get().menuInt,Rect(0,0,Screen.width,Screen.height),DoMyWindow,"");
}
function DoMyWindow(index:int)
{
	GUI.color = Color.white;
	
	var i:int;
	var j:int;
	var menuNames:String[];
	
	if(index != 1 && score != null)score.text = "";
	
	switch(index)
	{
		case 0://Main
		menuNames = ["Options","Play","Credits"];
		for(i = 0;i<menuNames.length;i++)
		if(GUI.Button(Rect(Screen.width*(i*0.20+0.225),Screen.height*0.7,Screen.width*0.15,Screen.height*0.15),menuNames[i]))
			switch(i)
			{
				case 0:PlayOptions.Get().menuInt = 1;break;
				case 1:PlayOptions.Get().menuInt = 2;break;
				case 2:PlayOptions.Get().menuInt = 3;break;
			}
		break;
		
		case 2://Choose Level
		
		Application.LoadLevel("play");
		break;
		
		case 1://Options
			myskin.label.alignment = TextAnchor.MiddleCenter;
			var colorNames:String[] = ["red","yellow","brown"];
			var sensNames:String[] = ["5","10","15"];
			
			GUI.Box(Rect(Screen.width/3,Screen.height*0.35,Screen.width/3,Screen.height*0.45),"");
			
			lastmusic = GUI.Toggle(Rect(Screen.width*0.45,Screen.height*0.725,Screen.width*0.1,Screen.height*0.05),lastmusic,"Music");
			if(PlayOptions.Get().music != lastmusic)
			{
				var music :AudioSource = GameObject.FindGameObjectWithTag("Music").audio;
				if(lastmusic)music.Play();
				else music.Stop();
				
				PlayOptions.Get().music = lastmusic;
			}
			
			if(GUI.Button(Rect(Screen.width/3,Screen.height*0.85,Screen.width/3,Screen.height*0.1),"Menu"))
				PlayOptions.Get().menuInt = 0;
		break;
		case 3://Credits
			
			GUI.Box(Rect(Screen.width/3,Screen.height*0.4,Screen.width/3,Screen.height*0.3),"Made by:\nAlexis Sotelo\n\nMusic by:\nGoukisan");
			if(GUI.Button(Rect(Screen.width/3,Screen.height*0.75,Screen.width/3,Screen.height*0.14),"Menu"))
				PlayOptions.Get().menuInt = 0;
		break;
	}
}