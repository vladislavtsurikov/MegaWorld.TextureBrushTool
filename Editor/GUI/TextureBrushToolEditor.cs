#if UNITY_EDITOR
using VladislavTsurikov.Core.Editor;
using VladislavTsurikov.MegaWorld.Editor.Common;
using VladislavTsurikov.MegaWorld.Editor.Core.Window;

namespace VladislavTsurikov.MegaWorld.Editor.TextureBrushTool
{
    [ElementEditor(typeof(TextureBrushTool))]
    public class TextureBrushToolEditor : ToolWindowEditor
    {
        public override void DrawButtons() => UndoEditor.DrawButtons(TargetType, WindowData.Instance.SelectedData);
    }
}
#endif
