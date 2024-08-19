using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PadDatePicker
{
    public partial class PadDateRangePickerDialog : PadComponentBase
    {
        #region PadDatePickerBase Parameters
        /// <summary>
        /// Custom CSS classes for different parts of the DatePicker component.
        /// </summary>
        [Parameter] public DatePickerClassStyles? Classes { get; set; } = new DatePickerClassStyles();

        /// <summary>
        /// CultureInfo for the DatePicker.
        /// </summary>
        [Parameter]
        public CultureInfo Culture { get; set; }

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
        /// Custom template for the DatePicker's icon.
        /// </summary>
        [Parameter] public RenderFragment? IconTemplate { get; set; }

        /// <summary>
        /// Whether the current item should be button for close.
        /// </summary>
        [Parameter] public bool ShowCloseButton { get; set; }

        /// <summary>
        /// Determines the location of the DatePicker's icon.
        /// </summary>
        [Parameter]
        public Side IconSide { get; set; }

        // [Parameter] public PSize Size { get; set; } = PSize.Base;
        /// <summary>
        /// Whether or not the DatePicker allows a string date input.
        /// </summary>
        [Parameter] public bool AllowTextInput { get; set; } = true;

        /// <summary>
        /// Custom label for the start DatePicker.
        /// </summary>
        [Parameter] public string StartRangeLabel { get; set; }

        /// <summary>
        /// Custom label for the start DatePicker.
        /// </summary>
        [Parameter] public string EndRangeLabel { get; set; }

        /// <summary>
        /// Whether or not the component is disabled.
        /// </summary>
        [Parameter]
        public bool IsDisabled
        {
            get => isDisabled;
            set
            {
                if (isDisabled == value) return;

                isDisabled = value;
            }
        }
        protected bool isDisabled;

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
                if (value == null || EqualityComparer<DateTimeOffset?>.Default.Equals(value, _value.Start)) return;

                _value.Start = value;
                _ = ValueChanged.InvokeAsync(_value);
            }
        }
        private DateTimeOffset? _end
        {
            get => _value.End;
            set
            {
                if (value == null || EqualityComparer<DateTimeOffset?>.Default.Equals(value, _value.End)) return;

                _value.End = value;
                _ = ValueChanged.InvokeAsync(_value);
            }
        }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter] public EventCallback<PadDateTimeRange> ValueChanged { get; set; }

        /// <summary>
        /// Gets or sets an expression that identifies the bound value.
        /// </summary>
        // [Parameter] public Expression<Func<PadDateTimeRange>>? ValueExpression { get; set; }

        /// <summary>
        /// The placeholder text of the DatePicker's input.
        /// </summary>
        [Parameter] public string StartPlaceholder { get; set; } = string.Empty;
        [Parameter] public string EndPlaceholder { get; set; } = string.Empty;

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

        private ElementReference _startInputRef = default!;
        private ElementReference _endInputRef = default!;

        private void Open() => _isVisible = true;
        private void Close() => _isVisible = false;
        private void Toggle() => _isVisible = !_isVisible;

        private void HandleOnStartChange(ChangeEventArgs args)
        {
            if (_isVisible is false) return;
            if (AllowTextInput is false) return;

            StartCurrentValueAsString = args.Value?.ToString();
        }

        private void HandleOnEndChange(ChangeEventArgs args)
        {
            if (_isVisible is false) return;
            if (AllowTextInput is false) return;

            StartCurrentValueAsString = args.Value?.ToString();
        }

        private void HandleOnStartFocus(FocusEventArgs args)
        {
            _isVisible = true;
        }

        private void HandleOnEndFocus(FocusEventArgs args)
        {
            _isVisible = true;
        }

        private void HandleOnStartFocusOut(FocusEventArgs args)
        {

        }

        private void HandleOnEndFocusOut(FocusEventArgs args)
        {

        }

        private Dictionary<string, object> SetAttributes()
        {
            var attributes = new Dictionary<string, object>();

            if (!AllowTextInput)
            {
                attributes.Add("readonly", "readonly");
            }
            return attributes;
        }
    }
}
