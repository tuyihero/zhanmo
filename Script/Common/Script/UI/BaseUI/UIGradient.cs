using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class UIGradient : BaseMeshEffect
{
    public Color maxColor = Color.white;
    public Color minColor = Color.black;
    public bool isHorizontal;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;
        var count = vh.currentVertCount;
        if (count < 1)
            return;
        List<UIVertex> verticles = new List<UIVertex>();
        for (var i = 0; i < count; i++)
        {
            var vertex = new UIVertex();
            vh.PopulateUIVertex(ref vertex, i);
            verticles.Add(vertex);
        }
        float min = GetAxis(verticles[0]);
        float max = GetAxis(verticles[0]);
        for (int i = 1; i < verticles.Count; i++)
        {
            min = Mathf.Min(min, GetAxis(verticles[i]));
            max = Mathf.Max(max, GetAxis(verticles[i]));
        }
        var heightInvert = 1f / (max - min);
        for (int i = 0; i < verticles.Count; i++)
        {
            var item = verticles[i];
            item.color = Color.Lerp(minColor, maxColor, (GetAxis(item) - min) * heightInvert);
            vh.SetUIVertex(item, i);
        }
    }
    public float GetAxis(UIVertex vertex)
    {
        return isHorizontal ?  vertex.position.x : vertex.position.y;
    }
}