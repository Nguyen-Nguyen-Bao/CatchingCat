using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Controller : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject playScreen;
    public GameObject stay1;
    public GameObject stay2;
    public GameObject move;
    bool istantiate_stay;
    Vector3 facing_direction;
    float facing_angle;
    enum Rotate { left, right };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        facing_direction = new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            gameManager.clicking = true;
            if (!istantiate_stay)
            {
                istantiate_stay = true;
                stay1.transform.position = Input.mousePosition;
                stay2.transform.position = Input.mousePosition;
            }
            Vector3 vector = Input.mousePosition - stay2.transform.position;
            float distance = Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2));
            if (distance >= 190)
            {
                stay1.SetActive(false);
                stay2.SetActive(true);
                move.SetActive(false);
            }
            else
            {
                stay1.SetActive(true);
                stay2.SetActive(false);
                move.SetActive(true);
            }
            float rotate_angle = Vector3.Angle(facing_direction, vector);
            gameManager.move_dir = (new Vector3(vector.x, 0, vector.y)).normalized;
            Rotate rotate;
            Vector3 try_rotate_vector = Quaternion.AngleAxis(rotate_angle, new Vector3(0, 0, 1)) * facing_direction;
            if (Vector3.Angle(try_rotate_vector, vector) == 0) rotate = Rotate.right;
            else rotate = Rotate.left;
            if (vector != Vector3.zero && rotate == Rotate.left)
            {
                stay2.transform.eulerAngles = new Vector3(0, 0, stay2.transform.eulerAngles.z - rotate_angle);
                facing_angle -= rotate_angle ;
            }
            else if (vector != Vector3.zero && rotate == Rotate.right)
            {
                stay2.transform.eulerAngles = new Vector3(0, 0, stay2.transform.eulerAngles.z + rotate_angle);
                facing_angle += rotate_angle;
            }
            facing_direction = Quaternion.AngleAxis(facing_angle, new Vector3(0, 0, 1)) * new Vector3(0, 1, 0);
            move.transform.position = Input.mousePosition;
        }
        else if (!Input.GetMouseButton(0))
        { 
            gameManager.clicking = false;
            gameManager.move_dir = Vector3.zero;
            istantiate_stay = false;
            stay1.SetActive(false);
            stay2.SetActive(false);
            move.SetActive(false);
        }
    }
}
