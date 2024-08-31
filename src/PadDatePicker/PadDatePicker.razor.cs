using PadDatePicker.Tools;

namespace PadDatePicker
{
    public partial class PadDatePicker : PadDatePickerGeneric<DateTimeOffset?>
    {
        public override bool IsSelectedDay(int week, int day) => _dateTimeManager.IsSelectedDay(Value, week, day);

        protected override DateTimeOffset GetDefaultDateTime() => Value.GetValueOrDefault(DateTimeOffset.Now);
        protected override void ClearValue() => Value = null;
        public override void SetValue(DateTime val)
        {
            Value = new DateTimeOffset(val, DateTimeOffset.Now.Offset);
            StateHasChanged();
        }

        protected override void DoUpdateValue()
        {
            if (Value.HasValue is false) return;

            var currentValueYear = Culture.Calendar.GetYear(Value.Value.LocalDateTime);
            var currentValueMonth = Culture.Calendar.GetMonth(Value.Value.LocalDateTime);
            var currentValueDay = Culture.Calendar.GetDayOfMonth(Value.Value.LocalDateTime);
            Value = new DateTimeOffset(Culture.Calendar.ToDateTime(currentValueYear, currentValueMonth, currentValueDay, _hour, _minute, 0, 0), DateTimeOffset.Now.Offset);
        }       
    }
}
