using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PadDatePicker
{
    public partial class PadDatePickerDialog : PInputBase<DateTimeOffset?>
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
        /// Whether the current item should be button for close.
        /// </summary>
        [Parameter] public bool ShowCloseButton { get; set; }
        /// <summary>
        /// Custom template for the DatePicker's icon.
        /// </summary>
        [Parameter] public RenderFragment? IconTemplate { get; set; }

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

        [Parameter] public EventCallback OnOkButtonClicked { get; set; }

        /// <summary>
        /// Whether the ok button should be shown or not when the DatePicker has a value.
        /// </summary>
        [Parameter] public bool ShowOkButton { get; set; }

        /// <summary>
        /// Text of ok button.
        /// </summary>
        [Parameter] public string OkButtonText { get; set; } = "Ok";

        protected override string? FormatValueAsString(DateTimeOffset? value)
        {
            //return null;
            return value.HasValue
            ? value.Value.ToString(DateFormat ?? Culture.DateTimeFormat.ShortDatePattern, Culture)
                : null;
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

        private bool _isVisible = false;

        private ElementReference _inputRef = default!;

        private void Open() => _isVisible = true;
        private void Close() => _isVisible = false;
        private void Toggle() => _isVisible = !_isVisible;

        private async Task HandleOnOkClicked()
        {
            if (_isVisible is false) return;
            Close();

            if (OnOkButtonClicked.HasDelegate)
                await OnOkButtonClicked.InvokeAsync();
        }

        private void HandleOnChange(ChangeEventArgs args)
        {
            if (_isVisible is false) return;
            if (AllowTextInput is false) return;

            CurrentValueAsString = args.Value?.ToString();
        }
        private void HandleOnFocus(FocusEventArgs args)
        {
            _isVisible = true;
        }
        private void HandleOnFocusOut(FocusEventArgs args)
        {

        }

        private Dictionary<string, object> SetAttributes()
        {
            var attributes = new Dictionary<string, object>();

            if(!AllowTextInput)
            {
                attributes.Add("readonly", "readonly");
            }
            return attributes;
        }
    }
}
