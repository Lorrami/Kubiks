using UnityEngine;

public class Destruction : MonoBehaviour
{
    public GameObject Explosion;

    private bool collisionSet = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cubes" && !collisionSet)
        {
            for (int i = collision.transform.childCount - 1; i >= 0; i--)
            {
                var child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100f, Vector3.up, 10f);
                child.SetParent(null);
            }

            Camera.main.gameObject.AddComponent<NewBehaviourScript>();

            Instantiate(Explosion, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z), Quaternion.identity);
            
            Destroy(collision.gameObject);
            collisionSet = true;
        }
    }
}
