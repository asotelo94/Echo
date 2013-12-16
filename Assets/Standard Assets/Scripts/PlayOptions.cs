using UnityEngine;
using System.Collections;

public class PlayOptions {
	
	private static PlayOptions Instance;
	public static PlayOptions Get()
	{
		if(Instance == null)
			Instance = new PlayOptions();
		return Instance;	
	}
	
	public bool[] unlock;
	public int menuInt =0;
	public Vector3 checkPoint;

	public bool music = true;
}
