using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class GetAbsoluteUV : BaseMeshEffect
{
    private Image image;
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;
        if (image == null)
            image = GetComponent<Image>();
        if (image.sprite == null)
            return;
        var outerUv = DataUtility.GetOuterUV(image.sprite);
        UIVertex vertex = new UIVertex();
        for (int i = 0; i < vh.currentVertCount; ++i)
        {
            vh.PopulateUIVertex(ref vertex, i);
            vertex.uv1.x = vertex.uv0.x - outerUv.x > 0 ? 1 : 0;
            vertex.uv1.y = vertex.uv0.y - outerUv.y > 0 ? 1 : 0;
            vh.SetUIVertex(vertex, i);
        }
    }
}
