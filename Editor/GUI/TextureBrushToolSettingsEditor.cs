#if UNITY_EDITOR
using UnityEngine;
using VladislavTsurikov.Core.Editor;
using VladislavTsurikov.IMGUIUtility.Editor;
using VladislavTsurikov.IMGUIUtility.Editor.ElementStack;
using VladislavTsurikov.IMGUIUtility.Editor.ElementStack.ReorderableList;

namespace VladislavTsurikov.MegaWorld.Editor.TextureBrushTool
{
    [DontDrawFoldout]
    [ElementEditor(typeof(TextureBrushToolSettings))]
    public class TextureBrushToolSettingsEditor : IMGUIElementEditor
    {
        private TextureBrushToolSettings _settings;

        public override void OnEnable()
        {
            _settings = (TextureBrushToolSettings)Target;
        }

        public override void OnGUI()
        {
            _settings.TextureTargetStrength = CustomEditorGUILayout.Slider(
                new GUIContent("Target Strength"), _settings.TextureTargetStrength, 0, 1);
        }
    }
}
#endif
