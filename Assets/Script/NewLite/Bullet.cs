using UnityEngine;
using UnityEngine.Networking;
	public class Bullet : NetworkBehaviour {

		[SerializeField] float lifeTimeStart = 2f;
		float age;
		public GameObject explosion;
		public int Damadge = 10;

		void OnCollisionEnter2D(Collision2D coll){
			if (!isServer)
				return;
				GameObject hit = coll.gameObject;
				Health health = hit.GetComponent<Health> ();
				if (health != null) {
				health.TakeDamage (Damadge);
				}
				foreach(ContactPoint2D missileHit in coll.contacts)
				{
					Vector2 hitPoint = missileHit.point;
					Instantiate(explosion,new Vector3(hitPoint.x, hitPoint.y, 0), Quaternion.identity);
				}
				print ("Выстрел попал по: "+coll.gameObject.name);
				Destroy (gameObject);
		}

		[ServerCallback]
		void Update(){
			age += Time.deltaTime;
			if (age > lifeTimeStart) {
				NetworkServer.Destroy (gameObject);
			}
		}
	}