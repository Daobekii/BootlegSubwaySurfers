using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character;
    private float laneDistance = 1.5f;
    public static float jumpHeight = 1.0f;
    private float gravity = -9.81f;
    private Vector3 verticalVelocity = Vector3.zero;
    private float laneSwitchSpeed = 15.0f;
    private float targetXPosition;
    private float runSpeed = 4.0f;
    private float speedIncrease = 0.04f; //war davor auf 0.03f, Laut meiner Schwester soll es etwas schneller sein
    private float maxSpeed = 12.0f; //eventuell ändern idk war davor auf 15
    private float lastZPosition;
    private float timeStationary;

    void Start()
    {
        character = GetComponent<CharacterController>();
        targetXPosition = transform.localPosition.x;
    }

    void Update()
    {
        Run();
        MoveCharacter();
        ApplyGravity();
        Jump();
        DashDown();
        DeathCondition();
    }

    private void MoveCharacter()
    {
        if (Input.GetKeyDown(KeyCode.A) && targetXPosition > -laneDistance)
        {
            targetXPosition -= laneDistance;
        }
        else if (Input.GetKeyDown(KeyCode.D) && targetXPosition < laneDistance)
        {
            targetXPosition += laneDistance;
        }
        float smoothXPosition = Mathf.MoveTowards(transform.localPosition.x, targetXPosition, laneSwitchSpeed * Time.deltaTime);
        Vector3 nextPosition = new Vector3(smoothXPosition, transform.localPosition.y, transform.localPosition.z);

        character.Move(nextPosition - transform.localPosition);
    }

    private void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && character.isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        if (character.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0f;
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
        character.Move(verticalVelocity * Time.deltaTime);
    }

    //For some reason kann man beide Methoden (Jump & ApplyGravity) NICHT kombinieren in meinem Fall, da Unity sonst denkt, dass der character die ganze Zeit NICHT grounded ist.
    //https://docs.unity3d.com/ScriptReference/CharacterController.Move.html -> Genau derselbe Code, aber funktioniert nicht bei mir.

    private void DashDown()
    {
        if (Input.GetKeyDown(KeyCode.S) && !character.isGrounded)
        {
            verticalVelocity.y += 2.0f * gravity;
        }
    }

    private void Run()
    {
        if (runSpeed < maxSpeed)
        {
            runSpeed += speedIncrease * Time.deltaTime;
        }
        Vector3 forwardMovement = Vector3.forward * runSpeed * Time.deltaTime;

        character.Move(forwardMovement);
    }

    private void DeathCondition()
    {
        float currentZPosition = transform.localPosition.z;

        if (Mathf.Approximately(currentZPosition, lastZPosition))
        {
            timeStationary += Time.deltaTime;

            if (timeStationary >= 0.1f) //Ich will dem Spieler eine kleine Chance geben weiterzumachen, wenn er schnell genug reagiert, deswegen nicht auf 0.001f oder so
            {
                PlayerManager.gameOver = true;
                FindFirstObjectByType<AudioManager>().PlaySound("DeathSound"); //WHY NO PLAAY????!!!
            }
        }
        else
        {
            timeStationary = 0f;
        }
        lastZPosition = currentZPosition;
    }

}
