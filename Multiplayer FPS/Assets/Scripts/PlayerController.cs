﻿using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 5000f;

    [Header("Spring settings:")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    // Component caching
    private Animator animator;
    private PlayerMotor motor;
    private ConfigurableJoint joint;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);
    }

    private void Update()
    {
        // Calculate movement as a 3d vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");
 

        Vector3 _moveHorizontal = transform.right * _xMov;
        Vector3 _moveVertical = transform.forward * _zMov;


        // Final movement vector
        Vector3 _velocity = (_moveHorizontal + _moveVertical) * speed;

        // Animate movement
        animator.SetFloat("ForwardVelocity", _zMov);

        // Apply Movement
        motor.Move(_velocity);

        // Calculate rotation as 3d vector
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        // Apply rotation
        motor.Rotate(_rotation);

        // Calculate camera rotation as 3d vector
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        // Apply camera rotation
        motor.RotateCamera(_cameraRotationX);


        Vector3 _thrusterForce = Vector3.zero;
        // Calculate thruster force based on input
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        } else
        {
            SetJointSettings(jointSpring);
        }

        // Apply thruster force
        motor.ApplyThruster(_thrusterForce);

        // Show player list
        //if (Input.GetKey(KeyCode.Tab))
        //{
        //    GameManager.PlayerList();
        //}

    }


    // Apply joint settings
    private void SetJointSettings (float _jointSpring)
    {
        joint.yDrive = new JointDrive {
            positionSpring = jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
