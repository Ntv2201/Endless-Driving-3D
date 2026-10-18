using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    private CarHandler carHandler;


    private void Awake()
    {
        carHandler = GetComponent<CarHandler>();
    }

    private void Update()
    {
        HandleMove();
        ReloadScene();

    }

    private void HandleMove()
    {
        Vector2 input = Vector2.zero;
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        carHandler.SetInput(input);
    }

    private void ReloadScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
