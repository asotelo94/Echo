using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {
	
	public GameObject echo;
	public float echoWait = 0.8f;
	public float JumpForce = 17f;
	
	public float playerSpeed;
	public float maxSpeed;
	public float echoSpeed;
	public float cameraSpeed = 2f;
	
	private bool isJumping = false;
	private Transform groundCheck;

	public static bool isEcho =false;
	
	private GameObject[] bgObstacles;
	private Animator[] mobileAnimators;
	private bool isGrounded = false; //is the player on the ground?
	private float jumpLapse ;
	
	//CAMERA
	private Vector3 newCameraPos;
	private Vector3 iniCameraPos;
	private float lastGround=0;
	private float h=0f;
	private float t=0f;

	private Animator anim;
	private bool camMove=false;
	private bool freeze=false;

	void Start () {
		Camera.main.transform.parent = null;

		anim = this.GetComponent<Animator>();
		iniCameraPos = Camera.main.transform.position;
		newCameraPos = Camera.main.transform.position;
		//spawnPos = transform.position;

		groundCheck = transform.Find("groundCheck");
		bgObstacles = GameObject.FindGameObjectsWithTag("echoView");
		GameObject[] mobiles = GameObject.FindGameObjectsWithTag("Platform");
		mobileAnimators = new Animator[mobiles.Length];
		for(int i = 0;i< mobiles.Length;i++)
			mobileAnimators[i] = mobiles[i].transform.parent.GetComponent<Animator>();

		foreach(GameObject r in bgObstacles)
			r.renderer.material.SetVector("_Epicenter",new Vector4(transform.position.x,transform.position.y,transform.position.z,r.transform.position.z));

	}
	
	void Update () {
	
		//print (isGrounded);
		if(!freeze)
		{
			h = Input.GetAxis("Horizontal");

			isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
			if(!isGrounded)
				transform.parent = null;
			if(Input.GetButtonDown("Vertical") && isGrounded)isJumping = true;
		}
		else
			h=0;

		if(Input.GetButtonDown("Echo") && !isJumping)
			MakeEcho();

		if(camMove)
		{
			t += Time.deltaTime*cameraSpeed;
			Camera.main.transform.position = Vector3.Lerp(iniCameraPos,newCameraPos,t);
			if(t >= 0.9f)
			{
				camMove=false;
				freeze = false;
				foreach(Animator platform in mobileAnimators)
				{
					platform.speed = 1f;
				}
				rigidbody2D.isKinematic=false;
				iniCameraPos = newCameraPos;
				cameraSpeed = 1f;
			}
		}
	}
	void FixedUpdate()
	{

		anim.SetFloat("Speed",Mathf.Abs(h));

		if(h > 0f)
			transform.eulerAngles = new Vector3(0f,90f,0f);
		if(h < 0f)
			transform.eulerAngles = new Vector3(0f,-90f,0f);

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player.
			rigidbody2D.AddForce(Vector2.right * h * playerSpeed);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

		if(transform.position.x <  Camera.main.transform.position.x- (26/2))
			moveCamera(-1f);
		
		if(transform.position.x > Camera.main.transform.position.x+(26/2))
			moveCamera(1f);

		if(transform.position.y < -20)
			Respawn();
	    
	    if(isJumping)Jump();	

	}
	private float height;
	private bool ft=true;

	void OnCollisionEnter2D(Collision2D col)
	{	
		if(Mathf.Abs(col.contacts[0].normal.x) > 0.9)//0.5 para evitar subir inclinacion
		{	
			print ("against wall");
			rigidbody2D.velocity = new Vector3(col.contacts[0].normal.x*5,-9.81f,0);
		}
		if(col.gameObject.layer == 8)//ground
		{
			if(ft)
			{
				height = 0;
				ft =false;
			}
			else
				height += transform.position.y - lastGround;
			//print("new Height = "+height);

			if(col.contacts[0].normal.y > 0.5 )
			{
				if(col.transform.tag == "Platform")
					transform.parent = col.transform;

				if(Mathf.Abs(height) > 5f)
				{
					camMove=true;
					t=0;
					iniCameraPos.y = Camera.main.transform.position.y;
					newCameraPos.y = transform.position.y +7.5f;
					cameraSpeed = 1.5f;
					height=0f;
					isJumping=false;
				}
			}
			lastGround = transform.position.y;
		}
		
	}
	
	void OnCollisionStay2D(Collision2D col)
	{
		//print("n.x = "+col.contacts[0].normal.x);
		/*if(Mathf.Abs(col.contacts[0].normal.x) > 0.9)//0.5 para evitar subir inclinacion
		{	
			print ("against wall");
			rigidbody2D.velocity = new Vector3(col.contacts[0].normal.x*3,-15,0);
		}
		else
			if(Mathf.Abs(col.contacts[0].normal.x) > 0.55)
			{	
				print ("sliding");
				rigidbody.velocity = new Vector3(0,-50,0);
			}*/
	}

	void OnTriggerEnter2D(Collider2D hit)
	{
		if(hit.collider2D.tag == "EditorOnly")
		{
			hit.SendMessage("SetText");
		}
		if(hit.collider2D.tag == "Enemy")
		{
			print("dead");
			Application.LoadLevel("play");
			//rigidbody.MovePosition(spawnPos);
		}
		if(hit.collider2D.tag == "Respawn")
		{
			PlayOptions.Get().checkPoint = hit.transform.position;
		}
	}
	 
	void moveCamera(float dir)
	{
		camMove=true;
		freeze=true;
		rigidbody2D.isKinematic = true;
		t=0f;
		foreach(Animator platform in mobileAnimators)
		{
			platform.speed = 0f;
		}
		iniCameraPos.x = Camera.main.transform.position.x;
		newCameraPos.x = Camera.main.transform.position.x + (26f*dir);
	}

	void Jump() 
	{
		//print ("is jumping");
		isJumping = false;
		rigidbody2D.AddForce(new Vector2(0f, JumpForce));
	}
	void Respawn()
	{
		transform.position = PlayOptions.Get().checkPoint;
	}

	void MakeEcho()
	{
		if(!GameObject.Find("Echo2") && !isEcho)
		{
			foreach(GameObject r in bgObstacles)
				r.renderer.material.SetVector("_Epicenter",new Vector4(transform.position.x,transform.position.y,transform.position.z,r.transform.position.z));
			
			isEcho = true;
			for(int i = 0; i < 3; i++)
			{
				GameObject aux = (GameObject) Instantiate(echo,transform.position,Quaternion.identity);
				aux.name = "Echo"+i;
			}
			//isEcho = false;
		}
	}
}

