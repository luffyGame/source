using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarItem : MonoBehaviour {
    public bool debuggg = false;
    public RectTransform rectTrans;
    public Image icon;
    public void SetPos(Vector2 pos){
        rectTrans.anchoredPosition = pos;
    }
    public void SetDir(Vector2 dir, float yRot, RectTransform playerTrans){
        rectTrans.RotateAround(playerTrans.position, Vector3.forward, yRot);
        rectTrans.localEulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, dir) + yRot + 180);

    }
}
