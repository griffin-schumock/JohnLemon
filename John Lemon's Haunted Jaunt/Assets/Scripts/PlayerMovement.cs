using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;

    //Create a animator and rigidbody class.
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    
    //The movement vector
    Vector3 m_Movement;

    //The rotation quaternion. it's like a vector, but more calculus, or something
    Quaternion m_Rotation = Quaternion.identity;


    // Start is called before the first frame update
    void Start()
    {
        //Adds the animator and rigidbody components to the script.
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // FixedUpdate is called once per frame
    // Needs to be fixedUpdate instead of update because movement is all about physics, not rendering.
    void FixedUpdate()
    {
        //Grab movement axes.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Set the x and y values in the vector. z axis remains 0
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        //Set the walking animation based on if the object is moving or not.
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        //A vector for rotation.
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        //This line simply calls the LookRotation method and creates a rotation looking in the direction of the given parameter. 
        m_Rotation = Quaternion.LookRotation(desiredForward);

    }

    void OnAnimatorMove()
    {
        //moves the rigidbody
        //parameters: moveposition(where the body's at + where it's going to be * how fast it's gonna get there)
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);

        //rotates the rigidbody
        //applies the m_rotation quaternion to the rigidbody.
        m_Rigidbody.MoveRotation(m_Rotation);


    }
}
