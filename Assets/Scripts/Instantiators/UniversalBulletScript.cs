using UnityEngine;

public class UniversalBulletScript : MonoBehaviour
{
    public int Damage;
    public string Debuff;
    private int collisions = 7;

    void Start () {
        if (gameObject.tag == "BasicBullet") {
            Damage = 100;
        } else {
            Debug.Log("Error: Instantiated bullet had no type: "+gameObject.tag);
            Damage = 1;
            Debuff = "none";
        }
    }
    private void OnCollisionEnter2D (Collision2D collision) {
        //Debug.Log("in OnCollisionEnter2D, tag = "+collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Walls")) { //If you are NOT colliding with the player or a segment
            if (collisions <= 0) {
                //Debug.Log("destroying bullet "+gameObject.name);
                Destroy(gameObject);
            } else {
                collisions--;
                /*Vector2 normal = collision.GetContact(0).normal;
                Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
                rigidbody.velocity = Vector2.Reflect(rigidbody.velocity, normal);*/
            }
        } else {
            Debug.Log("Bullet ignored collision with: " + collision.gameObject.name);
        }
    }
}


