using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    [SerializeField]
    float speed = 0f;
	// Use this for initialization
	void Start () {
        speed += Random.Range(0.2f, 0.5f);

        float randomAlpha = Random.Range(0f, 0.25f);

        foreach(Transform child in transform)
        {
            Color cloudColor = child.GetComponent<SpriteRenderer>().color;
            cloudColor.a -= randomAlpha;
            child.localScale *= Random.Range(0.8f, 1.5f);
            child.GetComponent<SpriteRenderer>().color = cloudColor;
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.right * speed * Time.deltaTime;
	}
}
