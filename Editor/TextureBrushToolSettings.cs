#if UNITY_EDITOR
using System;
using VladislavTsurikov.Nody.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.MegaWorld.Editor.TextureBrushTool
{
    [Serializable]
    [Name("Texture Brush Tool Settings")]
    public class TextureBrushToolSettings : Node
    {
        public float TextureTargetStrength = 1.0f;
    }
}
#endif
