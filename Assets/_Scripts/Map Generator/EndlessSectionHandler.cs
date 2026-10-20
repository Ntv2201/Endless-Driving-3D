using Unity.Burst.Intrinsics;
using UnityEngine;

public class EndlessSectionHandler : MonoBehaviour
{
    Transform playerCarTransform;

    private void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        RenderSectionBasedOnPlayerPosition();
    }

    private void RenderSectionBasedOnPlayerPosition()
    {
        float distanceToPlayer = transform.position.z - playerCarTransform.position.z;

        float lerpPercentage = 1 - ((distanceToPlayer - 100)) / 150f;

        lerpPercentage = Mathf.Clamp01(lerpPercentage);

        transform.position = Vector3.Lerp(
            new Vector3(transform.position.x, -30, transform.position.z),
            new Vector3(transform.position.x, 0, transform.position.z),
            lerpPercentage);

    }



}
