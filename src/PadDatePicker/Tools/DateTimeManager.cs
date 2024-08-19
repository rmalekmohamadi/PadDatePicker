﻿using Microsoft.AspNetCore.Components;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PadDatePicker
{
    public class DateTimeManager
    {
        private const int DEFAULT_DAY_COUNT_PER_WEEK = 7;
        private const int DEFAULT_WEEK_COUNT = 6;

        public DateTimeManager(CultureInfo culture, DateTimeOffset? minDate, DateTimeOffset? maxDate)
        {
            Culture = culture;
            CurrentYear = Culture.Calendar.GetYear(DateTime.Now);
            CurrentMonth = Culture.Calendar.GetMonth(DateTime.Now);
            CurrentDay = Culture.Calendar.GetDayOfMonth(DateTime.Now);
            MinDate = minDate;
            MaxDate = maxDate;
        }

        public CultureInfo Culture { get; set; }
        public DateTimeOffset? MaxDate { get; set; }
        public DateTimeOffset? MinDate { get; set; }

        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }
        public int SelectedDay { get; set; }
        public int SelectedHour { get; set; }
        public int SelectedMinute { get; set; }

        public int CurrentYear { get; set; }
        public int CurrentMonth { get; set; }
        public int CurrentDay { get; set; }

        private readonly int[,] _daysOfCurrentMonth = new int[DEFAULT_WEEK_COUNT, DEFAULT_DAY_COUNT_PER_WEEK];

        public bool IsToDay(int week, int day) => CurrentYear == SelectedYear && CurrentMonth == SelectedMonth && CurrentDay == DaysOfCurrentMonth(week, day);
        public bool IsSelectedDay(int week, int day) => SelectedDay == DaysOfCurrentMonth(week, day);
        public bool IsToMonth(int index) => CurrentYear == SelectedYear && CurrentMonth == index;
        public bool IsSelectedMonth(int index) => SelectedMonth == index;
        public bool IsToYear(int year) => CurrentYear == year;
        public bool IsSelectedYear(int year) => SelectedYear == year;

        public int? DaysOfCurrentMonth(int week, int day)
        {
            try
            {
                return _daysOfCurrentMonth[week, day];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private int? _selectedDateWeek = null;
        private int? _selectedDateDayOfWeek = null;

        public void HandleYearChange(bool isNext, DateTimeOffset? val)
        {
            SelectedYear += isNext ? +1 : -1;

            GenerateMonthData(val, SelectedYear, SelectedMonth);
        }

        public void HandleMonthChange(bool isNext, DateTimeOffset? val)
        {
            if (isNext)
            {
                if (SelectedMonth < 12)
                {
                    SelectedMonth++;
                }
                else
                {
                    SelectedYear++;
                    SelectedMonth = 1;
                }
            }
            else
            {
                if (SelectedMonth > 1)
                {
                    SelectedMonth--;
                }
                else
                {
                    SelectedYear--;
                    SelectedMonth = 12;
                }
            }

            GenerateMonthData(val, SelectedYear, SelectedMonth);
        }

        public void GenerateMonthData(DateTimeOffset? val, int year, int month)
        {
            _selectedDateWeek = null;
            _selectedDateDayOfWeek = null;

            for (int weekIndex = 0; weekIndex < DEFAULT_WEEK_COUNT; weekIndex++)
            {
                for (int dayIndex = 0; dayIndex < DEFAULT_DAY_COUNT_PER_WEEK; dayIndex++)
                {
                    _daysOfCurrentMonth[weekIndex, dayIndex] = 0;
                }
            }

            var monthDays = Culture.Calendar.GetDaysInMonth(year, month);
            var firstDayOfMonth = Culture.Calendar.ToDateTime(year, month, 1, 0, 0, 0, 0);
            var startWeekDay = (int)Culture.DateTimeFormat.FirstDayOfWeek;
            var weekDayOfFirstDay = (int)firstDayOfMonth.DayOfWeek;
            var correctedWeekDayOfFirstDay = weekDayOfFirstDay > startWeekDay ? startWeekDay : startWeekDay - 7;

            var currentDay = 1;
            var isCurrentMonthEnded = false;
            for (int weekIndex = 0; weekIndex < DEFAULT_WEEK_COUNT; weekIndex++)
            {
                for (int dayIndex = 0; dayIndex < DEFAULT_DAY_COUNT_PER_WEEK; dayIndex++)
                {
                    if (weekIndex == 0 && currentDay == 1 && weekDayOfFirstDay > dayIndex + correctedWeekDayOfFirstDay)
                    {
                        int prevMonth;
                        int prevMonthDays;
                        if (month > 1)
                        {
                            prevMonth = month - 1;
                            prevMonthDays = Culture.Calendar.GetDaysInMonth(year, prevMonth);
                        }
                        else
                        {
                            prevMonth = 12;
                            prevMonthDays = Culture.Calendar.GetDaysInMonth(year - 1, prevMonth);
                        }

                        if (weekDayOfFirstDay > startWeekDay)
                        {
                            _daysOfCurrentMonth[weekIndex, dayIndex] = prevMonthDays + dayIndex - (weekDayOfFirstDay - startWeekDay - 1);
                        }
                        else
                        {
                            _daysOfCurrentMonth[weekIndex, dayIndex] = prevMonthDays + dayIndex - (7 + weekDayOfFirstDay - startWeekDay - 1);
                        }
                    }
                    else if (currentDay <= monthDays)
                    {
                        _daysOfCurrentMonth[weekIndex, dayIndex] = currentDay;
                        currentDay++;
                    }

                    if (currentDay > monthDays)
                    {
                        currentDay = 1;
                        isCurrentMonthEnded = true;
                    }
                }

                if (isCurrentMonthEnded)
                {
                    break;
                }
            }
            SetSelectedDateWeek(val);
        }

        private void SetSelectedDateWeek(DateTimeOffset? val)
        {
            if (Culture is null) return;
            if (val.HasValue is false || (_selectedDateWeek.HasValue && _selectedDateDayOfWeek.HasValue)) return;

            var year = Culture.Calendar.GetYear(val.Value.DateTime);
            var month = Culture.Calendar.GetMonth(val.Value.DateTime);

            if (year == SelectedYear && month == SelectedMonth)
            {
                var dayOfMonth = Culture.Calendar.GetDayOfMonth(val.Value.DateTime);
                var startWeekDay = (int)Culture.DateTimeFormat.FirstDayOfWeek;
                var weekDayOfFirstDay = (int)Culture.Calendar.ToDateTime(year, month, 1, 0, 0, 0, 0).DayOfWeek;
                var indexOfWeekDayOfFirstDay = (weekDayOfFirstDay - startWeekDay + DEFAULT_DAY_COUNT_PER_WEEK) % DEFAULT_DAY_COUNT_PER_WEEK;

                _selectedDateDayOfWeek = ((int)val.Value.DayOfWeek - startWeekDay + DEFAULT_DAY_COUNT_PER_WEEK) % DEFAULT_DAY_COUNT_PER_WEEK;

                var days = indexOfWeekDayOfFirstDay + dayOfMonth;

                _selectedDateWeek = days % DEFAULT_DAY_COUNT_PER_WEEK == 0 ? (days / DEFAULT_DAY_COUNT_PER_WEEK) - 1 : days / DEFAULT_DAY_COUNT_PER_WEEK;

                if (indexOfWeekDayOfFirstDay is 0)
                {
                    _selectedDateWeek++;
                }
            }
        }

        private int GetWeekNumber(int weekIndex)
        {
            int year = SelectedYear;
            int month = FindMonth(weekIndex, 0);

            if (IsInCurrentMonth(weekIndex, 0) is false)
            {
                if (SelectedMonth == 12 && month == 1)
                {
                    year++;
                }
                else if (SelectedMonth == 1 && month == 12)
                {
                    year--;
                }
            }

            int day = _daysOfCurrentMonth[weekIndex, 0];
            var date = Culture.Calendar.ToDateTime(year, month, day, 0, 0, 0, 0);

            return Culture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, Culture.DateTimeFormat.FirstDayOfWeek);
        }

        public bool IsInCurrentMonth(int week, int day)
        {
            return (
                ((week == 0 || week == 1) && _daysOfCurrentMonth[week, day] > 20) ||
                ((week == 4 || week == 5) && _daysOfCurrentMonth[week, day] < 7)
                ) is false;
        }

        private int FindMonth(int week, int day)
        {
            int month = SelectedMonth;

            if (IsInCurrentMonth(week, day) is false)
            {
                if (week >= 4)
                {
                    month = SelectedMonth < 12 ? SelectedMonth + 1 : 1;
                }
                else
                {
                    month = SelectedMonth > 1 ? SelectedMonth - 1 : 12;
                }
            }

            return month;
        }

        public bool IsWeekDayOutOfMinAndMaxDate(int dayIndex, int weekIndex)
        {
            var day = _daysOfCurrentMonth[weekIndex, dayIndex];
            var month = FindMonth(weekIndex, dayIndex);

            if (MaxDate.HasValue)
            {
                var MaxDateYear = Culture.Calendar.GetYear(MaxDate.Value.DateTime);
                var MaxDateMonth = Culture.Calendar.GetMonth(MaxDate.Value.DateTime);
                var MaxDateDay = Culture.Calendar.GetDayOfMonth(MaxDate.Value.DateTime);

                if (SelectedYear > MaxDateYear ||
                    (SelectedYear == MaxDateYear && month > MaxDateMonth) ||
                    (SelectedYear == MaxDateYear && month == MaxDateMonth && day > MaxDateDay)) return true;
            }

            if (MinDate.HasValue)
            {
                var MinDateYear = Culture.Calendar.GetYear(MinDate.Value.DateTime);
                var MinDateMonth = Culture.Calendar.GetMonth(MinDate.Value.DateTime);
                var MinDateDay = Culture.Calendar.GetDayOfMonth(MinDate.Value.DateTime);

                if (SelectedYear < MinDateYear ||
                    (SelectedYear == MinDateYear && month < MinDateMonth) ||
                    (SelectedYear == MinDateYear && month == MinDateMonth && day < MinDateDay)) return true;
            }

            return false;
        }

        public bool IsYearOutOfMinAndMaxDate(int year)
        {
            return (MaxDate.HasValue && year > Culture.Calendar.GetYear(MaxDate.Value.DateTime))
                || (MinDate.HasValue && year < Culture.Calendar.GetYear(MinDate.Value.DateTime));
        }

        public bool IsMonthOutOfMinAndMaxDate(int month)
        {
            if (MaxDate.HasValue)
            {
                var MaxDateYear = Culture.Calendar.GetYear(MaxDate.Value.DateTime);
                var MaxDateMonth = Culture.Calendar.GetMonth(MaxDate.Value.DateTime);

                if (SelectedYear > MaxDateYear || (SelectedYear == MaxDateYear && month > MaxDateMonth)) return true;
            }

            if (MinDate.HasValue)
            {
                var MinDateYear = Culture.Calendar.GetYear(MinDate.Value.DateTime);
                var MinDateMonth = Culture.Calendar.GetMonth(MinDate.Value.DateTime);

                if (SelectedYear < MinDateYear || (SelectedYear == MinDateYear && month < MinDateMonth)) return true;
            }

            return false;
        }

        public async Task SelectDate(int dayIndex, int weekIndex)
        {
            if (IsWeekDayOutOfMinAndMaxDate(dayIndex, weekIndex)) return;

            SelectedDay = _daysOfCurrentMonth[weekIndex, dayIndex];
            int selectedMonth = FindMonth(weekIndex, dayIndex);
            var isNotInCurrentMonth = IsInCurrentMonth(weekIndex, dayIndex) is false;

            //The number of days displayed in the picker is about 34 days, and if the selected day is less than 15, it means that the next month has been selected in next year.
            if (selectedMonth < SelectedMonth && SelectedMonth == 12 && isNotInCurrentMonth && SelectedDay < 15)
            {
                SelectedYear++;
            }

            //The number of days displayed in the picker is about 34 days, and if the selected day is greater than 15, it means that the previous month has been selected in previous year.
            if (selectedMonth > SelectedMonth && SelectedMonth == 1 && isNotInCurrentMonth && SelectedDay > 15)
            {
                SelectedYear--;
            }

            SelectedMonth = selectedMonth;
        }

        public DateTimeOffset GetDateTimeOfDayCell(int dayIndex, int weekIndex)
        {
            int selectedMonth = FindMonth(weekIndex, dayIndex);
            var currentDay = _daysOfCurrentMonth[weekIndex, dayIndex];
            var currentYear = SelectedYear;
            if (selectedMonth < SelectedMonth && SelectedMonth == 12 && IsInCurrentMonth(weekIndex, dayIndex) is false)
            {
                currentYear++;
            }

            if (selectedMonth > SelectedMonth && SelectedMonth == 1 && IsInCurrentMonth(weekIndex, dayIndex) is false)
            {
                currentYear--;
            }

            return new DateTimeOffset(Culture.Calendar.ToDateTime(currentYear, selectedMonth, currentDay, 0, 0, 0, 0), DateTimeOffset.Now.Offset);
        }
    }
}
