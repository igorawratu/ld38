using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenDone : MonoBehaviour {
    private AudioSource source_;
	// Use this for initialization
	void Start () {
        source_ = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(source_ == null || !source_.isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
