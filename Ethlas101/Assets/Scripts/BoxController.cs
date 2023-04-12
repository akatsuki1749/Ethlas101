using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    //Bool to detect if current box is in contact with previous box
    public bool IsColliding = false;
    //List to hold materials for switching the box color
    public List<Material> ListOfMaterials = new List<Material>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Adds a force on the current box along the X axis
    public void AddForceX(Rigidbody gameobject, float speed) 
    {
        gameobject.AddForce(speed, 0, 0, ForceMode.Impulse);
    }
    //Adds a force on the current box along the Z axis
    public void AddForceZ(Rigidbody gameobject, float speed)
    {
        gameobject.AddForce(0, 0, speed, ForceMode.Impulse);
    }
    public Material GetColor(int color) 
    {
        return ListOfMaterials[color];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null) 
        {
            IsColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        IsColliding = false;
    }
}
