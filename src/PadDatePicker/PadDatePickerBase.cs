using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace PadDatePicker
{
    public class PadDatePickerBase : PadComponentBase//PInputBase<DateTimeOffset?>
    {
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

        /// <summary>
        /// The time format of the time-picker, 24H or 12H.
        /// </summary>
        [Parameter] public TimeFormat TimeFormat { get; set; }

        /// <summary>
        /// Custom template to render the header of the DatePicker.
        /// </summary>
        [Parameter] public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// Custom template to render the footer of the DatePicker.
        /// </summary>
        [Parameter] public RenderFragment FooterTemplate { get; set; }

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
        /// The maximum date allowed for the DatePicker.
        /// </summary>
        [Parameter] public DateTimeOffset? MaxDate { get; set; }

        /// <summary>
        /// The minimum date allowed for the DatePicker.
        /// </summary>
        [Parameter] public DateTimeOffset? MinDate { get; set; }

        [Parameter] public EventCallback OnClearButtonClicked { get; set; }

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
        [Parameter] public bool ShowToDayButton { get; set; }

        [Parameter] public EventCallback OnToDayButtonClicked { get; set; }

        /// <summary>
        /// Text of go today button.
        /// </summary>
        [Parameter] public string ToDayButtonText { get; set; } = "Today";

        /// <summary>
        /// Whether the current item should be highlight or not when the.
        /// </summary>
        [Parameter] public bool HighlightCurrent { get; set; }

        /// <summary>
        /// Determines increment/decrement steps for date-picker's hour.
        /// </summary>
        [Parameter] public int HourStep { get; set; }

        /// <summary>
        /// Determines increment/decrement steps for date-picker's minute.
        /// </summary>
        [Parameter] public int MinuteStep { get; set; }

        /// <summary>
        /// The text of the DatePicker's label.
        /// </summary>
        [Parameter] public string? Label { get; set; }

        /// <summary>
        /// Whether the show label on header.
        /// </summary>
        [Parameter] public bool ShowLabelOnHeader { get; set; }

        private DateTimeOffset? _value;
        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public DateTimeOffset? Value 
        { 
            get => _value; 
            set {
                if(_value != value)
                {
                    _value = value;
                    ValueChanged.InvokeAsync(_value);
                }
            } 
        }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter] public EventCallback<DateTimeOffset?> ValueChanged { get; set; }

        //protected override void BuildRenderTree(RenderTreeBuilder builder)
        //{
        //    builder.OpenComponent<CascadingValue<PadDatePicker>>(1);
        //        builder.AddAttribute(2, "Name", "DatePicker");
        //        builder.AddAttribute(3, "Value", this);
        //            builder.OpenElement(4, "div");
        //            builder.AddAttribute(5, "class", Classes?.PickerContainer);
        //            if(HeaderTemplate != null)
        //            {
        //                builder.AddContent(6, HeaderTemplate);
        //            }
        //            builder.OpenElement(7, "div");
        //                builder.OpenElement(8, "div");
        //                builder.AddAttribute(9, Classes?.PickerHeaderLabel);
        //                builder.AddContent(10, Label);
        //                builder.CloseElement();
        //                builder.OpenComponent(11, typeof(PadDatePickerHeader));
        //                builder.CloseComponent();
        //            builder.CloseElement();
        //            builder.OpenElement(12, "div");
        //                builder.AddAttribute(13, "class", "p-1");
        //                builder.OpenComponent(14, typeof(PadDatePickerBody));
        //                builder.CloseComponent();
        //            builder.CloseElement();
        //            if (_bodyViewType.Equals(PadDatePickerViewType.Day) && (ShowToDayButton || ShowClearButton))
        //            {
        //                builder.OpenElement(15, "div");
        //                    builder.OpenElement(16, "div");
        //                    builder.AddAttribute(17, "class", Classes?.FooterWrapper);
        //                    if (ShowToDayButton)
        //                    {
        //                        builder.OpenElement(18, "button");
        //                        builder.AddAttribute(19, "@onclick", EventCallback.Factory.Create(this, GoToToday));
        //                        builder.AddAttribute(20, "type", "button");
        //                        builder.AddAttribute(21, "class", Classes?.GoToTodayButton);
        //                        builder.AddContent(22, @ToDayButtonText);
        //                        builder.CloseElement();
        //                    }
        //                    if (ShowClearButton)
        //                    {
        //                        builder.OpenElement(18, "button");
        //                        builder.AddAttribute(19, "@onclick", EventCallback.Factory.Create(this, ClearButtonClick));
        //                        builder.AddAttribute(20, "type", "button");
        //                        builder.AddAttribute(21, "class", Classes?.ClearButton);
        //                        builder.AddContent(22, ClearButtonText);
        //                        builder.CloseElement();
        //                    }
        //                    builder.CloseElement();
        //                builder.CloseElement();
        //            }
        //            if (FooterTemplate != null)
        //            {
        //                builder.AddContent(6, FooterTemplate);
        //            }
        //    builder.CloseElement();
        //    builder.CloseComponent();
        //}

        protected override void OnInitialized()
        {
            _dateTimeManager = new DateTimeManager(Culture, MinDate, MaxDate);
        }
        protected override void OnParametersSet()
        {
            var dateTime = Value.GetValueOrDefault(DateTimeOffset.Now);

            _dateTimeManager.Culture = Culture;
            _dateTimeManager.MinDate = MinDate;
            _dateTimeManager.MaxDate = MaxDate;

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
        private int? _selectedDateWeek;
        private int _yearPickerEndYear;
        private int _yearPickerStartYear;
        private int? _selectedDateDayOfWeek;
        private string _monthTitle = string.Empty;

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

        protected async Task ClearButtonClick()
        {
            Value = null;

            _hour = 0;
            _minute = 0;

            _selectedDateWeek = null;
            _selectedDateDayOfWeek = null;

            GenerateCalendarData(DateTime.Now);

            await OnClearButtonClicked.InvokeAsync();

            StateHasChanged();
        }

        public async Task SelectDate(int dayIndex, int weekIndex)
        {
            _dateTimeManager.SelectDate(dayIndex, weekIndex);

            var currentDateTime = Culture.Calendar.ToDateTime(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth, _dateTimeManager.SelectedDay, _hour, _minute, 0, 0);
            Value = new DateTimeOffset(currentDateTime, DateTimeOffset.Now.Offset);

            GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);

            StateHasChanged();
        }

        public async Task SelectMonth(int month)
        {
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
            if (CanChangeMonth(isNext) is false) return;

            _dateTimeManager.HandleMonthChange(isNext, Value);

            _monthTitle = $"{Culture.DateTimeFormat.GetMonthName(_dateTimeManager.SelectedMonth)} {_dateTimeManager.SelectedYear}";

            StateHasChanged();
        }

        public void HandleYearChange(bool isNext)
        {
            if (CanChangeYear(isNext) is false) return;

            _dateTimeManager.HandleYearChange(isNext, Value);

            StateHasChanged();
        }

        public void HandleYearRangeChange(bool isNext)
        {
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

        protected async Task GoToToday()
        {
            Value = DateTime.Now;
            GenerateCalendarData(DateTime.Now);

            await OnToDayButtonClicked.InvokeAsync();

            StateHasChanged();
        }

        protected async Task GoToNow()
        {
            _hour = DateTime.Now.Hour;
            _minute = DateTime.Now.Minute;

            await UpdateValue();
        }

        private void GenerateCalendarData(DateTime dateTime)
        {
            _dateTimeManager.SelectedYear = Culture.Calendar.GetYear(dateTime);
            _dateTimeManager.SelectedMonth = Culture.Calendar.GetMonth(dateTime);
            //_dateTimeManager.SelectedDay = Culture.Calendar.GetDayOfMonth(dateTime);

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

        //private void CheckCurrentCalendarMatchesValue()
        //{
        //    var currentValue = Value.GetValueOrDefault(DateTimeOffset.Now);
        //    var currentValueYear = Culture.Calendar.GetYear(currentValue.DateTime);
        //    var currentValueMonth = Culture.Calendar.GetMonth(currentValue.DateTime);
        //    var currentValueDay = Culture.Calendar.GetDayOfMonth(currentValue.DateTime);

        //    if (currentValueYear != _dateTimeManager.SelectedYear || currentValueMonth != _dateTimeManager.SelectedMonth || (AllowTextInput && currentValueDay != _dateTimeManager.SelectedDay))
        //    {
        //        _dateTimeManager.SelectedYear = currentValueYear;
        //        _dateTimeManager.SelectedMonth = currentValueMonth;
        //        GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);
        //    }
        //}

        public string GetDayButtonCss(bool isEnable, bool isCurrent, bool isSelected)
        {
            ElementClassBuilder _mainBtnClassBuilder = new ElementClassBuilder();
            _mainBtnClassBuilder.Add(Classes?.DayButton);

            if (isEnable)
            {
                _mainBtnClassBuilder.Add(Classes?.EnableDayButton);
                if (isSelected)
                {
                    _mainBtnClassBuilder.Add(Classes?.SelectedDayButton);
                }
                else if(isCurrent && HighlightCurrent)
                {
                    _mainBtnClassBuilder.Add(Classes?.TodayDayButton);
                }
                else
                {
                    _mainBtnClassBuilder.Add(Classes?.NormalDayButton);
                }
            }
            else
            {
                _mainBtnClassBuilder.Add(Classes?.DisableDayButton);
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
        }

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

        public int? DaysOfCurrentMonth(int week, int day) => _dateTimeManager.DaysOfCurrentMonth(week, day);
    }
}
