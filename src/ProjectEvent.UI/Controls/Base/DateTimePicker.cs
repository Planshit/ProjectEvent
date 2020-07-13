using ProjectEvent.UI.Controls.Input;
using ProjectEvent.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectEvent.UI.Controls.Base
{
    public class DateTimePicker : Control
    {
        #region 依赖属性
        #region 当前选中时间
        public DateTime SelectedDateTime
        {
            get { return (DateTime)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }
        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime",
                typeof(DateTime),
                typeof(DateTimePicker), new PropertyMetadata(new PropertyChangedCallback(OnSelectedDateTimeChanged)));

        private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DateTimePicker;
            if (e.NewValue != e.OldValue)
            {
                control.Render();
            }
        }

        #endregion

        #region 当前选中年月
        public string SelectedYearMonth
        {
            get { return (string)GetValue(SelectedYearMonthProperty); }
            set { SetValue(SelectedYearMonthProperty, value); }
        }
        public static readonly DependencyProperty SelectedYearMonthProperty =
            DependencyProperty.Register("SelectedYearMonth",
                typeof(string),
                typeof(DateTimePicker), new PropertyMetadata($"{DateTime.Now.ToString("yyyy年MM月")}"));

        #endregion

        #region 年份选择
        public string SelectedYear
        {
            get { return (string)GetValue(SelectedYearProperty); }
            set { SetValue(SelectedYearProperty, value); }
        }
        public static readonly DependencyProperty SelectedYearProperty =
            DependencyProperty.Register("SelectedYear",
                typeof(string),
                typeof(DateTimePicker), new PropertyMetadata($"{DateTime.Now.ToString("yyyy年")}"));

        #endregion

        #region 当前选中时间字符串
        public string SelectedDateTimeStr
        {
            get { return (string)GetValue(SelectedDateTimeStrProperty); }
            set { SetValue(SelectedDateTimeStrProperty, value); }
        }
        public static readonly DependencyProperty SelectedDateTimeStrProperty =
            DependencyProperty.Register("SelectedDateTimeStr",
                typeof(string),
                typeof(DateTimePicker));

        #endregion

        #region 月份选择上调年份命令
        public Command AddYearCommand
        {
            get { return (Command)GetValue(AddYearCommandProperty); }
            set { SetValue(AddYearCommandProperty, value); }
        }
        public static readonly DependencyProperty AddYearCommandProperty =
            DependencyProperty.Register("AddYearCommand",
                typeof(Command),
                typeof(DateTimePicker));

        #endregion

        #region 月份选择下调年份命令
        public Command SubtractYearCommand
        {
            get { return (Command)GetValue(SubtractYearCommandProperty); }
            set { SetValue(SubtractYearCommandProperty, value); }
        }
        public static readonly DependencyProperty SubtractYearCommandProperty =
            DependencyProperty.Register("SubtractYearCommand",
                typeof(Command),
                typeof(DateTimePicker));

        #endregion
        #region 月份选择命令
        public Command SelectMonthCommand
        {
            get { return (Command)GetValue(SelectMonthCommandProperty); }
            set { SetValue(SelectMonthCommandProperty, value); }
        }
        public static readonly DependencyProperty SelectMonthCommandProperty =
            DependencyProperty.Register("SelectMonthCommand",
                typeof(Command),
                typeof(DateTimePicker));

        #endregion

        #region 当前选中小时
        public string SelectedHour
        {
            get { return (string)GetValue(SelectedHourProperty); }
            set { SetValue(SelectedHourProperty, value); }
        }
        public static readonly DependencyProperty SelectedHourProperty =
            DependencyProperty.Register("SelectedHour",
                typeof(string),
                typeof(DateTimePicker), new PropertyMetadata($"{DateTime.Now.ToString("HH")}", new PropertyChangedCallback(OnSelectedTimeChanged)));

        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DateTimePicker;
            if (e.NewValue != e.OldValue)
            {
                control.SelectedDateTime = new DateTime(control.SelectedDateTime.Year,
                    control.SelectedDateTime.Month,
                    control.SelectedDateTime.Day,
                    int.Parse(control.SelectedHour),
                    int.Parse(control.SelectedMinutes),
                    0);
            }
        }

        #endregion
        #region 当前选中分钟
        public string SelectedMinutes
        {
            get { return (string)GetValue(SelectedMinutesProperty); }
            set { SetValue(SelectedMinutesProperty, value); }
        }
        public static readonly DependencyProperty SelectedMinutesProperty =
            DependencyProperty.Register("SelectedMinutes",
                typeof(string),
                typeof(DateTimePicker), new PropertyMetadata($"{DateTime.Now.ToString("mm")}", new PropertyChangedCallback(OnSelectedTimeChanged)));

        #endregion


        #region 月份选择上调命令
        public Command AddMonthCommand
        {
            get { return (Command)GetValue(AddMonthCommandProperty); }
            set { SetValue(AddMonthCommandProperty, value); }
        }
        public static readonly DependencyProperty AddMonthCommandProperty =
            DependencyProperty.Register("AddMonthCommand",
                typeof(Command),
                typeof(DateTimePicker));

        #endregion

        #region 月份选择下调命令
        public Command SubtractMonthCommand
        {
            get { return (Command)GetValue(SubtractMonthCommandProperty); }
            set { SetValue(SubtractMonthCommandProperty, value); }
        }
        public static readonly DependencyProperty SubtractMonthCommandProperty =
            DependencyProperty.Register("SubtractMonthCommand",
                typeof(Command),
                typeof(DateTimePicker));

        #endregion
        #endregion
        private InputBox HourInputBox;
        //月份切换
        private int YearNum = DateTime.Now.Year;
        private int MonthNum = DateTime.Now.Month;
        //独立的月份选择器
        private int SelectedYearNum = DateTime.Now.Year;
        private int SelectedMonthNum = DateTime.Now.Month;
        private WrapPanel DaysWrapPanel;
        private Border WeekTextBorder;
        private Dictionary<string, Button> daysButtons = new Dictionary<string, Button>();
        private Button selectedButton;
        public DateTimePicker()
        {
            DefaultStyleKey = typeof(DateTimePicker);
            AddYearCommand = new Command(new Action<object>(OnAddYearCommand));
            SubtractYearCommand = new Command(new Action<object>(OnSubtractYearCommand));
            SelectMonthCommand = new Command(new Action<object>(OnSelectMonthCommand));
            AddMonthCommand = new Command(new Action<object>(OnAddMonthCommand));
            SubtractMonthCommand = new Command(new Action<object>(OnSubtractMonthCommand));
            SelectedDateTime = DateTime.Now;
        }
        #region 左边的月份切换器命令
        private void OnSubtractMonthCommand(object obj)
        {
            ActionMonth(false);

        }

        private void OnAddMonthCommand(object obj)
        {
            ActionMonth();
        }
        private void ActionMonth(bool isadd = true)
        {
            var newdatetime = new DateTime(YearNum, MonthNum, 1);
            newdatetime = newdatetime.AddMonths(isadd ? 1 : -1);
            YearNum = newdatetime.Year;
            MonthNum = newdatetime.Month;
            SelectedYearMonth = newdatetime.ToString("yyyy年MM月");
            RenderDays(YearNum, MonthNum);
        }
        #endregion
        private void OnSelectMonthCommand(object obj)
        {
            SelectedMonthNum = int.Parse(obj.ToString());
            SelectedYearMonth = $"{SelectedYearNum}年{SelectedMonthNum}月";
            RenderDays();
        }

        private void OnSubtractYearCommand(object obj)
        {
            SelectedYearNum--;
            SelectedYear = $"{SelectedYearNum}年";
        }

        private void OnAddYearCommand(object obj)
        {
            SelectedYearNum++;
            SelectedYear = $"{SelectedYearNum}年";
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HourInputBox = GetTemplateChild("HourInputBox") as InputBox;
            DaysWrapPanel = GetTemplateChild("DaysWrapPanel") as WrapPanel;
            WeekTextBorder = GetTemplateChild("WeekTextBorder") as Border;
            Loaded += DateTimePicker_Loaded;
        }

        private void DateTimePicker_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedYearNum = SelectedDateTime.Year;
            SelectedMonthNum = SelectedDateTime.Month;
            RenderDays();
            MarkSelectedBtn();
            Render();
        }

        #region 根据当前选择时间渲染控件
        private void Render()
        {
            SelectedYearMonth = SelectedDateTime.ToString("yyyy年MM月");
            SelectedYear = SelectedDateTime.ToString("yyyy年");
            SelectedHour = SelectedDateTime.ToString("HH");
            SelectedMinutes = SelectedDateTime.ToString("mm");
            SelectedDateTimeStr = SelectedDateTime.ToString($"yyyy年MM月dd日 星期{ToCNWeekString()} HH:mm");
            YearNum = SelectedDateTime.Year;
            MonthNum = SelectedDateTime.Month;
            //SelectedTime = SelectedDateTime.ToString("HH:mm");
        }
        //渲染日期选择
        private void RenderDays(int year = 0, int month = 0)
        {
            if (DaysWrapPanel != null)
            {
                DaysWrapPanel.Children.Clear();
                daysButtons.Clear();
                year = year == 0 ? SelectedYearNum : year;
                month = month == 0 ? SelectedMonthNum : month;

                //判断1号是否是周一
                var selectedyearMonth = new DateTime(year, month, 1);
                int days = DateTime.DaysInMonth(year, month);
                if (selectedyearMonth.DayOfWeek == DayOfWeek.Monday)
                {
                    //如果该月1号是星期一时直接渲染
                    for (int i = 0; i < days; i++)
                    {
                        RenderDayButton(year, month, i + 1);
                    }
                }
                else
                {
                    //该月不是一号时渲染之前的月份

                    //1号星期几
                    int weeknum = (int)selectedyearMonth.DayOfWeek;
                    //1号距离上周星期一相差几天
                    int diffweek1days = weeknum == 0 ? 6 : weeknum - 1;
                    //找到上周星期一的日期
                    var lastWeekDate = new DateTime(year, month, 1).AddDays(-diffweek1days);
                    //获得这个日期的总天数
                    int lastWeekDateDays = DateTime.DaysInMonth(lastWeekDate.Year, lastWeekDate.Month);
                    //先渲染上周的日期
                    for (int i = lastWeekDate.Day; i <= lastWeekDateDays; i++)
                    {
                        RenderDayButton(lastWeekDate.Year, lastWeekDate.Month, i, false);
                    }
                    //再渲染这个月的日期
                    for (int i = 0; i < days; i++)
                    {
                        RenderDayButton(year, month, i + 1);
                    }
                }
            }
        }
        private void RenderDayButton(int year, int month, int day, bool isthismonth = true)
        {
            if (DaysWrapPanel != null)
            {
                double width = WeekTextBorder.ActualWidth;
                var btn = new Button();
                btn.Style = FindResource("Icon") as Style;
                btn.Width = width;
                btn.Height = width;
                btn.Padding = new Thickness(0);
                btn.Content = day;
                btn.Click += (e, c) =>
                {
                    SelectedYearNum = year;
                    SelectedMonthNum = month;
                    if (!isthismonth)
                    {
                        //非本月时需要先重新渲染该月
                        RenderDays();
                    }
                    //设置选中日期
                    SelectedDateTime = new DateTime(year, month, day, int.Parse(SelectedHour), int.Parse(SelectedMinutes), 0);
                   
                    MarkSelectedBtn();
                };
                if (!isthismonth)
                {
                    btn.Foreground = FindResource("DisabledTextBrush") as SolidColorBrush;
                }
                DaysWrapPanel.Children.Add(btn);
                daysButtons.Add($"{year}{month}{day}", btn);
            }
        }
        #endregion

        private void MarkSelectedBtn()
        {
            //标记选择的日期
            string key = SelectedDateTime.ToString("yyyyMd");
            if (daysButtons.ContainsKey(key))
            {
                daysButtons[key].IsEnabled = false;
                if (selectedButton != null)
                {
                    selectedButton.IsEnabled = true;
                }
                selectedButton = daysButtons[key];

            }
        }

        /// <summary>
        /// 获取当前日期的中国星期字符串
        /// </summary>
        /// <returns></returns>
        private string ToCNWeekString()
        {
            string[] cnweeks = { "日", "一", "二", "三", "四", "五", "六" };
            return cnweeks[(int)SelectedDateTime.DayOfWeek];
        }
    }
}
