using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace PadDatePicker.Tools
{
    public abstract class PadDatePickerBase : PadComponentBase//PInputBase<DateTimeOffset?>
    {
        [Parameter] public string Id { get; set; }
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
                if (value == null || culture == value) return;

                culture = value;
            }
        }
        private CultureInfo culture = CultureInfo.CurrentUICulture ?? new CultureInfo("en-US");

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


        protected DateTimeOffset? DynamicMaxDate;
        private DateTimeOffset? _maxDate;
        /// <summary>
        /// The maximum date allowed for the DatePicker.
        /// </summary>
        [Parameter] public DateTimeOffset? MaxDate
        {
            get => _maxDate > DynamicMaxDate ? _maxDate : DynamicMaxDate;
            set => _maxDate = value;
        }


        protected DateTimeOffset? DynamicMinDate;
        private DateTimeOffset? _minDate;
        /// <summary>
        /// The minimum date allowed for the DatePicker.
        /// </summary>
        [Parameter] public DateTimeOffset? MinDate
        {
            get => _minDate < DynamicMinDate ? _minDate : DynamicMinDate;
            set => _minDate = value;
        }

        [Parameter] public EventCallback OnClearButtonClicked { get; set; }

        /// <summary>
        /// Whether the clear button should be shown or not when the DatePicker has a value.
        /// </summary>
        [Parameter] public bool ShowClearButton { get; set; }

        /// <summary>
        /// Text of clear button.
        /// </summary>
        [Parameter] public string ClearButtonText { get; set; } = "Clear";

        [Parameter] public EventCallback OnOkButtonClicked { get; set; }

        /// <summary>
        /// Whether the ok button should be shown or not when the DatePicker has a value.
        /// </summary>
        [Parameter] public bool ShowOkButton { get; set; }

        /// <summary>
        /// Text of ok button.
        /// </summary>
        [Parameter] public string OkButtonText { get; set; } = "Ok";

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

        protected DateTimeOffset? StartHighlightDay;
        private DateTimeOffset? _endHighlightDay;
        protected DateTimeOffset? EndHighlightDay
        {
            get => _endHighlightDay != null ? _endHighlightDay  : HoverDay;
            set => _endHighlightDay = value;
        }
        protected DateTimeOffset? HoverDay;


        private DateTimeOffset? _selected;
        [Parameter]
        public DateTimeOffset? Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    if (_selected.HasValue && value.HasValue)
                    {
                        if (_selected.Value.Year != value.Value.Year || _selected.Value.Month != value.Value.Month)
                        {
                            _selected = value;
                            if (_selected.HasValue)
                            {
                                //GenerateCalendarData(_selected.Value.DateTime);
                            }
                            SelectedChanged.InvokeAsync(_selected);
                        }
                    }
                    else
                    {
                        _selected = value;
                        SelectedChanged.InvokeAsync(_selected);
                    }
                }
            }
        }

        [Parameter] public EventCallback<DateTimeOffset?> SelectedChanged { get; set; }

        [Parameter] public EventCallback<DateTimeOffset?> OnMonthChanged { get; set; }
        [Parameter] public EventCallback<DateTimeOffset?> OnYearChanged { get; set; }

        protected override void OnInitialized()
        {
            _dateTimeManager = new DateTimeManager(Culture, MinDate, MaxDate);
        }

        protected abstract DateTimeOffset GetDefaultDateTime();
        protected override void OnParametersSet()
        {
            var dateTime = GetDefaultDateTime();

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

            _hour = dateTime.Hour;
            _minute = dateTime.Hour;

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

        protected int _hour;
        protected int _hourView
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

        protected int _minute;
        protected int _minuteView
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

        protected DateTimeManager _dateTimeManager;
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
            Selected = Culture.Calendar.ToDateTime(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth, _dateTimeManager.SelectedDay, _hour, _minute, 0, 0);

            _monthTitle = $"{Culture.DateTimeFormat.GetMonthName(month)} {year}";
            //_monthTitle = $"{Culture.DateTimeFormat.GetMonthName(_dateTimeManager.SelectedMonth)} {_dateTimeManager.SelectedYear}";
            _dateTimeManager.GenerateMonthData(Selected, year, month);
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

        protected abstract void ClearValue();
        protected async Task ClearButtonClickd()
        {
            ClearValue();

            _hour = 0;
            _minute = 0;

            _selectedDateWeek = null;
            _selectedDateDayOfWeek = null;

            GenerateCalendarData(DateTime.Now);

            await OnClearButtonClicked.InvokeAsync();

            StateHasChanged();
        }

        protected async Task OkButtonClicked()
        {
            await OnOkButtonClicked.InvokeAsync();
        }

        public abstract void SetValue(DateTime val);
        public async Task SelectDate(int dayIndex, int weekIndex)
        {
            _dateTimeManager.SelectDate(dayIndex, weekIndex);

            var currentDateTime = Culture.Calendar.ToDateTime(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth, _dateTimeManager.SelectedDay, _hour, _minute, 0, 0);
            SetValue(currentDateTime);

            GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);

            StateHasChanged();
        }

        public async Task SelectMonth(int month)
        {
            if (IsMonthOutOfMinAndMaxDate(month)) return;

            SetSelectedMonth(month);

            GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);

            _headerViewType = PadDatePickerViewType.Month;
            _bodyViewType = PadDatePickerViewType.Day;

            Selected = Culture.Calendar.ToDateTime(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth, _dateTimeManager.SelectedDay, _hour, _minute, 0, 0);
            OnMonthChanged.InvokeAsync(Selected);

            StateHasChanged();
        }

        public async Task SelectYear(int year)
        {
            if (IsYearOutOfMinAndMaxDate(year)) return;

            SetSelectedYear(year);

            ChangeYearRanges(_dateTimeManager.SelectedYear - 1);

            GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);

            _headerViewType = PadDatePickerViewType.Year;
            _bodyViewType = PadDatePickerViewType.Month;

            Selected = Culture.Calendar.ToDateTime(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth, _dateTimeManager.SelectedDay, _hour, _minute, 0, 0);
            OnYearChanged.InvokeAsync(Selected);

            StateHasChanged();
        }

        public void HandleYearChange(bool isNext)
        {
            if (CanChangeYear(isNext) is false) return;

            if (isNext) SetSelectedYear(_dateTimeManager.SelectedYear + 1);
            else SetSelectedYear(_dateTimeManager.SelectedYear - 1);

            GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);

            StateHasChanged();
        }

        public void HandleMonthChange(bool isNext)
        {
            if (CanChangeMonth(isNext) is false) return;

            if (isNext)
            {
                if (_dateTimeManager.SelectedMonth < 12)
                {
                    SetSelectedMonth(_dateTimeManager.SelectedMonth + 1);
                }
                else
                {
                    SetSelectedYear(_dateTimeManager.SelectedYear + 1);
                    SetSelectedMonth(1);
                }
            }
            else
            {
                if (_dateTimeManager.SelectedMonth > 1)
                {
                    SetSelectedMonth(_dateTimeManager.SelectedMonth - 1);
                }
                else
                {
                    SetSelectedYear(_dateTimeManager.SelectedYear - 1);
                    SetSelectedMonth(12);
                }
            }

            SetSelectedDay(1);

            Selected = Culture.Calendar.ToDateTime(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth, _dateTimeManager.SelectedDay, _hour, _minute, 0, 0);
            OnMonthChanged.InvokeAsync(Selected);

            GenerateMonthData(_dateTimeManager.SelectedYear, _dateTimeManager.SelectedMonth);

            //_monthTitle = $"{Culture.DateTimeFormat.GetMonthName(_dateTimeManager.SelectedMonth)} {_dateTimeManager.SelectedYear}";

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
            Selected = DateTime.Now;
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

        public void SetSelectedYear(int year)
        {
            _dateTimeManager.SetSelectedYear(year);
        }
        public void SetSelectedMonth(int month)
        {
            _dateTimeManager.SetSelectedMonth(month);
        }
        public void SetSelectedDay(int day)
        {
            _dateTimeManager.SetSelectedDay(day);
        }
        public void SetSelectedHour(int hour)
        {
            _dateTimeManager.SetSelectedHour(hour);
        }
        public void SetSelectedMinute(int minute)
        {
            _dateTimeManager.SetSelectedMinute(minute);
        }

        private void GenerateCalendarData(DateTime dateTime)
        {
            SetSelectedYear(Culture.Calendar.GetYear(dateTime));
            SetSelectedMonth(Culture.Calendar.GetMonth(dateTime));
            SetSelectedDay(Culture.Calendar.GetDayOfMonth(dateTime));

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
        public bool IsDayInRange(int week, int day) => _dateTimeManager.IsDayInRange(StartHighlightDay, EndHighlightDay, week, day);
        public abstract bool IsSelectedDay(int week, int day);
        public bool IsWeekDayOutOfMinAndMaxDate(int dayIndex, int weekIndex) => _dateTimeManager.IsWeekDayOutOfMinAndMaxDate(dayIndex, weekIndex);
        public bool IsInCurrentMonth(int week, int day) => _dateTimeManager.IsInCurrentMonth(week, day);
        public DayOfWeek GetDayOfWeek(int index) => _dateTimeManager.GetDayOfWeek(index);

        public void MouseOver(int week, int day)
        {
            HoverDay = _dateTimeManager.GetDateTime(week, day);
            StateHasChanged();
        }
        public void MouseLeave(int week, int day)
        {
            if (HoverDay == _dateTimeManager.GetDateTime(week, day))
            {
                HoverDay = null;
            }
            StateHasChanged();
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
                    isNext && MaxDate.HasValue && Culture.Calendar.GetYear(MaxDate.Value.DateTime) == _dateTimeManager.SelectedYear ||
                    isNext is false && MinDate.HasValue && Culture.Calendar.GetYear(MinDate.Value.DateTime) == _dateTimeManager.SelectedYear
                   ) is false;
        }

        private bool CanChangeYearRange(bool isNext)
        {
            return (
                    isNext && MaxDate.HasValue && Culture.Calendar.GetYear(MaxDate.Value.DateTime) < _yearPickerStartYear + 12 ||
                    isNext is false && MinDate.HasValue && Culture.Calendar.GetYear(MinDate.Value.DateTime) >= _yearPickerStartYear
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

        public string GetDayButtonCss(bool isEnable, bool isCurrent, bool isSelected, bool isInRange)
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
                else if (isCurrent && HighlightCurrent)
                {
                    _mainBtnClassBuilder.Add(Classes?.TodayDayButton);
                }
                else if (isInRange)
                {
                    _mainBtnClassBuilder.Add(Classes?.InRangeDayButton);
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

        protected abstract void DoUpdateValue();
        private async Task UpdateValue()
        {
            DoUpdateValue();
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
