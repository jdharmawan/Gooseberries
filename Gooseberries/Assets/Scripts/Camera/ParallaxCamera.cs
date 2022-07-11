using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    //public Camera cam;
    public Cinemachine.CinemachineVirtualCamera cam;
    public Camera tempCam;
    public Transform player;

    Vector2 startPosition;
    float startXPosition;
    float startYPosition;
    float startZPosition;

    Vector2 distBetweenCamAndObject => (Vector2)cam.transform.position - startPosition;
    float distanceFromPlayer => transform.position.z - player.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromPlayer > 0 ? cam.m_Lens.FarClipPlane : cam.m_Lens.NearClipPlane));

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
        startXPosition = transform.position.x;
        startYPosition = distBetweenCamAndObject.y;
        startZPosition = transform.position.z;
    }

    Vector2 ParallaxEffect()
    {
        Debug.Log($"{transform.name} {parallaxFactor}");
        float horizontalParallax = startPosition.x + (distBetweenCamAndObject.x * parallaxFactor);
        float verticalParallax = startPosition.y + (distBetweenCamAndObject.y * parallaxFactor);
		//if (distanceFromPlayer != 0)
		//	verticalParallax = startPosition.y + (distBetweenCamAndObject.y * 1);

		return new Vector2(horizontalParallax, verticalParallax);
    }
    #endregion

}
