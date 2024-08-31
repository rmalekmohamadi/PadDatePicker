using Microsoft.AspNetCore.Components;

namespace PadDatePicker.Tools
{
    public abstract class PadDatePickerGeneric<TValue> : PadDatePickerBase
    {
        protected TValue? _value;
        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public TValue? Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    ValueChanged.InvokeAsync(_value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }
    }
}
