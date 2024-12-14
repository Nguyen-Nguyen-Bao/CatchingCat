using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayCotroller : MonoBehaviour
{
    public GameManager gameManager;
    public float speed;
    public float gravityforce;
    public Transform point1;
    public Transform point2;
    public Transform boxPoint;
    public float boxWidth;
    public float boxLength;
    public LayerMask layerMask_Cat;
    Rigidbody rb;
    Vector3 facing_direction;
    float facing_angle;
    float y_speed = 0;
    enum Rotate { left, right };
    Vector3 lastframe_angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ControL();
        RotatE();
        DroP();
        CatchCaT();
    }
    void ControL()
    {
        y_speed = rb.linearVelocity.y;
        rb.linearVelocity = gameManager.move_dir * speed;
        transform.eulerAngles = lastframe_angle;
        Debug.DrawRay(transform.position, point1.position - transform.position, Color.green);
        Debug.DrawRay(transform.position, point2.position - transform.position, Color.green);
        Debug.DrawRay(point2.position, point1.position - point2.position, Color.green);

    }
    void RotatE()
    {
        float rotate_angle = Vector3.Angle(facing_direction, rb.linearVelocity);
        Rotate rotate;
        Vector3 try_rotate_vector = Quaternion.AngleAxis(rotate_angle, new Vector3(0, 1, 0)) * facing_direction;
        if (Vector3.Angle(try_rotate_vector, rb.linearVelocity) == 0) rotate = Rotate.right;
        else rotate = Rotate.left;
        if (rb.linearVelocity != Vector3.zero && rotate == Rotate.left)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - (rotate_angle < 6 ? rotate_angle : 6), 0);
            facing_angle -= rotate_angle < 6 ? rotate_angle : 6;
        }
        else if (rb.linearVelocity != Vector3.zero && rotate == Rotate.right)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + (rotate_angle < 6 ? rotate_angle : 6), 0);
            facing_angle += rotate_angle < 6 ? rotate_angle : 6;
        }
        facing_direction = Quaternion.AngleAxis(facing_angle, new Vector3(0, 1, 0)) * new Vector3(0, 0, 1);
        lastframe_angle = transform.eulerAngles;
    }
    void DroP()
    {
        if(!gameManager.grounded && rb.linearVelocity.y > -gravityforce)
        {
            y_speed -= gravityforce * Time.deltaTime;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, y_speed, rb.linearVelocity.z);
        }
    }
    void CatchCaT()
    {
        Collider[] colliders = Physics.OverlapBox(boxPoint.position, new Vector3(boxLength, 0.4f, boxWidth), Quaternion.identity, layerMask_Cat);
        foreach (Collider collider in colliders)
        {
            Vector2 cat = new Vector2(collider.transform.position.x, collider.transform.position.z);
            Vector2 player = new Vector2(transform.position.x, transform.position.z);
            Vector2 pointA = new Vector2(point1.position.x, point1.position.z);
            Vector2 pointB = new Vector2(point2.position.x, point2.position.z);
            Vector2 fromPlayer = cat - new Vector2(transform.position.x,transform.position.z);
            bool compare1 = Mathf.Sqrt(Mathf.Pow(fromPlayer.x, 2) + Mathf.Pow(fromPlayer.y, 2)) <= 2.22f;
            float a = TC(player, pointA, pointB);
            float b = TC(cat, player, pointA) + TC(cat, player, pointB) + TC(cat, pointA, pointB);
            bool compare2 = a == b;
                //Debug.Log(Mathf.Abs(player.x * (pointA.y - pointB.y) + pointA.x * (pointB.y - player.y) + pointB.x * (player.y - pointA.y))/2);
                Debug.Log(a +" " + b+" "+compare2);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            gameManager.grounded = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log(gameManager.grounded);
        if (other.gameObject.layer == 3)
        {
            gameManager.grounded = false;
        }
        Debug.Log(gameManager.grounded);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(boxPoint.position, new Vector3(boxLength, 0.4f, boxWidth));
    }
    float TC(Vector2 a, Vector2 b, Vector2 c)
    {
        float s = (a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) / 2;
        if (s < 0) s *= -1;
        return s;
    }
}
