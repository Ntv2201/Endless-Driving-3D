using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class RandomizeObject : MonoBehaviour
{
    [Header("Object Settings")]
    [SerializeField] private Vector3 localRotationMin = Vector3.zero;
    [SerializeField] private Vector3 localRotationMax = Vector3.zero;
    [SerializeField] private float localScaleMultiplierMin = .8f;
    [SerializeField] private float localScaleMultiplierMax = 1.5f;

    Vector3 localScaleOriginal = Vector3.one;

    private void Start()
    {
        localScaleOriginal = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localRotation = Quaternion.Euler(
            Random.Range(localRotationMin.x, localRotationMax.x), 
            Random.Range(localRotationMin.y, localRotationMax.y),
            Random.Range(localRotationMin.z, localRotationMax.z)
            );
        
        transform.localScale = localScaleOriginal * Random.Range(localScaleMultiplierMin, localScaleMultiplierMax);

        
    }

    

}
