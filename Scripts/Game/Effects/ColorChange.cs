using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class ColorChange : BaseMeshEffect
{
    [SerializeField]
    private Color beginColor = Color.green;
    [SerializeField]
    private Color endColor = Color.red;
    Image mImage;
	protected override void Start()
	{
        base.Start();
        mImage = this.GetComponent<Image>();
	}

	public override void ModifyMesh(VertexHelper vh)
    {
        if(!this.IsActive()){
            return;
        }
        var verts = new List<UIVertex>(vh.currentVertCount);
        vh.GetUIVertexStream(verts);
        ModifyVertices(verts);
        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);

    }
    void ModifyVertices(List<UIVertex> vertices){
        for (int i = 0; i < vertices.Count; i ++){
            var tempVert = vertices[i];
            var tempColor = vertices[i].color;

            tempColor = Color.Lerp(beginColor, endColor, 1 - mImage.fillAmount);
            tempVert.color = tempColor;
            vertices[i] = tempVert;
        }
    }
}
