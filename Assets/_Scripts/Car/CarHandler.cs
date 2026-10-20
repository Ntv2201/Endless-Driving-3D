using Unity.VisualScripting;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [Header("Model")]
    [SerializeField] Transform gameModel;

    [Header("Speed Settings")]
    [SerializeField] float accelerationSpeed = 3;
    [SerializeField] float maxAccelerationSpeed = 30;
    [SerializeField] float brakePower = 15;
    [SerializeField] float steerPower = 5;
    [SerializeField] float maxSteerVelocity = 2;
    private Rigidbody rb;
    [SerializeField] private MeshRenderer carMeshRenderer;
    private Vector2 input = Vector2.zero;

    // emissive property 
    int _EmissionColor = Shader.PropertyToID("_EmissionColor");
    Color emissiveColor = Color.white;
    float emissiveColorMultiplier = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();

        input = inputVector;
    }

    private void Update()
    {
        // Rotate car model when turning
        gameModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);

        ControlLightWhenBrake();

    }

    private void FixedUpdate()
    {
        if(input.y > 0)
        {
            Accelerate();
        }
        else
        {
            rb.linearDamping = .2f;
        }
        // apply brake
        if(input.y < 0) Brake();

        Steer();    
        // force the car no to go backward
        if(rb.linearVelocity.z <= 0)
        {
            rb.linearVelocity = Vector3.zero;
        }

    }

    private void Accelerate()
    {
        rb.linearDamping = 0;
        
        if(rb.linearVelocity.z >= maxAccelerationSpeed) return;

        rb.AddForce(rb.transform.forward * input.y * accelerationSpeed);
    }

    private void Brake()
    {
        // dont brake if we're moving forward
        if(rb.linearVelocity.z <= 0) return;

        rb.AddForce(rb.transform.forward * input.y * brakePower);
    }

    private void Steer()
    {
        if(Mathf.Abs(input.x) > 0)
        {
            // move the car side way
            float speedSteerLimit = rb.linearVelocity.z / 5f;
            
            speedSteerLimit = Mathf.Clamp01(speedSteerLimit);

            // Normalize the X velocity
            rb.AddForce(rb.transform.right * input.x * steerPower * speedSteerLimit);

            float normalizedX = rb.linearVelocity.x / maxSteerVelocity;

            // Ensure that we dont allow it to get bigger than 1 in magnitude
            normalizedX = Mathf.Clamp(normalizedX, -1f, 1f);

            // make sure we stay within the turn speed limit
            rb.linearVelocity = new Vector3(normalizedX * maxSteerVelocity, 0, rb.linearVelocity.z);
        }
        else
        {
            // auto center car
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, 0, rb.linearVelocity.z), Time.deltaTime * 3);
        }
    }

    private void ControlLightWhenBrake()
    {
        if(carMeshRenderer != null)
        {
            float targetColorMultiplier = 0f;

            if(input.y < 0)
            {
                targetColorMultiplier = 7f;
            }
            emissiveColorMultiplier = Mathf.Lerp(emissiveColorMultiplier, targetColorMultiplier, Time.deltaTime * 4);
            carMeshRenderer.material.SetColor(_EmissionColor, emissiveColor * emissiveColorMultiplier);
        }
    }

}
