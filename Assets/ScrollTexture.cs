using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour {

    public float ScrollX = 0.5f;
    public float ScrollY = 0.5f;

    Renderer rend;

	// Use this for initialization
	void Start () {

        rend = GetComponent<Renderer>();

	}
	
	// Update is called once per frame
	void Update () {

        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * ScrollY;

        rend.material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
	}
}
