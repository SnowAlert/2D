using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour
{
	public float movementSpeed = 3f;
	public GameObject ShootPrefab;
	public Transform bulletSpawn;
	public float ChangSpeed = 4f;

	private Transform RightHandt;
	private Transform LeftHandt;
	private Transform Addition;
	public GameObject Sword_;
	public GameObject Sword;
    public GameObject Sword2;
    private bool TimeAttak = false;


    [SyncVar]
    public bool startTime = false;
    [SyncVar]
    private bool nowHit = false;
    [SyncVar]
    private bool attackCompleted = true;
    public float attackTime = 1.0f;    // Время пока оружие наносит урон при контакте
    [SyncVar]
    private float attackTimeConstant;

    [SyncVar]
    public string ChangeTriger1 = "1";
    [SyncVar]
    public string ChangeTriger2 = "2";

    public Transform zRotate;	// объект для вращения по оси Z
	void Start(){
        this.name = "Player" +Time.fixedTime;

        Sword = GameObject.Find("Sword(Clone)");    //Надо переделать!!!!!!!!!!!!!!!!!!!!
    }
	
	void Update ()
	{

		if (!isLocalPlayer)
			return;

        

        //ChangeHands
        if (ChangeTriger1 != ChangeTriger2)
            ChangeHands("loh");

        //MOVE
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position = new Vector3(transform.position.x, transform.position.y + movementSpeed * Time.deltaTime);
            SetWaeponAnimation(2);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position = new Vector3(transform.position.x, transform.position.y - movementSpeed * Time.deltaTime);
            SetWaeponAnimation(1);
        }
		if (Input.GetKey (KeyCode.A)) {
			transform.position = transform.position = new Vector3 (transform.position.x - movementSpeed * Time.deltaTime, transform.position.y);
            SetWaeponAnimation(1);

            ChangeTriger1 = "Left";
        }
		if (Input.GetKey (KeyCode.D)) {
			transform.position = transform.position = new Vector3 (transform.position.x + movementSpeed * Time.deltaTime, transform.position.y);
            SetWaeponAnimation(2);
            ChangeTriger1 = "Right";
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
        {
            print("DontMove");
            //если мы стоим то и анимация оружия будет для стояния
            SetWaeponAnimation(4);
        }


        if (Input.GetKeyDown(KeyCode.Mouse1) && (attackTime >= attackTimeConstant))
        {
            startTime = true;
            attackCompleted = false;
        }




        if (startTime == true)
        {
            attackTime -= Time.deltaTime;
        }
        if (attackTime <= 0)
        {
            startTime = false;
            attackTime = attackTimeConstant; //возвращаем переменной задержки её первоначальное значение из константы
            nowHit = false;
        }

        //Берем состояние атаки из меча. Проверяем, что не бъем рукой и нажатие кнопки
        WeaponDamage WeaDam = Sword.GetComponent<WeaponDamage>();
        if (WeaDam != null)
        {
            TimeAttak = WeaDam.startTime;
            print("TimeAttak "+TimeAttak);
            print("startTime " + startTime);
            print("attackCompleted " + attackCompleted);
            print("nowHit " + nowHit);
            if (startTime == false)
                if (nowHit == false)
                    if (attackCompleted == true)
                        if  ((Input.GetKeyDown(KeyCode.Mouse1)) && (TimeAttak == false) )
                    CmdFire();
        }


        if (zRotate)
			SetRotation (); //поворачивает префаб с точкой спауна снарядов
	}
	//выстрел
	[Command]
	void CmdFire ()
	{
		GameObject bullet = Instantiate (ShootPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
		bullet.GetComponent<Rigidbody2D> ().velocity = bullet.transform.up * 10.0f;
		NetworkServer.Spawn (bullet);
	}

	[Command]
	void CmdSword ()
	{
		GameObject Sword = Instantiate (Sword_, RightHandt.position, RightHandt.rotation, this.transform.Find("RightHandt")) as GameObject;
        Sword.transform.localPosition = new Vector3(Sword.transform.localPosition.x - 0.45f, Sword.transform.localPosition.y - 0.25f);
		NetworkServer.Spawn (Sword);
	}

	void SetRotation ()
	{
		Vector3 pos = Input.mousePosition;
		pos.z = transform.position.z - Camera.main.transform.position.z;
		pos = Camera.main.ScreenToWorldPoint (pos);
		Quaternion q = Quaternion.FromToRotation (Vector3.up, pos - transform.position);
		zRotate.rotation = q;
	}

    public void SetWaeponAnimation(int NumberAnimation) {
        WeaponDamage waepons_animation = Sword.GetComponent<WeaponDamage>();
        waepons_animation.SwordAnimation(NumberAnimation);
    }

    //из одной руки в другую меняет оружие и щит
    void ChangeHands(string loh)
    {
        if ((ChangeTriger1 == "Left") & !(ChangeTriger2 == ChangeTriger1))
        {
            //Press A
            // изменяет плавно координаты того что в рукках
            Sword.GetComponent<WeaponDamage>().BuferAnim = 1;
            if (LeftHandt.transform.localPosition.x < 0.0f)
                LeftHandt.transform.localPosition = new Vector3(LeftHandt.transform.localPosition.x + ChangSpeed * Time.deltaTime, LeftHandt.transform.localPosition.y);
            if (Addition.transform.localPosition.x < 0.0f)
                Addition.transform.localPosition = new Vector3(Addition.transform.localPosition.x + ChangSpeed * Time.deltaTime, Addition.transform.localPosition.y);
            if (RightHandt.transform.localPosition.x > 0.0f)
                RightHandt.transform.localPosition = new Vector3(RightHandt.transform.localPosition.x - ChangSpeed * Time.deltaTime, RightHandt.transform.localPosition.y);
            // если после цикла  значение перевалило за указаное условие (немножко левее и тд)
            if (LeftHandt.transform.localPosition.x > 0.0f)
                LeftHandt.transform.localPosition = new Vector3(0.0f, LeftHandt.transform.localPosition.y);
            if (Addition.transform.localPosition.x > 0.0f)
                Addition.transform.localPosition = new Vector3(0.0f, Addition.transform.localPosition.y);
            if (RightHandt.transform.localPosition.x < 0.0f)
                RightHandt.transform.localPosition = new Vector3(0.0f, RightHandt.transform.localPosition.y);
            //когда дойдет до нужного положения
            if ((LeftHandt.transform.localPosition.x == 0.0f) && (Addition.transform.localPosition.x == 0.0f) && (RightHandt.transform.localPosition.x == 0.0f))
                ChangeTriger2 = "Left";
        }
        else if ((ChangeTriger1 == "Right") & !(ChangeTriger2 == ChangeTriger1))
        {
            //Press D
            // изменяет плавно координаты того что в рукках
            Sword.GetComponent<WeaponDamage>().BuferAnim = 2;
            if (LeftHandt.transform.localPosition.x > -0.736f)
                LeftHandt.transform.localPosition = new Vector3(LeftHandt.transform.localPosition.x - ChangSpeed * Time.deltaTime, LeftHandt.transform.localPosition.y);
            if (RightHandt.transform.localPosition.x < 0.9f)
                RightHandt.transform.localPosition = new Vector3(RightHandt.transform.localPosition.x + ChangSpeed * Time.deltaTime, RightHandt.transform.localPosition.y);
            if (Addition.transform.localPosition.x > -0.48f)
                Addition.transform.localPosition = new Vector3(Addition.transform.localPosition.x - ChangSpeed * Time.deltaTime, Addition.transform.localPosition.y);
            // если после цикла  значение перевалило за указаное условие (немножко провее и тд)
            if (LeftHandt.transform.localPosition.x < -0.736f)
                LeftHandt.transform.localPosition = new Vector3(-0.736f, LeftHandt.transform.localPosition.y);
            if (RightHandt.transform.localPosition.x > 0.9f)
                RightHandt.transform.localPosition = new Vector3(0.9f, RightHandt.transform.localPosition.y);
            if (Addition.transform.localPosition.x < -0.48f)
                Addition.transform.localPosition = new Vector3(-0.48f, Addition.transform.localPosition.y);
            //когда дойдет до нужного положения
            if ((LeftHandt.transform.localPosition.x == -0.736f) && (Addition.transform.localPosition.x == -0.48f) && (RightHandt.transform.localPosition.x == 0.9f))
                ChangeTriger2 = "Right";
        }
    }
    //при появление игрока определяет что у него в руках
    public override void OnStartLocalPlayer ()
	{
		RightHandt = transform.Find ("RightHandt");
		LeftHandt = transform.Find ("LeftHandt");
		Addition = transform.Find ("Addition");
		CmdSword();
	}
}