using UnityEngine;

public class UniversalBulletScript : MonoBehaviour
{
    public int Damage;
    public string Debuff;
    //private int collisions = 3;
    private float timer = 2.5f;

    void Start () {
        if (gameObject.tag == "BasicBullet") {
            Damage = 100;
        } else {
            Debug.Log("Error: Instantiated bullet had no type: "+gameObject.tag);
            Damage = 1;
            Debuff = "none"; //could imagine maybe a bullet that speeds you up or something. Interesting thoughts, but I think beyond my scope
            //even if it would be incredibly easy to do that (it would be). This isn't really a game with room for that.
        }
    }

    void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            Destroy(gameObject);
        }
    }
    /*
    private void OnCollisionEnter2D (Collision2D collision) {
        //Debug.Log("in OnCollisionEnter2D, tag = "+collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Walls") || collision.gameObject.CompareTag("Laser")) { //If you are NOT colliding with the player's head
            if (collisions <= 0) {
                //Debug.Log("destroying bullet "+gameObject.name);
                Destroy(gameObject);
            } else {
                collisions--;
                //Vector2 normal = collision.GetContact(0).normal;
                //Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
                //rigidbody.velocity = Vector2.Reflect(rigidbody.velocity, normal);
            }
        } else {
            Debug.Log("Bullet ignored collision with: " + collision.gameObject.name);
        }
    }
    */
}


