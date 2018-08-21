using UnityEngine;
using UnityEngine.Networking;

public class WeaponDamage : NetworkBehaviour
{
    public int Damage = 1;
    public float attackTime = 1.0f;
    // Время пока оружие наносит урон при контакте
    public AnimationClip[] anim;
    //public int NumberAnim = 1;
    [SyncVar(hook = "SwordAnimation")]
    public int BuferAnim = 0;
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
        if (coll.name != "Player(Clone)")
        {
            if (coll.gameObject.tag == "Player")
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
        print("SwordAnimation=" + NomberAnim + " Name " + this.name);
        BuferAnim = NomberAnim;
        print("SwordAnimationBuf=" + BuferAnim + " Name " + this.name);
    }

    void Start()
    {
        player = transform.parent.gameObject;
        player = player.transform.parent.gameObject;
        attackTimeConstant = attackTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!player.GetComponent<NetworkIdentity>().isServer)
            {
                print("Local");
                CmdWeaponAnim();
            }
            else
            {
                print("!Local");
                ClientWeaponAnim();
            }
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
    }

    [Command]
    public void CmdWeaponAnim()
    {
        ClientWeaponAnim();
    }

    public void ClientWeaponAnim()
    {
        print("ClientWeaponAnim=" + BuferAnim+" Name "+this.name);
        if ((Input.GetButtonDown("Fire1")) && (attackTime >= attackTimeConstant))
        {
            if (BuferAnim == 1)
            {
                print("BuferAnim=1 hit Left");
                GetComponent<Animation>().Play(anim[0].name);
            }
            else if (BuferAnim == 2)
            {
                print("BuferAnim=2 hit Right");
                GetComponent<Animation>().Play(anim[1].name);
            }
            startTime = true;
            attackCompleted = false;
        }
        else if (BuferAnim == 3 && startTime == false)
        {
            GetComponent<Animation>().Play(anim[2].name);
            print("BuferAnim=3 Walk");
        }
        else if (BuferAnim == 4 && startTime == false)
        {
            GetComponent<Animation>().Play(anim[3].name);
            print("BuferAnim=4 Stand");
        }
    }
}
