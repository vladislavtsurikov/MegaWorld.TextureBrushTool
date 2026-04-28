#if UNITY_EDITOR
using VladislavTsurikov.MegaWorld.Editor.Common.Settings.FilterSettings.MaskFilterSystem;
using VladislavTsurikov.MegaWorld.Runtime.Common.Area;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings.FilterSettings.MaskFilterSystem.Utility;
using VladislavTsurikov.MegaWorld.Runtime.Core.GlobalSettings.ElementsSystem;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas;

namespace VladislavTsurikov.MegaWorld.Editor.TextureBrushTool
{
    public static class TextureBrushToolVisualisation
    {
        private static readonly MaskFilterVisualisation _maskFilterVisualisation = new();

        public static void Draw(BoxArea area, SelectionData data)
        {
            if (area == null || area.RayHit == null)
                return;

            if (data.SelectedData.HasOneSelectedPrototype())
            {
                _maskFilterVisualisation.DrawMaskFilterVisualization(
                    MaskFilterUtility.GetMaskFilterFromSelectedPrototype(data), area);
            }
            else
            {
                DrawShaderVisualisationUtility.DrawAreaPreview(area);
            }
        }
    }
}
#endif
