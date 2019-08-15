// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Kit
{ 
    /// <summary>
    /// 文本自动分格
    ///     当前只能从中间往右移动
    /// </summary>
    [AddComponentMenu("UI/Effects/TextSpacing")]
    [RequireComponent(typeof(Text))]
    public class TextSpacing : BaseMeshEffect
    {
        public float _textSpacing = 1f;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive() || vh.currentVertCount == 0)
            {
                return;
            }
            List<UIVertex> vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);
            int indexCount = vh.currentIndexCount;
            UIVertex vt;
            for (int i = 6; i < indexCount; i++)
            {
                //第一个字不用改变位置,且一个字在 List<UIVertex>中占6个位置，第一个字既（0-5）  
                vt = vertexs[i];
                vt.position += new Vector3(_textSpacing * (i / 6), 0, 0);
                vertexs[i] = vt;
                //以下注意点与索引的对应关系,  
                //注：经测试，一个字符的三角面的顶点记录顺序为顺时针从左上角的点开始计数 0 1 2 3 4 5 ，  
                //而第3个顶点和第2个顶点重合，第5个顶点和第0个顶点重合，所以只需要修改4个顶点就能修改该字符的偏移  
                if (i % 6 <= 2)
                {
                    vh.SetUIVertex(vt, (i / 6) * 4 + i % 6);
                }
                if (i % 6 == 4)
                {
                    vh.SetUIVertex(vt, (i / 6) * 4 + i % 6 - 1);
                }
            }
        }
    }
}
