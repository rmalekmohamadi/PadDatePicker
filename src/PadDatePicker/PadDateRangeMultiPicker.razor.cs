using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace PadDatePicker
{
    public partial class PadDateRangeMultiPicker : PadComponentBase
    {
        #region PadDatePickerBase Parameters
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

        /// <summary>
        /// The maximum date allowed for the DatePicker.
        /// </summary>
        [Parameter] public DateTimeOffset? MaxDate { get; set; }

        /// <summary>
        /// The minimum date allowed for the DatePicker.
        /// </summary>
        [Parameter] public DateTimeOffset? MinDate { get; set; }


        [Parameter] public EventCallback OnOkButtonClicked { get; set; }

        /// <summary>
        /// Whether the ok button should be shown or not when the DatePicker has a value.
        /// </summary>
        [Parameter] public bool ShowOkButton { get; set; }

        /// <summary>
        /// Text of ok button.
        /// </summary>
        [Parameter] public string OkButtonText { get; set; } = "Ok";

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
        #endregion

        /// <summary>
        /// The format of the date in the DatePicker.
        /// </summary>
        [Parameter] public string? DateFormat { get; set; }

        /// <summary>
        /// The custom validation error message for the invalid value.
        /// </summary>
        [Parameter] public string? InvalidErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public PadDateTimeRange Value
        {
            get => _value;
            set
            {
                if (value == null || EqualityComparer<PadDateTimeRange>.Default.Equals(value, Value)) return;

                _value = value;
                _ = ValueChanged.InvokeAsync(value);
            }
        }
        private PadDateTimeRange _value = new PadDateTimeRange();

        private DateTimeOffset? _start
        {
            get => _value.Start;
            set
            {
                if (EqualityComparer<DateTimeOffset?>.Default.Equals(value, _value.Start)) return;

                _value.Start = value;
                _ = ValueChanged.InvokeAsync(_value);
            }
        }
        private DateTimeOffset? _end
        {
            get => _value.End;
            set
            {
                if (EqualityComparer<DateTimeOffset?>.Default.Equals(value, _value.End)) return;

                _value.End = value;
                _ = ValueChanged.InvokeAsync(_value);
            }
        }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter] public EventCallback<PadDateTimeRange> ValueChanged { get; set; }

        protected string? StartCurrentValueAsString
        {
            get => FormatValueAsString(Value.Start);
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Value = new PadDateTimeRange(default, Value.End);
                }
                else if (TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage))
                {
                    Value = new PadDateTimeRange(parsedValue!, Value.End);
                }
                else
                {
                    Value = new PadDateTimeRange(default, Value.End);
                }
            }
        }

        protected string? EndCurrentValueAsString
        {
            get => FormatValueAsString(Value.End);
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Value = new PadDateTimeRange(Value.Start, default);
                }
                else if (TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage))
                {
                    Value = new PadDateTimeRange(Value.Start, parsedValue!);
                }
                else
                {
                    Value = new PadDateTimeRange(Value.Start, default);
                }
            }
        }


        protected string? FormatValueAsString(DateTimeOffset? value)
        {
            //return null;
            return value.HasValue
            ? value.Value.ToString(DateFormat ?? Culture.DateTimeFormat.ShortDatePattern, Culture)
                : null;
        }

        protected bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateTimeOffset? result, [NotNullWhen(false)] out string? validationErrorMessage)
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

        private bool _isVisible = false;

        private async Task HandleOnClearClicked()
        {
            if (_isVisible is false) return;
            _start = null;
            _end = null;

            if (OnClearButtonClicked.HasDelegate)
                await OnClearButtonClicked.InvokeAsync();
        }

        private async Task HandleOnToDayClicked()
        {
            if (_isVisible is false) return;
            _start = _end;

            if (OnToDayButtonClicked.HasDelegate)
                await OnToDayButtonClicked.InvokeAsync();
        }


        //protected override void OnAfterRender(bool firstRender)
        //{
        //    base.OnAfterRender(firstRender);
        //    SelectedStartChanged(DateTimeOffset.Now);
        //}
        private bool _startInit = false;
        private DateTimeOffset? _selectedStart;
        private void SelectedStartInitialized(DateTimeOffset? selected)
        {
            Console.WriteLine("Initialized {0}", selected);
            if(!_startInit)
            {
                _startInit = true;
                SelectedStartChanged(selected);
            }
        }
        private void SelectedStartChanged(DateTimeOffset? selected)
        {
            _selectedStart = selected;
            _selectedEnd = selected.Value.AddMonths(1);
        }
        private DateTimeOffset? _selectedEnd;
        private void SelectedEndChanged(DateTimeOffset? selected)
        {

            _selectedEnd = selected;
            _selectedStart = selected.Value.AddMonths(-1);
        }
    }
}