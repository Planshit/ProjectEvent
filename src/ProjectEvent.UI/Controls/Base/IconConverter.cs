using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.UI.Controls.Base
{
    public class IconConverter
    {
        private static Dictionary<IconTypes, string> iconUnicodes = new Dictionary<IconTypes, string>()
        {
            {IconTypes.None,""},
            {IconTypes.Back,"\xE72B"},
            {IconTypes.Calendar,"\xE787"},
            {IconTypes.ChevronDown,"\xE70D"},
            {IconTypes.CheckMark,"\xE73E"},
            {IconTypes.CompletedSolid,"\xEC61"},
            {IconTypes.RadioBtnOn,"\xECCB"},
            {IconTypes.StatusCircleOuter,"\xF136"},
            {IconTypes.AcceptMedium,"\xF78C"},
            {IconTypes.Accept,"\xE8FB"},
            {IconTypes.Timer,"\xE91E"},
            {IconTypes.StatusCircleQuestionMark,"\xF142"},
            {IconTypes.ProjectDocument,"\xF759"},
            {IconTypes.AutomateFlow,"\xE3F5"},
            {IconTypes.FlashAuto,"\xE95C"},
            {IconTypes.ProductVariant,"\xEE30"},
            {IconTypes.AppIconDefault,"\xECAA"},
            {IconTypes.ChromeMinimize,"\xE921"},
            {IconTypes.ChromeClose,"\xE8BB"},
            {IconTypes.ChromeRestore,"\xE923"},
            {IconTypes.CalculatorMultiply,"\xE947"},
            {IconTypes.SquareShape,"\xF1A6"},
            {IconTypes.More,"\xE712"},
            {IconTypes.AppIconDefaultList,"\xEFDE"},
            {IconTypes.ClipboardList,"\xF0E3"},
            {IconTypes.Settings,"\xE713"},
            {IconTypes.ChromeBack,"\xE830"},
            {IconTypes.BulletedList,"\xE8FD"},
            {IconTypes.CalculatorAddition,"\xE948"},
            {IconTypes.DeviceRun,"\xE401"},
            {IconTypes.ProcessingRun,"\xE404"},
            {IconTypes.Product,"\xECDC"},
            {IconTypes.FileTemplate,"\xF2E6"},
            {IconTypes.DeviceOff,"\xE402"},
            {IconTypes.FlowChart,"\xE9D4"},
            {IconTypes.DownloadDocument,"\xF549"},
            {IconTypes.Delete,"\xE74D"},
            {IconTypes.EditStyle,"\xEF60"},
            {IconTypes.OpenWithMirrored,"\xEA5C"},
            {IconTypes.FileCode,"\xF30E"},
            {IconTypes.BuildIssue,"\xF319"},
            {IconTypes.WebEnvironment,"\xE3DB"},
            {IconTypes.DesktopScreenshot,"\xF5D9"},
            {IconTypes.MusicInCollectionFill,"\xEA36"},
            {IconTypes.NetworkTower,"\xEC05"},
            {IconTypes.KeyboardClassic,"\xE765"},
            {IconTypes.FabricSyncFolder,"\xF0A7"},
            {IconTypes.DateTime12,"\xF38F"},
            {IconTypes.DateTime,"\xEC92"},
            {IconTypes.MyNetwork,"\xEC27"},
            {IconTypes.WifiEthernet,"\xEE77"},
            {IconTypes.NormalWeight,"\xF4EF"},
            {IconTypes.Message,"\xE8BD"},
            {IconTypes.Download,"\xE896"},
            {IconTypes.Storyboard,"\xF308"},
            {IconTypes.HandsFree,"\xEAD0"},
            {IconTypes.PlaybackRate1x,"\xEC57"},

            {IconTypes.ChevronUp,"\xE70E"}

        };
        public static string ToUnicode(IconTypes iconType)
        {
            return iconUnicodes[iconType];
        }
        public static IconTypes ToType(string unicode)
        {
            var type = iconUnicodes.Where(m => m.Value == unicode).Select(s => s.Key).FirstOrDefault();
            return type;
        }
    }
}
