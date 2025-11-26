using UnityEngine;
[RequireComponent (typeof(PlayerControllerTest))]
public class PlayerInput : MonoBehaviour
{
    public KeyCode forward = KeyCode.W;
    public KeyCode back= KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode turnLeft = KeyCode.Q;
    public KeyCode turnRight = KeyCode.E;
    public KeyCode interact = KeyCode.J;


    PlayerControllerTest controller;

    public void Awake()
    {
        controller = GetComponent<PlayerControllerTest>();
    }


    public void Update()
    {
        if (Input.GetKeyUp(forward)) { controller.MoveForward(); }
        if (Input.GetKeyUp(back)) { controller.MoveBack(); }
        if (Input.GetKeyUp(left)) { controller.MoveLeft(); }
        if (Input.GetKeyUp(right)) { controller.MoveRight(); }
        if (Input.GetKeyUp(turnLeft)) { controller.RotateLeft(); }
        if(Input.GetKeyUp(turnRight)) { controller.RotateRight(); }
        if (Input.GetKeyUp(interact)) { controller.Interact(); }

    }
}
