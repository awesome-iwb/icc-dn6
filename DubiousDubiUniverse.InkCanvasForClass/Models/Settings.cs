using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DubiousDubiUniverse.InkCanvasForClass.Models;
using Newtonsoft.Json;

namespace DubiousDubiUniverse.InkCanvasForClass.Models {
    #region 工具栏相关

    public enum ToolbarPlacement {
        LeftBottom,
        RightBottom,
        LeftTop,
        RightTop,
        Bottom,
        Center,
        Top,
        Custom
    }

    public enum ToolbarBorderThickness {
        None = 0,
        Thin = 10,
        Medium = 15,
        Thick = 20
    }


    public enum ToolbarHeadIconStyle {
        NewStyleIccBlack,
        NewStyleIccWhite,
        OldStyleIccColored,
        OldStyleIccBlack,
        OldStyleIccWhite,
        SmileEmoji,
    }

    #endregion

    public class Settings {
        #region 工具栏相关

        [JsonProperty(PropertyName = "toolbar.zoom")]
        public double ToolbarZoomFactor = 1.0;

        /// <summary>
        /// 工具栏的默认位置，默认值为Bottom
        /// </summary>
        [JsonProperty(PropertyName = "toolbar.placement")]
        public ToolbarPlacement ToolbarDefaultPlacement = ToolbarPlacement.Bottom;

        [JsonProperty(PropertyName = "toolbar.placement.xMargin")]
        public double ToolbarDefaultPlacementXMargin = 0;

        [JsonProperty(PropertyName = "toolbar.placement.yMargin")]
        public double ToolbarDefaultPlacementYMargin = 24;

        /// <summary>
        /// 工具栏的自定义默认位置，默认值为(0,0)，仅在ToolbarDefaultPlacement为Custom时有效。
        /// </summary>
        [JsonProperty(PropertyName = "toolbar.placement.custom")]
        public Point ToolbarCustomDefaultPlacement = new Point(0, 0);

        /// <summary>
        /// 工具栏定位锚点。
        /// </summary>
        [JsonProperty(PropertyName = "toolbar.placement.anchor")]
        public Point ToolbarAnchorPoint = new Point(0, 0);

        [JsonProperty(PropertyName = "toolbar.color.head.background")]
        public Color ToolbarHeadBackgroundColor = (Color)ColorConverter.ConvertFromString("#FFffffff");

        [JsonProperty(PropertyName = "toolbar.color.head.iconStyle")]
        public ToolbarHeadIconStyle ToolbarHeadIconStyle = ToolbarHeadIconStyle.NewStyleIccBlack;

        [JsonProperty(PropertyName = "toolbar.color.head.border")]
        public Color ToolbarHeaderBorderColor = (Color)ColorConverter.ConvertFromString("#FF9f9fa9");

        [JsonProperty(PropertyName = "toolbar.borderThickness.head")]
        public ToolbarBorderThickness ToolbarHeaderBorderThickness = ToolbarBorderThickness.Medium;

        [JsonProperty(PropertyName = "toolbar.color.body.background")]
        public Color ToolbarBodyBackgroundColor = (Color)ColorConverter.ConvertFromString("#FFffffff");

        [JsonProperty(PropertyName = "toolbar.color.body.iconColor")]
        public Color ToolbarBodyIconColor = (Color)ColorConverter.ConvertFromString("#FF000000");

        [JsonProperty(PropertyName = "toolbar.color.body.fontColor")]
        public Color ToolbarBodyFontColor = (Color)ColorConverter.ConvertFromString("#FF000000");

        [JsonProperty(PropertyName = "toolbar.color.body.buttonPointerOverBackground")]
        public Color ToolbarBodyButtonPointerOverBackground = (Color)ColorConverter.ConvertFromString("#14000000");

        [JsonProperty(PropertyName = "toolbar.color.body.buttonPressedFontColor")]
        public Color ToolbarBodyButtonPressedFontColor = (Color)ColorConverter.ConvertFromString("#FF000000");

        [JsonProperty(PropertyName = "toolbar.color.body.buttonPressedIconColor")]
        public Color ToolbarBodyButtonPressedIconColor = (Color)ColorConverter.ConvertFromString("#FF000000");

        [JsonProperty(PropertyName = "toolbar.color.body.buttonPressedBackground")]
        public Color ToolbarBodyButtonPressedBackground = (Color)ColorConverter.ConvertFromString("#8C9F9F9F");

        [JsonProperty(PropertyName = "toolbar.fontSize.body")]
        public double ToolbarBodyFontSize = 11;

        /// <summary>
        /// 工具栏的自动折叠，开启后，工具栏将会在展开后指定条件下自动折叠。
        /// </summary>
        [JsonProperty(PropertyName = "toolbar.behaviour.autoFold.enabled")]
        public bool IsToolbarAutoFoldEnabled = false;

        /// <summary>
        /// 工具栏自动折叠的规则。具体请看文档
        /// </summary>
        [JsonProperty(PropertyName = "toolbar.behaviour.autoFold.schema")]
        public string ToolbarAutoFoldRuleSchema = "idleTime >= 20.00s && mouseStatus == idle";

        /// <summary>
        /// 鼠标移动到工具栏 icc 按钮上时，自动展开被规则折叠的工具栏。
        /// </summary>
        [JsonProperty(PropertyName = "toolbar.behaviour.autoFold.autoUnfold")]
        public bool IsToolbarFoldedBySchemaAutoUnfold = false;

        /// <summary>
        /// 工具栏的自动变淡功能，开启后会根据条件自动让工具栏的不透明度降低。
        /// </summary>
        [JsonProperty(PropertyName = "toolbar.behaviour.autoFading.enabled")]
        public bool IsToolbarAutoFadingEnabled = true;

        /// <summary>
        /// 工具栏自动变淡的规则。具体请看文档
        /// </summary>
        [JsonProperty(PropertyName = "toolbar.behaviour.autoFading.schema")]
        public string ToolbarAutoFadingRuleSchema = "idleTime >= 5.00s && !isMouseIn";

        /// <summary>
        /// 工具栏自动变淡的不透明度。
        /// </summary>
        [JsonProperty(PropertyName = "toolbar.behaviour.autoFading.opacity")]
        public double ToolbarAutoFadingOpacity = 0.5;

        [JsonProperty(PropertyName = "toolbar.opacity.normal")]
        public double ToolbarNormalOpacity = 1;

        [JsonProperty(PropertyName = "toolbar.opacity.presentationMode")]
        public double ToolbarPresentationModeOpacity = 1;

        #endregion
    }
}
