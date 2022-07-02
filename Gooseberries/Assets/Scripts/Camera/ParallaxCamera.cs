using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    public Camera cam;
    public Transform player;

    Vector2 startPosition;
    float startZPosition;

    Vector2 travel => (Vector2)cam.transform.position - startPosition;
    float distanceFromPlayer => transform.position.z - player.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromPlayer > 0 ? cam.farClipPlane : cam.nearClipPlane));

    float parallaxFactor => Mathf.Abs(distanceFromPlayer) / clippingPlane;

    private void Start()
    {
        Initialise_StartingPosition();
    }

    private void Update()
    {
        transform.position = new Vector3(ParallaxEffect().x, ParallaxEffect().y, startZPosition);
    }

    #region Local Functions
    void Initialise_StartingPosition()
    {
        startPosition = transform.position;
        startZPosition = transform.position.z;
    }

    Vector2 ParallaxEffect()
    {
        return startPosition + travel * parallaxFactor;
    }
    #endregion

}
