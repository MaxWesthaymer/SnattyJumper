using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[AddComponentMenu("UI/Effects/Gradient")] 
public class Gradient : BaseMeshEffect
{
    #region Inspector Fields
    [SerializeField] private EGradientType _gradientType = EGradientType.VERTICAL;

    [SerializeField] [Range(-1.5f, 1.5f)] private float _offset;

    [SerializeField] private Color32 _startColor = Color.white;
    [SerializeField] private Color32 _endColor = Color.black;
    #endregion

    #region Public Methods
    public override void ModifyMesh(VertexHelper helper)
    {
        if (!IsActive() || helper.currentVertCount == 0)
        {
            return;
        }

        var vertexList = new List<UIVertex>();
        helper.GetUIVertexStream(vertexList);

        switch (_gradientType)
        {
                case EGradientType.VERTICAL:
                    SetVertical(vertexList, ref helper);
                    break;
                case EGradientType.HORIZONTAL:
                    SetHorizontal(vertexList, ref helper);
                    break;
        }
    }
    #endregion

    #region Private Methods
    private void SetVertical(IReadOnlyList<UIVertex> vertexList, ref VertexHelper helper)
    {
        var bottom = vertexList[0].position.y;
        var top = vertexList[0].position.y;
        for (var i = vertexList.Count - 1; i >= 1; i--)
        {
            SetBounds(vertexList[i].position.y, ref top, ref bottom);
        }

        var height = 1.0f / (top - bottom);
        var vertex = new UIVertex();
        for (var i = 0; i < helper.currentVertCount; i++)
        {
            helper.PopulateUIVertex(ref vertex, i);
            vertex.color = Color32.Lerp(_endColor, _startColor, (vertex.position.y) * height - _offset);
            helper.SetUIVertex(vertex, i);
        }
    }
    
    private void SetHorizontal(IReadOnlyList<UIVertex> vertexList, ref VertexHelper helper)
    {
        var left = vertexList[0].position.x;
        var right = vertexList[0].position.x;
        for (var i = vertexList.Count - 1; i >= 1; i--)
        {
            SetBounds(vertexList[i].position.x, ref right, ref left);
        }

        var width = 1.0f / (right - left);
        var vertex = new UIVertex();
        for (var i = 0; i < helper.currentVertCount; i++)
        {
            helper.PopulateUIVertex(ref vertex, i);
            vertex.color = Color32.Lerp(_endColor, _startColor, (vertex.position.x) * width - _offset);
            helper.SetUIVertex(vertex, i);
        }
    }

    private void SetBounds(float pos, ref float first, ref float second)
    {
        if (pos > first)
        {
            first = pos;
        }
        else if (pos < second)
        {
            second = pos;
        }
    }
    #endregion
    
    public enum EGradientType
    {
        VERTICAL,
        HORIZONTAL
    }
}