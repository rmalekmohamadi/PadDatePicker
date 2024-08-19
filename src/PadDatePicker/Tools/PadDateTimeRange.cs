using System;
using System.Diagnostics.CodeAnalysis;

namespace PadDatePicker
{
    public struct PadDateTimeRange
    {
        public PadDateTimeRange(DateTimeOffset? start, DateTimeOffset? end)
        {
            Start = start;
            End = end;
        }

        public DateTimeOffset? Start { get; set; }
        public DateTimeOffset? End { get; set; }

        public static bool operator ==(PadDateTimeRange first, PadDateTimeRange second)
        {
            return Equals(first, second);
        }
        public static bool operator !=(PadDateTimeRange first, PadDateTimeRange second)
        {
            return !(first == second);
        }

        public bool Equals(PadDateTimeRange range) => range.Start == Start && range.End == End;

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }

        public override string ToString() => $"{Start}-{End}";
    }
}
