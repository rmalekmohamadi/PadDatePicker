using PadDatePicker.Tools;

namespace PadDatePicker
{
    public partial class PadDateRangePicker : PadDatePickerGeneric<PadDateTimeRange>
    {
        public override bool IsSelectedDay(int week, int day) => _dateTimeManager.IsSelectedDay(Value, week, day);

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

        protected override DateTimeOffset GetDefaultDateTime() => Selected.GetValueOrDefault(Value.Start.GetValueOrDefault(DateTimeOffset.Now));
        protected override void ClearValue()
        {
            _start = null;
            _end = null;
            StartHighlightDay = _start;
            EndHighlightDay = _end;
        }

        public override void SetValue(DateTime val)
        {
            if(_start == null || _start > val)
            {
                _start = new DateTimeOffset(val, DateTimeOffset.Now.Offset);
                _end = null;
            }
            else if(_end == null)
            {
                _end = new DateTimeOffset(val, DateTimeOffset.Now.Offset);
            }
            else
            {
                _start = new DateTimeOffset(val, DateTimeOffset.Now.Offset);
                _end = null;
            }
            StartHighlightDay = _start;
            EndHighlightDay = _end;
        }

        protected override void DoUpdateValue()
        {
            if (Value.Start.HasValue is true)
            {
                var currentStartValueYear = Culture.Calendar.GetYear(Value.Start.Value.LocalDateTime);
                var currentStartValueMonth = Culture.Calendar.GetMonth(Value.Start.Value.LocalDateTime);
                var currentStartValueDay = Culture.Calendar.GetDayOfMonth(Value.Start.Value.LocalDateTime);
                _start = new DateTimeOffset(Culture.Calendar.ToDateTime(currentStartValueYear, currentStartValueMonth, currentStartValueDay, _hour, _minute, 0, 0), DateTimeOffset.Now.Offset);
            }

            if (Value.End.HasValue is true)
            {
                var currentEndValueYear = Culture.Calendar.GetYear(Value.End.Value.LocalDateTime);
                var currentEndValueMonth = Culture.Calendar.GetMonth(Value.End.Value.LocalDateTime);
                var currentEndValueDay = Culture.Calendar.GetDayOfMonth(Value.End.Value.LocalDateTime);
                _end = new DateTimeOffset(Culture.Calendar.ToDateTime(currentEndValueYear, currentEndValueMonth, currentEndValueDay, _hour, _minute, 0, 0), DateTimeOffset.Now.Offset);
            }

            StartHighlightDay = _start;
            EndHighlightDay = _end;
        }
    }
}
