using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class EnableDepthBuffer : MonoBehaviour {

    public bool enable = true;

    #if UNITY_EDITOR
    void OnDrawGizmos(){
		Set();
	}
    #endif

	void Start () {
		Set();
	}

	void Set(){
		if(enable)
			GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }
}
