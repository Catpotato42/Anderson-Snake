using UnityEngine;

public class UniversalBulletScript : MonoBehaviour
{
    private int maxRenderTime = 125;
    public int Damage;
    public string Debuff;
    private int collisions = 1;

    void Start () {
        if (gameObject.tag == "BasicBullet") {
            Damage = 100;
        } else {
            Debug.Log("Error: Instantiated bullet had no type: "+gameObject.tag);
            Damage = 1;
            Debuff = "none";
        }
    }
    void FixedUpdate () {
        maxRenderTime --; //~every .02 seconds real time, so each 50 render time is around a second.
        if (maxRenderTime <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        //Debug.Log("in OnCollisionEnter2D, tag = "+collision.gameObject.tag);
        if (!collision.gameObject.CompareTag("Obstacle")) { //If you are NOT colliding with the player or a segment
            if (collisions <= 0) {
                //Debug.Log("destroying bullet "+gameObject.name);
                Destroy(gameObject);
            } else {
                collisions--;
                Vector2 normal = collision.GetContact(0).normal;
                Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
                rigidbody.velocity = Vector2.Reflect(rigidbody.velocity, normal);
            }
        } else {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
            //Debug.Log("Bullet ignored collision with: " + collision.gameObject.name);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("In collision (bullet)");
        if (collider.CompareTag("Walls")) {
            if (collisions <= 0) {
                Debug.Log("destroy");
                Destroy(gameObject);
            } else {
                Debug.Log("about to reflect, collisions = "+collisions);
                collisions--;
                Vector2 normal = (collider.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
                GetComponent<Rigidbody2D>().velocity = Vector2.Reflect(GetComponent<Rigidbody2D>().velocity, normal);
                GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + GetComponent<Rigidbody2D>().velocity * Time.fixedDeltaTime);
            }
        }
    }*/
}


