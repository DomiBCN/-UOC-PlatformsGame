using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    [SerializeField]
    Transform[] backgrounds;
    [SerializeField]
    float smoothing = 1f;

    float[] parallaxScales;
    Transform camPos;
    Vector3 previousCamPosition;

    private void Awake()
    {
        camPos = Camera.main.transform;
    }

    // Use this for initialization
    void Start () {
        previousCamPosition = camPos.position;

        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < parallaxScales.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPosition.x - camPos.position.x) * parallaxScales[i];

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            Vector3 backgroundsTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundsTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPosition = camPos.position;
    }
}
