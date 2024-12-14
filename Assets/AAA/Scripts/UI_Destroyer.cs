using UnityEngine;

public class UI_Destroyer : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.clicking) Destroy(gameObject);
    }
}
