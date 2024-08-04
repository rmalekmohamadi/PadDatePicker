using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using PadDatePicker.Tools;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace PadDatePicker
{
    public partial class PadDatePicker : PInputBase<DateTimeOffset?>
    {
        /// <summary>
        /// Whether or not the DatePicker's callout is open
        /// </summary>
        [Parameter]
        public bool IsOpen
        {
            get => isOpen;
            set
            {
                if (isOpen == value) return;

                isOpen = value;

                _ = IsOpenChanged.InvokeAsync(value);
            }
        }
        private bool isOpen;

        [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

        /// <summary>
        /// Custom CSS classes for different parts of the DatePicker component.
        /// </summary>
        [Parameter] public DatePickerClassStyles? Classes { get; set; } = new DatePickerClassStyles();

        /// <summary>
        /// CultureInfo for the DatePicker.
        /// </summary>
        [Parameter]
        public CultureInfo Culture
        {
            get => culture;
            set
            {
                if (culture == value) return;

                culture = value;
            }
        }
        private CultureInfo culture = CultureInfo.CurrentUICulture;

        // [Parameter] public PSize Size { get; set; } = PSize.Base;
        /// <summary>
        /// Whether or not the DatePicker allows a string date input.
        /// </summary>
        [Parameter] public bool AllowTextInput { get; set; } = true;

        /// <summary>
        /// Whether the DatePicker closes automatically after selecting the date.
        /// </summary>
        [Parameter] public bool AutoClose { get; set; } = true;

        /// <summary>
        /// The format of the date in the DatePicker.
        /// </summary>
        [Parameter] public string? DateFormat { get; set; }

        /// <summary>
        /// The time format of the time-picker, 24H or 12H.
        /// </summary>
        [Parameter] public TimeFormat TimeFormat { get; set; }

        /// <summary>
        /// Custom template to render the day cells of the DatePicker.
        /// </summary>
        [Parameter] public RenderFragment<DateTimeOffset>? DayCellTemplate { get; set; }

        /// <summary>
        /// Custom template to render the month cells of the DatePicker.
        /// </summary>
        [Parameter] public RenderFragment<DateTimeOffset>? MonthCellTemplate { get; set; }

        /// <summary>
        /// Custom template to render the year cells of the DatePicker.
        /// </summary>
        [Parameter] public RenderFragment<int>? YearCellTemplate { get; set; }

        /// <summary>
        /// Custom template for the DatePicker's icon.
        /// </summary>
        [Parameter] public RenderFragment? IconTemplate { get; set; }

        /// <summary>
        /// Determines the location of the DatePicker's icon.
        /// </summary>
        [Parameter]
        public Side IconSide { get; set; }

        /// <summary>
        /// The custom validation error message for the invalid value.
        /// </summary>
        [Parameter] public string? InvalidErrorMessage { get; set; }

        /// <summary>
        /// The text of the DatePicker's label.
        /// </summary>
        [Parameter] public string? Label { get; set; }

        /// <summary>
        /// The maximum date allowed for the DatePicker.
        /// </summary>
        [Parameter] public DateTimeOffset? MaxDate { get; set; }

        /// <summary>
        /// The minimum date allowed for the DatePicker.
        /// </summary>
        [Parameter] public DateTimeOffset? MinDate { get; set; }

        /// <summary>
        /// Whether the DatePicker's close button should be shown or not.
        /// </summary>
        [Parameter] public bool ShowCloseButton { get; set; } = true;

        /// <summary>
        /// Whether the clear button should be shown or not when the DatePicker has a value.
        /// </summary>
        [Parameter] public bool ShowClearButton { get; set; }

        /// <summary>
        /// Text of clear button.
        /// </summary>
        [Parameter] public string ClearButtonText { get; set; } = "Clear";

        /// <summary>
        /// Whether the go today button should be shown or not when the.
        /// </summary>
        [Parameter] public bool ShowToDayButton { get; set; } = true;

        /// <summary>
        /// Text of go today button.
        /// </summary>
        [Parameter] public string ToDayButtonText { get; set; } = "Today";

        /// <summary>
        /// Whether the current item should be highlight or not when the.
        /// </summary>
        [Parameter] public bool HighlightCurrent { get; set; } = true;

        /// <summary>
        /// Determines increment/decrement steps for date-picker's hour.
        /// </summary>
        [Parameter] public int HourStep { get; set; }

        /// <summary>
        /// Determines increment/decrement steps for date-picker's minute.
        /// </summary>
        [Parameter] public int MinuteStep { get; set; }

        protected override void OnInitialized()
        {
            _dateTimeManager = new DateTimeManager(Culture, MinDate, MaxDate);
        }
        protected override void OnParametersSet()
        {
            var dateTime = Value.GetValueOrDefault(DateTimeOffset.Now);

            if (MinDate.HasValue && MinDate > dateTime)
            {
                dateTime = MinDate.Value;
            }

            if (MaxDate.HasValue && MaxDate < dateTime)
            {
                dateTime = MaxDate.Value;
            }

            _hour = Value.HasValue ? dateTime.Hour : 0;
            _minute = Value.HasValue ? dateTime.Minute : 0;

            GenerateCalendarData(dateTime.DateTime);

            base.OnParametersSet();
        }

        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateTimeOffset? result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                validationErrorMessage = null;
                return true;
            }

            if (DateTime.TryParseExact(value, DateFormat ?? Culture.DateTimeFormat.ShortDatePattern, Culture, DateTimeStyles.None, out DateTime parsedValue))
            {
                result = new DateTimeOffset(parsedValue, DateTimeOffset.Now.Offset);
                validationErrorMessage = null;
                return true;
            }

            result = default;
            validationErrorMessage = !string.IsNullOrWhiteSpace(InvalidErrorMessage) ? InvalidErrorMessage! : $"The {Label} field is not valid.";
            return false;
        }

        protected override string? FormatValueAsString(DateTimeOffset? value)
        {
            //return null;
            return value.HasValue
                ? value.Value.ToString(DateFormat ?? Culture.DateTimeFormat.ShortDatePattern, Culture)
                : null;
        }

        private Dictionary<string, object> SetAttributes()
        {
            var dict = new Dictionary<string, object>();
            if (!AllowTextInput) dict.Add("readonly", "readonly");
            return dict;
        }

        private ElementReference _inputRef = default!;

        protected PadDatePickerViewType _bodyViewType = PadDatePickerViewType.Day;
        protected PadDatePickerViewType _headerViewType = PadDatePickerViewType.Month;

        private void GoToTimeView()
        {
            _bodyViewType = PadDatePickerViewType.Time;
            _headerViewType = PadDatePickerViewType.None;
        }

        private void GoToDayView()
        {
            _bodyViewType = PadDatePickerViewType.Day;
            _headerViewType = PadDatePickerViewType.Month;
        }

        private void GoToMonthView()
        {
            _bodyViewType = PadDatePickerViewType.Month;
            _headerViewType = PadDatePickerViewType.Year;
        }

        private void GoToYearView()
        {
            _bodyViewType = PadDatePickerViewType.Year;
            _headerViewType = PadDatePickerViewType.YearRange;
        }
        //private const int DEFAULT_DAY_COUNT_PER_WEEK = 7;
        //private const int DEFAULT_WEEK_COUNT = 6;
        //private const int STEP_DELAY = 75;
        //private const int INITIAL_STEP_DELAY = 400;
        private int _hour;
        private int _hourView
        {
            get
            {
                if (TimeFormat == TimeFormat.TwelveHours)
                {
                    if (_hour > 12)
                    {
                        return _hour - 12;
                    }

                    if (_hour == 0)
                    {
                        return 12;
                    }
                }

                return _hour;
            }
            set
            {
                if (value > 23)
                {
                    _hour = 23;
                }
                else if (value < 0)
                {
                    _hour = 0;
                }
                else
                {
                    _hour = value;
                }

                _ = UpdateValue();
            }
        }

        private int _minute;
        private int _minuteView
        {
            get => _minute;
            set
            {
                if (value > 59)
                {
                    _minute = 59;
                }
                else if (value < 0)
                {
                    _minute = 0;
                }
                else
                {
                    _minute = value;
                }

                _ = UpdateValue();
            }
        }

        private DateTimeManager _dateTimeManager;
        //private int _currentDay;
        //private int _currentYear;
        //private int _currentMonth;
        private int? _selectedDateWeek;
        private int _yearPickerEndYear;
        private int _yearPickerStartYear;
        private int? _selectedDateDayOfWeek;
        //private bool _showMonthPicker = true;
        //private bool _isMonthPickerOverlayOnTop;
        //private bool _showMonthPickerAsOverlayInternal;
        private string _monthTitle = string.Empty;
        private string _readonly => AllowTextInput ? "" : "readonly";
        //private bool _isTimePickerOverlayOnTop;
        //private bool _showTimePickerAsOverlayInternal;
        //private readonly int[,] _daysOfCurrentMonth = new int[DEFAULT_WEEK_COUNT, DEFAULT_DAY_COUNT_PER_WEEK];

        //private int _todayYear => Culture.Calendar.GetYear(DateTime.Now);
        //private int _todayMonth => Culture.Calendar.GetMonth(DateTime.Now);
        //private int _todayDay => Culture.Calendar.GetDayOfMonth(DateTime.Now);
        //public bool IsToDay(int week, int day) => _todayYear == _currentYear && _todayMonth == _currentMonth && _todayDay == DaysOfCurrentMonth(week, day);
        //public bool IsSelectedDay(int week, int day) => _currentDay == DaysOfCurrentMonth(week, day);
        //public bool IsToMonth(int index) => _todayYear == _currentYear && _todayMonth == index;
        //public bool IsSelectedMonth(int index) => _currentMonth == index;
        //public bool IsToYear(int year) => _todayYear == year;
        //public bool IsSelectedYear(int year) => _currentYear == year;

        public void HandleClickHeader()
        {
            if (_bodyViewType.Equals(PadDatePickerViewType.Day))
            {
                GoToMonthView();
            }
            else if (_bodyViewType.Equals(PadDatePickerViewType.Month))
            {
                GoToYearView();
            }
            StateHasChanged();
        }

        public void GenerateMonthData(int year, int month)
        {
            _monthTitle = $"{Culture.DateTimeFormat.GetMonthName(month)} {year}";
            _dateTimeManager.GenerateMonthData(Value, year, month);
        }

        public string GetHeaderTitle()
        {
            if (_headerViewType.Equals(PadDatePickerViewType.Month)) return _monthTitle;
            else if (_headerViewType.Equals(PadDatePickerViewType.Year)) return _dateTimeManager.SelectedYear.ToString();
            else return "";
        }

        public PadDatePickerViewType GetBodyView => _bodyViewType;
        public PadDatePickerViewType GetHeaderView => _headerViewType;

        public bool GetHeaderVisibility => !_headerViewType.Equals(PadDatePickerViewType.None);

        public int GetYearPickerStartYear => _yearPickerStartYear;


        // private async Task HandleOnClick()
        // {
        //     if (IsDisabled is true) return;
        //     if (IsOpenChanged.HasDelegate is false) return;

        //     IsOpen = true;
        //     var result = await ToggleCallout();

        //     if (_showMonthPickerAsOverlayInternal is false)
        //     {
        //         _showMonthPickerAsOverlayInternal = result;
        //     }

        //     if (_showMonthPickerAsOverlayInternal)
        //     {
        //         _isMonthPickerOverlayOnTop = false;
        //     }

        //     if (_showTimePickerAsOverlayInternal is false)
        //     {
        //         _showTimePickerAsOverlayInternal = result;
        //     }

        //     if (_showTimePickerAsOverlayInternal)
        //     {
        //         _isTimePickerOverlayOnTop = false;
        //     }

        //     if (Value.HasValue)
        //     {
        //         CheckCurrentCalendarMatchesValue();
        //     }

        //     await OnClick.InvokeAsync();
        // }

        //private async Task HandleOnFocusIn()
        //{
        //    if (IsDisabled is true) return;

        //    isOpen = true;

        //    //_focusClass = $"{RootElementClass}-foc";

        //    await OnFocusIn.InvokeAsync();
        //}

        public void Open()
        {
            IsOpen = true;
            StateHasChanged();
        }

        public void Close()
        {
            IsOpen = false;
            StateHasChanged();
        }

        private async Task HandleOnFocusOut()
        {
            if (IsDisabled is true) return;

            // if (isOpen is false) return;

            // isOpen = false;

            //_focusClass = string.Empty;

            await OnFocusOut.InvokeAsync();
        }

        private async Task HandleOnFocus(FocusEventArgs args)
        {
            if (IsDisabled is true) return;

            if (isOpen is true) return;

            isOpen = true;

            //_focusClass = $"{RootElementClass}-foc";

            await OnFocus.InvokeAsync();
        }

        private async Task HandleOnChange(ChangeEventArgs args)
        {
            if (IsDisabled is true) return;
            //if (ValueChanged.HasDelegate is false) return;
            if (AllowTextInput is false) return;

            var oldValue = Value.GetValueOrDefault(DateTimeOffset.Now);
            CurrentValueAsString = args.Value.ToString();
            var curValue = Value.GetValueOrDefault(DateTimeOffset.Now);
            if (IsOpen && oldValue != curValue)
            {
                CheckCurrentCalendarMatchesValue();
                if (curValue.Year != oldValue.Year)
                {
                    _dateTimeManager.SelectedYear = curValue.Year;
                    ChangeYearRanges(_dateTimeManager.SelectedYear - 1);
                }
            }

            GenerateCalendarData(Value.Value.DateTime);

            StateHasChanged();
        }

        private async Task ClearButtonClick()
        {
            if (IsDisabled is true) return;

            Value = null;

            _hour = 0;
            _minute = 0;

            _selectedDateWeek = null;
            _selectedDateDayOfWeek = null;

            GenerateCalendarData(DateTime.Now);

            await _inputRef.FocusAsync();

            StateHasChanged();
        }

        public async Task SelectDate(int dayIndex, int weekIndex)
        {
            if (IsDisabled is true) return;
            //if (ValueChanged.HasDelegate is false) return;
            //if (IsOpenChanged.HasDelegate is false) return;
            
            _dateTimeManager.SelectDate(dayIndex, weekIndex);

            if (AutoClose)
            {
                IsOpen = false;
            }

            var currentDateTime = Culture.Calendar.ToDateTime(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth, _dateTimeManager.SelectedDay, _hour, _minute, 0, 0);
            Value = new DateTimeOffset(currentDateTime, DateTimeOffset.Now.Offset);

            GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);

            StateHasChanged();
        }

        public async Task SelectMonth(int month)
        {
            if (IsDisabled is true) return;
            if (IsMonthOutOfMinAndMaxDate(month)) return;

            _dateTimeManager.SelectedMonth = month;

            GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);

            _headerViewType = PadDatePickerViewType.Month;
            _bodyViewType = PadDatePickerViewType.Day;

            var currentDateTime = Culture.Calendar.ToDateTime(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth, _dateTimeManager.SelectedDay, _hour, _minute, 0, 0);
            Value = new DateTimeOffset(currentDateTime, DateTimeOffset.Now.Offset);

            StateHasChanged();
        }

        public async Task SelectYear(int year)
        {
            if (IsDisabled is true) return;
            if (IsYearOutOfMinAndMaxDate(year)) return;

            _dateTimeManager.SelectedYear = year;

            ChangeYearRanges(_dateTimeManager.SelectedYear - 1);

            GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);

            _headerViewType = PadDatePickerViewType.Year;
            _bodyViewType = PadDatePickerViewType.Month;

            var currentDateTime = Culture.Calendar.ToDateTime(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth, _dateTimeManager.SelectedDay, _hour, _minute, 0, 0);
            Value = new DateTimeOffset(currentDateTime, DateTimeOffset.Now.Offset);

            StateHasChanged();
        }

        public void HandleMonthChange(bool isNext)
        {
            if (IsDisabled is true) return;
            if (CanChangeMonth(isNext) is false) return;

            _dateTimeManager.HandleMonthChange(isNext, Value);

            _monthTitle = $"{Culture.DateTimeFormat.GetMonthName(_dateTimeManager.SelectedMonth)} {_dateTimeManager.SelectedYear}";

            StateHasChanged();
        }

        public void HandleYearChange(bool isNext)
        {
            if (IsDisabled is true) return;
            if (CanChangeYear(isNext) is false) return;

            _dateTimeManager.HandleYearChange(isNext, Value);

            StateHasChanged();
        }

        public void HandleYearRangeChange(bool isNext)
        {
            if (IsDisabled is true) return;
            if (CanChangeYearRange(isNext) is false) return;

            var fromYear = _yearPickerStartYear + (isNext ? +12 : -12);

            ChangeYearRanges(fromYear);
        }
        private void ChangeYearRanges(int fromYear)
        {
            _yearPickerStartYear = fromYear;
            _yearPickerEndYear = fromYear + 11;

            StateHasChanged();
        }

        private void GoToToday()
        {
            if (IsDisabled is true) return;

            Value = DateTime.Now;
            GenerateCalendarData(DateTime.Now);

            StateHasChanged();
        }

        private async Task GoToNow()
        {
            if (IsDisabled is true) return;

            _hour = DateTime.Now.Hour;
            _minute = DateTime.Now.Minute;

            await UpdateValue();
        }

        private void GenerateCalendarData(DateTime dateTime)
        {
            _dateTimeManager.SelectedYear = Culture.Calendar.GetYear(dateTime);
            _dateTimeManager.SelectedMonth = Culture.Calendar.GetMonth(dateTime);
            _dateTimeManager.SelectedDay = Culture.Calendar.GetDayOfMonth(dateTime);

            _yearPickerStartYear = _dateTimeManager.SelectedYear - 1;
            _yearPickerEndYear = _dateTimeManager.SelectedYear + 10;

            GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);
        }

        public bool IsYearOutOfMinAndMaxDate(int year) => _dateTimeManager.IsYearOutOfMinAndMaxDate(year);
        public bool IsMonthOutOfMinAndMaxDate(int month) => _dateTimeManager.IsMonthOutOfMinAndMaxDate(month);
        public bool IsToYear(int year) => _dateTimeManager.IsToYear(year);
        public bool IsSelectedYear(int year) => _dateTimeManager.IsSelectedYear(year);
        public bool IsToMonth(int month) => _dateTimeManager.IsToMonth(month);
        public bool IsSelectedMonth(int month) => _dateTimeManager.IsSelectedMonth(month);
        public bool IsToDay(int week, int day) => _dateTimeManager.IsToDay(week, day);
        public bool IsSelectedDay(int week, int day) => _dateTimeManager.IsSelectedDay(week, day);
        public bool IsWeekDayOutOfMinAndMaxDate(int dayIndex, int weekIndex) => _dateTimeManager.IsWeekDayOutOfMinAndMaxDate(dayIndex, weekIndex);
        public bool IsInCurrentMonth(int week, int day) => _dateTimeManager.IsInCurrentMonth(week, day);


        //private void GenerateMonthData(int year, int month)
        //{
        //    _selectedDateWeek = null;
        //    _selectedDateDayOfWeek = null;
        //    _monthTitle = $"{Culture.DateTimeFormat.GetMonthName(month)} {year}";

        //    for (int weekIndex = 0; weekIndex < DEFAULT_WEEK_COUNT; weekIndex++)
        //    {
        //        for (int dayIndex = 0; dayIndex < DEFAULT_DAY_COUNT_PER_WEEK; dayIndex++)
        //        {
        //            _daysOfCurrentMonth[weekIndex, dayIndex] = 0;
        //        }
        //    }

        //    var monthDays = Culture.Calendar.GetDaysInMonth(year, month);
        //    var firstDayOfMonth = Culture.Calendar.ToDateTime(year, month, 1, 0, 0, 0, 0);
        //    var startWeekDay = (int)Culture.DateTimeFormat.FirstDayOfWeek;
        //    var weekDayOfFirstDay = (int)firstDayOfMonth.DayOfWeek;
        //    var correctedWeekDayOfFirstDay = weekDayOfFirstDay > startWeekDay ? startWeekDay : startWeekDay - 7;

        //    var currentDay = 1;
        //    var isCurrentMonthEnded = false;
        //    for (int weekIndex = 0; weekIndex < DEFAULT_WEEK_COUNT; weekIndex++)
        //    {
        //        for (int dayIndex = 0; dayIndex < DEFAULT_DAY_COUNT_PER_WEEK; dayIndex++)
        //        {
        //            if (weekIndex == 0 && currentDay == 1 && weekDayOfFirstDay > dayIndex + correctedWeekDayOfFirstDay)
        //            {
        //                int prevMonth;
        //                int prevMonthDays;
        //                if (month > 1)
        //                {
        //                    prevMonth = month - 1;
        //                    prevMonthDays = Culture.Calendar.GetDaysInMonth(year, prevMonth);
        //                }
        //                else
        //                {
        //                    prevMonth = 12;
        //                    prevMonthDays = Culture.Calendar.GetDaysInMonth(year - 1, prevMonth);
        //                }

        //                if (weekDayOfFirstDay > startWeekDay)
        //                {
        //                    _daysOfCurrentMonth[weekIndex, dayIndex] = prevMonthDays + dayIndex - (weekDayOfFirstDay - startWeekDay - 1);
        //                }
        //                else
        //                {
        //                    _daysOfCurrentMonth[weekIndex, dayIndex] = prevMonthDays + dayIndex - (7 + weekDayOfFirstDay - startWeekDay - 1);
        //                }
        //            }
        //            else if (currentDay <= monthDays)
        //            {
        //                _daysOfCurrentMonth[weekIndex, dayIndex] = currentDay;
        //                currentDay++;
        //            }

        //            if (currentDay > monthDays)
        //            {
        //                currentDay = 1;
        //                isCurrentMonthEnded = true;
        //            }
        //        }

        //        if (isCurrentMonthEnded)
        //        {
        //            break;
        //        }
        //    }
        //    StateHasChanged();
        //    SetSelectedDateWeek();
        //}

        //private void SetSelectedDateWeek()
        //{
        //    if (Culture is null) return;
        //    if (Value.HasValue is false || (_selectedDateWeek.HasValue && _selectedDateDayOfWeek.HasValue)) return;

        //    var year = Culture.Calendar.GetYear(Value.Value.DateTime);
        //    var month = Culture.Calendar.GetMonth(Value.Value.DateTime);

        //    if (year == _currentYear && month == _currentMonth)
        //    {
        //        var dayOfMonth = Culture.Calendar.GetDayOfMonth(Value.Value.DateTime);
        //        var startWeekDay = (int)Culture.DateTimeFormat.FirstDayOfWeek;
        //        var weekDayOfFirstDay = (int)Culture.Calendar.ToDateTime(year, month, 1, 0, 0, 0, 0).DayOfWeek;
        //        var indexOfWeekDayOfFirstDay = (weekDayOfFirstDay - startWeekDay + DEFAULT_DAY_COUNT_PER_WEEK) % DEFAULT_DAY_COUNT_PER_WEEK;

        //        _selectedDateDayOfWeek = ((int)Value.Value.DayOfWeek - startWeekDay + DEFAULT_DAY_COUNT_PER_WEEK) % DEFAULT_DAY_COUNT_PER_WEEK;

        //        var days = indexOfWeekDayOfFirstDay + dayOfMonth;

        //        _selectedDateWeek = days % DEFAULT_DAY_COUNT_PER_WEEK == 0 ? (days / DEFAULT_DAY_COUNT_PER_WEEK) - 1 : days / DEFAULT_DAY_COUNT_PER_WEEK;

        //        if (indexOfWeekDayOfFirstDay is 0)
        //        {
        //            _selectedDateWeek++;
        //        }
        //    }
        //}


        //public bool IsInCurrentMonth(int week, int day)
        //{
        //    return (
        //            ((week == 0 || week == 1) && _daysOfCurrentMonth[week, day] > 20) ||
        //            ((week == 4 || week == 5) && _daysOfCurrentMonth[week, day] < 7)
        //           ) is false;
        //}

        //private int FindMonth(int week, int day)
        //{
        //    int month = _currentMonth;

        //    if (IsInCurrentMonth(week, day) is false)
        //    {
        //        if (week >= 4)
        //        {
        //            month = _currentMonth < 12 ? _currentMonth + 1 : 1;
        //        }
        //        else
        //        {
        //            month = _currentMonth > 1 ? _currentMonth - 1 : 12;
        //        }
        //    }

        //    return month;
        //}

        // private bool IsGoToTodayButtonDisabled(int todayYear, int todayMonth, bool showYearPicker = false)
        // {
        //     if (showYearPicker)
        //     {
        //         return _yearPickerStartYear == todayYear - 1
        //             && _yearPickerEndYear == todayYear + 10
        //             && todayMonth == _dateTimeManager.SelectedMonth
        //             && todayYear == _dateTimeManager.SelectedYear;
        //     }
        //     else
        //     {
        //         return todayMonth == _dateTimeManager.SelectedMonth
        //             && todayYear == _dateTimeManager.SelectedYear;
        //     }
        // }

        public DayOfWeek GetDayOfWeek(int index)
        {
            int dayOfWeek = (int)Culture.DateTimeFormat.FirstDayOfWeek + index;

            if (dayOfWeek > 6)
            {
                dayOfWeek -= 7;
            }

            return (DayOfWeek)dayOfWeek;
        }      

        public bool CanChangeMonth(bool isNext)
        {
            if (isNext && MaxDate.HasValue)
            {
                var MaxDateYear = Culture.Calendar.GetYear(MaxDate.Value.DateTime);
                var MaxDateMonth = Culture.Calendar.GetMonth(MaxDate.Value.DateTime);

                if (MaxDateYear == _dateTimeManager.SelectedYear && MaxDateMonth == _dateTimeManager.SelectedMonth) return false;
            }


            if (isNext is false && MinDate.HasValue)
            {
                var MinDateYear = Culture.Calendar.GetYear(MinDate.Value.DateTime);
                var MinDateMonth = Culture.Calendar.GetMonth(MinDate.Value.DateTime);

                if (MinDateYear == _dateTimeManager.SelectedYear && MinDateMonth == _dateTimeManager.SelectedMonth) return false;
            }

            return true;
        }

        public bool CanChangeYear(bool isNext)
        {
            return (
                    (isNext && MaxDate.HasValue && Culture.Calendar.GetYear(MaxDate.Value.DateTime) == _dateTimeManager.SelectedYear) ||
                    (isNext is false && MinDate.HasValue && Culture.Calendar.GetYear(MinDate.Value.DateTime) == _dateTimeManager.SelectedYear)
                   ) is false;
        }

        private bool CanChangeYearRange(bool isNext)
        {
            return (
                    (isNext && MaxDate.HasValue && Culture.Calendar.GetYear(MaxDate.Value.DateTime) < _yearPickerStartYear + 12) ||
                    (isNext is false && MinDate.HasValue && Culture.Calendar.GetYear(MinDate.Value.DateTime) >= _yearPickerStartYear)
                   ) is false;
        }



        //public bool IsMonthOutOfMinAndMaxDate(int month)
        //{
        //    if (MaxDate.HasValue)
        //    {
        //        var MaxDateYear = Culture.Calendar.GetYear(MaxDate.Value.DateTime);
        //        var MaxDateMonth = Culture.Calendar.GetMonth(MaxDate.Value.DateTime);

        //        if (_currentYear > MaxDateYear || (_currentYear == MaxDateYear && month > MaxDateMonth)) return true;
        //    }

        //    if (MinDate.HasValue)
        //    {
        //        var MinDateYear = Culture.Calendar.GetYear(MinDate.Value.DateTime);
        //        var MinDateMonth = Culture.Calendar.GetMonth(MinDate.Value.DateTime);

        //        if (_currentYear < MinDateYear || (_currentYear == MinDateYear && month < MinDateMonth)) return true;
        //    }

        //    return false;
        //}

        //public bool IsYearOutOfMinAndMaxDate(int year)
        //{
        //    return (MaxDate.HasValue && year > Culture.Calendar.GetYear(MaxDate.Value.DateTime))
        //        || (MinDate.HasValue && year < Culture.Calendar.GetYear(MinDate.Value.DateTime));
        //}

        private void CheckCurrentCalendarMatchesValue()
        {
            var currentValue = Value.GetValueOrDefault(DateTimeOffset.Now);
            var currentValueYear = Culture.Calendar.GetYear(currentValue.DateTime);
            var currentValueMonth = Culture.Calendar.GetMonth(currentValue.DateTime);
            var currentValueDay = Culture.Calendar.GetDayOfMonth(currentValue.DateTime);

            if (currentValueYear != _dateTimeManager.SelectedYear || currentValueMonth != _dateTimeManager.SelectedMonth || (AllowTextInput && currentValueDay != _dateTimeManager.SelectedDay))
            {
                _dateTimeManager.SelectedYear = currentValueYear;
                _dateTimeManager.SelectedMonth = currentValueMonth;
                GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);
            }
        }

        public string GetDayButtonCss(bool isEnable, bool isCurrent, bool isSelected)
        {
            ElementClassBuilder _mainBtnClassBuilder = new ElementClassBuilder();
            _mainBtnClassBuilder.Add(Classes?.BodyBtnDefault);

            if (isEnable)
            {
                _mainBtnClassBuilder.Add(Classes?.BodyBtnEnable);
                if (isSelected)
                {
                    _mainBtnClassBuilder.Add(Classes?.BodyBtnSelected);
                }
                else if(isCurrent && HighlightCurrent)
                {
                    _mainBtnClassBuilder.Add(Classes?.BodyBtnCurrent);
                }
                else
                {
                    _mainBtnClassBuilder.Add(Classes?.BodyBtnNormal);
                }
            }
            else
            {
                _mainBtnClassBuilder.Add(Classes?.BodyBtnDisable);
            }

            return _mainBtnClassBuilder.Value;
        }

        public DateTimeOffset GetDateTimeOfDayCell(int dayIndex, int weekIndex) => _dateTimeManager.GetDateTimeOfDayCell(dayIndex, weekIndex);

        public DateTimeOffset GetDateTimeOfMonthCell(int monthIndex) => new(Culture.Calendar.ToDateTime(_dateTimeManager.SelectedYear, monthIndex, 1, 0, 0, 0, 0), DateTimeOffset.Now.Offset);

        private async Task UpdateValue()
        {
            if (Value.HasValue is false) return;

            var currentValueYear = Culture.Calendar.GetYear(Value.Value.LocalDateTime);
            var currentValueMonth = Culture.Calendar.GetMonth(Value.Value.LocalDateTime);
            var currentValueDay = Culture.Calendar.GetDayOfMonth(Value.Value.LocalDateTime);
            Value = new DateTimeOffset(Culture.Calendar.ToDateTime(currentValueYear, currentValueMonth, currentValueDay, _hour, _minute, 0, 0), DateTimeOffset.Now.Offset);

            //await OnChange.InvokeAsync(Value);
        }

        // private async Task HandleOnTimeHourFocus()
        // {
        //     if (IsDisabled is true || ShowTimePicker is false) return;
        //     //TODO
        //     //await _js.SelectText(_inputTimeHourRef);
        // }

        // private async Task HandleOnTimeMinuteFocus()
        // {
        //     if (IsDisabled is true || ShowTimePicker is false) return;
        //     //TODO
        //     //await _js.SelectText(_inputTimeMinuteRef);
        // }

        // private void ToggleAmPmTime()
        // {
        //     if (IsDisabled is true) return;

        //     _hourView = _hour + (_hour >= 12 ? -12 : 12);
        // }

        // private async Task HandleOnAmClick()
        // {
        //     _hour %= 12;  // "12:-- am" is "00:--" in 24h
        //     await UpdateValue();
        // }

        // private async Task HandleOnPmClick()
        // {
        //     if (_hour <= 12) // "12:-- pm" is "12:--" in 24h
        //     {
        //         _hour += 12;
        //     }

        //     _hour %= 24;
        //     await UpdateValue();
        // }

        // private bool? IsAm()
        // {
        //     if (Value.HasValue is false) return null;

        //     return _hour >= 0 && _hour < 12; // am is 00:00 to 11:59
        // }

        // private async Task HandleOnPointerDown(bool isNext, bool isHour)
        // {
        //     if (IsDisabled is true) return;

        //     await ChangeTime(isNext, isHour);
        //     ResetCts();

        //     var cts = _cancellationTokenSource;
        //     await Task.Run(async () =>
        //     {
        //         await InvokeAsync(async () =>
        //         {
        //             await Task.Delay(INITIAL_STEP_DELAY);
        //             await ContinuousChangeTime(isNext, isHour, cts);
        //         });
        //     }, cts.Token);
        // }

        //private async Task ContinuousChangeTime(bool isNext, bool isHour, CancellationTokenSource cts)
        //{
        //    if (cts.IsCancellationRequested) return;

        //    await ChangeTime(isNext, isHour);

        //    StateHasChanged();

        //    await Task.Delay(STEP_DELAY);
        //    await ContinuousChangeTime(isNext, isHour, cts);
        //}

        private async Task ChangeTime(bool isNext, bool isHour)
        {
            if (isHour)
            {
                await ChangeHour(isNext);
            }
            else
            {
                await ChangeMinute(isNext);
            }
        }

        private async Task ChangeHour(bool isNext)
        {
            if (isNext)
            {
                _hour += HourStep;
            }
            else
            {
                _hour -= HourStep;
            }

            if (_hour > 23)
            {
                _hour -= 24;
            }
            else if (_hour < 0)
            {
                _hour += 24;
            }

            await UpdateValue();
        }

        private async Task ChangeMinute(bool isNext)
        {
            if (isNext)
            {
                _minute += MinuteStep;
            }
            else
            {
                _minute -= MinuteStep;
            }

            if (_minute > 59)
            {
                _minute -= 60;
            }
            else if (_minute < 0)
            {
                _minute += 60;
            }

            await UpdateValue();
        }

        private async Task CloseCallout()
        {
            if (IsDisabled is true) return;

            IsOpen = false;

            StateHasChanged();
        }

        //private bool ShowDayPicker()
        //{
        //    if (ShowTimePicker)
        //    {
        //        if (ShowTimePickerAsOverlay)
        //        {
        //            return _showMonthPickerAsOverlayInternal is false || (_showMonthPickerAsOverlayInternal && _isMonthPickerOverlayOnTop is false && _isTimePickerOverlayOnTop is false);
        //        }
        //        else
        //        {
        //            return (_showMonthPickerAsOverlayInternal is false && _isMonthPickerOverlayOnTop is false) || (_showTimePickerAsOverlayInternal && _isMonthPickerOverlayOnTop is false && _isTimePickerOverlayOnTop is false);
        //        }
        //    }
        //    else
        //    {
        //        return _showMonthPickerAsOverlayInternal is false || (_showMonthPickerAsOverlayInternal && _isMonthPickerOverlayOnTop is false);
        //    }
        //}

        //private bool ShowMonthPicker()
        //{
        //    if (ShowTimePicker)
        //    {
        //        if (ShowTimePickerAsOverlay)
        //        {
        //            return (_showMonthPickerAsOverlayInternal is false || (_showMonthPickerAsOverlayInternal && _isMonthPickerOverlayOnTop)) && _isTimePickerOverlayOnTop is false;
        //        }
        //        else
        //        {
        //            return (_showMonthPickerAsOverlayInternal is false && _isMonthPickerOverlayOnTop) || (_showTimePickerAsOverlayInternal && _isMonthPickerOverlayOnTop && _isTimePickerOverlayOnTop is false);
        //        }
        //    }
        //    else
        //    {
        //        return _showMonthPickerAsOverlayInternal is false || (_showMonthPickerAsOverlayInternal && _isMonthPickerOverlayOnTop);
        //    }
        //}

        //[JSInvokable("CloseCallout")]
        //public void CloseCalloutBeforeAnotherCalloutIsOpened()
        //{
        //    if (IsDisabled is true) return;
        //    if (IsOpenHasBeenSet && IsOpenChanged.HasDelegate is false) return;

        //    IsOpen = false;
        //    StateHasChanged();
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    //if (disposing)
        //    //{
        //    //    _dotnetObj.Dispose();
        //    //    _cancellationTokenSource.Dispose();
        //    //}

        //    //base.Dispose(disposing);
        //}

        public int? DaysOfCurrentMonth(int week, int day) => _dateTimeManager.DaysOfCurrentMonth(week, day);
    }
}
