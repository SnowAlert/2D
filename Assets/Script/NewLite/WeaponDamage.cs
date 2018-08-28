using UnityEngine;
using UnityEngine.Networking;

public class WeaponDamage : NetworkBehaviour
{
    public int Damage = 1;
    public float attackTime = 1.0f;    // Время пока оружие наносит урон при контакте
    public AnimationClip[] anim;
    [SyncVar(hook = "SwordAnimation")]
    public int BuferAnim = 0;
    [SyncVar(hook = "SwordAnimation")]
    public int Anim = 1;
    [SyncVar]
    private float attackTimeConstant;
    [SyncVar]
    private bool startTime = false;
    [SyncVar]
    private bool nowHit = false;
    [SyncVar]
    private bool attackCompleted = true;
    public GameObject player;

    void OnTriggerStay2D(Collider2D coll)
    {
        print("Name: "+coll.name);
        if (coll.name !=player.name)
        {
            if (coll.gameObject.tag == "Enemy")
                if (startTime == true)
                    if (nowHit == false)
                        if (attackCompleted == false)
                        {
                            GameObject whoHit = coll.gameObject;
                            CmdHit(whoHit, Damage);
                            nowHit = true;
                        }
        }
    }

    [Command]
    void CmdHit(GameObject hit, int _damdge)
    {
        Hit(hit, _damdge);
    }
    void Hit(GameObject hit, int _damdge)
    {
        Health health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(_damdge);
            //print(hit.name + " health: " + health.currentHealth);
        }
    }

    public void SwordAnimation(int NomberAnim)
    {
        //print("SwordAnimation=" + NomberAnim + " Name " + this.name);
        BuferAnim = NomberAnim;
        //print("SwordAnimationBuf=" + BuferAnim + " Name " + this.name);
    }

    void Start()
    //public override void OnStartLocalPlayer()
        {
        player = transform.parent.gameObject;
        player = player.transform.parent.gameObject;
        print("NameStart: "+player.name);
        attackTimeConstant = attackTime;
        Anim = 2;
    }

    void Update()
    {
        if (BuferAnim == 1)
            Anim = 1;
       else if(BuferAnim == 2)
            Anim = 2;

        //if (Input.GetKeyDown(KeyCode.Mouse0))        {
        //if (!player.GetComponent<NetworkIdentity>().isServer)
            if (!isServer)
            {
                //print("Local");
                CmdWeaponAnim();
            }
            else
            {
                //print("!Local");
                ClientWeaponAnim();
            }
       //}
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
    }

    [Command]
    public void CmdWeaponAnim()
    {
        ClientWeaponAnim();
    }

    public void ClientWeaponAnim()
    {
        //print("ClientWeaponAnim=" + BuferAnim+" Name "+this.name);
        if ((Input.GetButton("Fire1")) && (attackTime >= attackTimeConstant))
        {
            if ((BuferAnim == 1) || ((Anim==1)&&(BuferAnim == 4)))
            {
                Anim = 1;
                //print("BuferAnim=1 hit Left");
                GetComponent<Animation>().Play(anim[0].name);
            }
            else if ((BuferAnim == 2) || ((Anim == 2) && (BuferAnim == 4)))
            {
                Anim = 2;
                //print("BuferAnim=2 hit Right");
                GetComponent<Animation>().Play(anim[1].name);
            }
            startTime = true;
            attackCompleted = false;
        }
        else if (BuferAnim == 3 && startTime == false)
        {
            GetComponent<Animation>().Play(anim[2].name);
            //print("BuferAnim=3 Walk");
        }
        else if (BuferAnim == 4 && startTime == false)
        {
            GetComponent<Animation>().Play(anim[3].name);
            //print("BuferAnim=4 Stand");
        }
    }
}