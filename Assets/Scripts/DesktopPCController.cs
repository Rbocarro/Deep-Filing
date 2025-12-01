using Evengy.GridBasedMovementController.Player;
using UnityEngine;

using UnityEngine.UI;

public class DesktopPCController : MonoBehaviour,IEntity
{
    public Button ExitDesktopbutton;

    void Start()
    {
        ExitDesktopbutton.onClick.AddListener(ExitPCInteraction);
    }

    public void InteractWith()
    {

        PlayerController.CanPlayerMove = false;
        PlayerController.PlayerCameraTransform.transform.localPosition = new Vector3(0f, -0.07f, 0.28f);
        //player
    }

    public void ExitPCInteraction()
    {
        Debug.Log("Exit PC");
        PlayerController.CanPlayerMove = true;//need to fix input buffering
        PlayerController.ResetPlayerCamTransformToDefault();
    }
}
