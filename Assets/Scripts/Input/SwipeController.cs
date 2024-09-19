using UnityEngine;

public class SwipeController
{
    private PlayerController playerController;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;


    public void UpdateSwipes()
    {
        var inputData = new InputData();

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            startTouchPosition = Input.GetTouch(0).position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;

            inputData.MoveUp = endTouchPosition.y > (startTouchPosition.y + 300f);

            if (inputData.MoveUp == false)
            {
                inputData.MoveRight = endTouchPosition.x > startTouchPosition.x;
                inputData.MoveLeft = endTouchPosition.x < startTouchPosition.x;
            }
        }

        playerController.ProcessInput(inputData);
    }

    public void SetPlayerController(PlayerController controller)
    {
        playerController = controller;
    }
}

public class InputData
{
    public bool MoveLeft = false;
    public bool MoveRight = false;
    public bool MoveUp = false;
}
