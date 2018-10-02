using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public float speed = 1.0f;
	// Use this for initialization
	void Start () {
        speed += Random.Range(-0.5f, 0.5f);
        transform.localScale *= Random.Range(0.8f, 1.2f);

        float randomAlpha = Random.Range(0.5f, 0.75f);

        foreach(Transform child in transform)
        {
            Color cloudColor = child.GetComponent<SpriteRenderer>().color;
            cloudColor.a -= randomAlpha;
            child.GetComponent<SpriteRenderer>().color = cloudColor;
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.right * speed * Time.deltaTime;
	}
}
