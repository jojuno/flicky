using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.tutorial
{
    public class PlayerMovementControllerCustom : NetworkBehaviour
    {
        public CharacterController controller;
        public Transform cam;
        public float speed = 1f;

        void Start()
        {
            controller.detectCollisions = false;
        }

        public override void OnStartAuthority()
        {
            enabled = true;

            //Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
            //Controls.Player.Move.canceled += ctx => ResetMovement();
        }

        // Update is called once per frame
        void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical"); // if another controller is connected, it will not be 0
            Vector3 direction = new Vector3(x, 0f, z).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir * speed * Time.deltaTime);
            }
        }
    }
}