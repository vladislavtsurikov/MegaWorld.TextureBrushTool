#if UNITY_EDITOR
using UnityEngine;
using VladislavTsurikov.ColliderSystem.Runtime;
using VladislavTsurikov.MegaWorld.Editor.Common.Window;
using VladislavTsurikov.MegaWorld.Editor.Core.Window;
using VladislavTsurikov.MegaWorld.Runtime.Common.Area;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings.FilterSettings.MaskFilterSystem;
using VladislavTsurikov.MegaWorld.Runtime.Common.Stamper;
using VladislavTsurikov.MegaWorld.Runtime.Core;
using VladislavTsurikov.MegaWorld.Runtime.Core.GlobalSettings.ElementsSystem;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Attributes;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.ElementsSystem;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeTerrainTexture;
using VladislavTsurikov.MegaWorld.Runtime.Core.Utility;
using VladislavTsurikov.ReflectionUtility;
using BrushSettings = VladislavTsurikov.MegaWorld.Runtime.Common.Settings.AdvancedBrushSettings.AdvancedBrushSettings;
using ToolsComponentStack = VladislavTsurikov.MegaWorld.Runtime.Core.GlobalSettings.ElementsSystem.ToolsComponentStack;

namespace VladislavTsurikov.MegaWorld.Editor.TextureBrushTool
{
    [Name("Texture Brush")]
    [SupportMultipleSelectedGroups]
    [SupportedPrototypeTypes(new[] { typeof(PrototypeTerrainTexture) })]
    [AddGlobalCommonComponents(new[] { typeof(LayerSettings) })]
    [AddToolComponents(new[] { typeof(TextureBrushToolSettings), typeof(BrushSettings) })]
    [AddGeneralPrototypeComponents(typeof(PrototypeTerrainTexture), new[] { typeof(MaskFilterComponentSettings) })]
    [MegaWorldDocUrl("tools/texture-brush")]
    public class TextureBrushTool : ToolWindow
    {
        private TextureBrushToolSettings _textureBrushToolSettings;
        private BrushSettings _brushSettings;
        private SpacingMouseMove _mouseMove = new();
        private readonly StampTerrainAreaResolver _stampTerrainAreaResolver = new();

        protected override void OnEnable()
        {
            _textureBrushToolSettings =
                (TextureBrushToolSettings)ToolsComponentStack.GetElement(typeof(TextureBrushTool),
                    typeof(TextureBrushToolSettings));
            _brushSettings =
                (BrushSettings)ToolsComponentStack.GetElement(typeof(TextureBrushTool), typeof(BrushSettings));

            _mouseMove = new SpacingMouseMove();
            _mouseMove.OnMouseDown += OnMouseDown;
            _mouseMove.OnMouseDrag += OnMouseDrag;
            _mouseMove.OnRepaint += OnRepaint;
        }

        protected override void DoTool()
        {
            _mouseMove.Spacing = _brushSettings.Spacing;
            _mouseMove.LookAtSize = _brushSettings.BrushSize;
            _mouseMove.Run();
        }

        protected override void HandleKeyboardEvents()
        {
            var brushSettings =
                (BrushSettings)ToolsComponentStack.GetElement(typeof(TextureBrushTool), typeof(BrushSettings));
            brushSettings.ScrollBrushRadiusEvent();
        }

        private void OnMouseDown()
        {
            foreach (Group group in WindowData.Instance.SelectedData.SelectedGroupList)
            {
                BoxArea area = _brushSettings.BrushJitterSettings.GetAreaVariables(
                    _brushSettings, _mouseMove.Raycast.Point, group);

                if (area.RayHit != null)
                {
                    PaintTexture(group, area);
                }
            }
        }

        private void OnMouseDrag(Vector3 dragPoint)
        {
            foreach (Group group in WindowData.Instance.SelectedData.SelectedGroupList)
            {
                RayHit rayHit = RaycastUtility.Raycast(RayUtility.GetRayDown(dragPoint),
                    GlobalCommonComponentSingleton<LayerSettings>.Instance.GetCurrentPaintLayers(group.PrototypeType));

                if (rayHit != null)
                {
                    BoxArea area =
                        _brushSettings.BrushJitterSettings.GetAreaVariables(_brushSettings, rayHit.Point, group);

                    if (area?.RayHit != null)
                    {
                        PaintTexture(group, area);
                    }
                }
            }
        }

        private void OnRepaint()
        {
            BoxArea area = _brushSettings.GetAreaVariables(_mouseMove.Raycast);
            TextureBrushToolVisualisation.Draw(area, WindowData.Instance.SelectionData);
        }

        private void PaintTexture(Group group, BoxArea area)
        {
            _stampTerrainAreaResolver.RefreshCells(area.Bounds);

            foreach (StampTerrainArea stampArea in _stampTerrainAreaResolver.StampAreas)
            {
                BoxArea terrainArea = _stampTerrainAreaResolver.CreateBoxArea(area, stampArea);
                if (terrainArea == null)
                {
                    continue;
                }

                Runtime.Common.Utility.Spawn.TerrainTextureSpawner.SpawnArea(group, group.GetAllSelectedPrototypes(),
                    terrainArea, _textureBrushToolSettings.TextureTargetStrength);
            }
        }
    }
}
#endif
