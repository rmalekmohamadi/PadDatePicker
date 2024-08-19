namespace PadDatePicker;

public class DatePickerClassStyles
{
    /// <summary>
    /// Custom CSS classes/styles for the root element of the DatePicker.
    /// </summary>
    public string? Root { get; set; } = "relative";

    /// <summary>
    /// Custom CSS classes/styles for the label of the DatePicker.
    /// </summary>
    public string? Label { get; set; } = "block text-xs font-medium text-gray-900";

    /// <summary>
    /// Custom CSS classes/styles for the input wrapper of the DatePicker.
    /// </summary>
    public string? InputWrapper { get; set; } = "mt-2 mb-4";

    /// <summary>
    /// Custom CSS classes/styles for the input container of the DatePicker.
    /// </summary>
    public string? InputContainer { get; set; } = "flex text-xs w-full rounded-md border border-gray-200 bg-white bg-gray-white focus:ring-blue-500 focus:ring-2 dark:bg-gray-700 dark:focus:ring-blue-500 focus:border-blue-500 border-gray-300 dark:border-gray-600 dark:focus:border-blue-500";

    /// <summary>
    /// Custom CSS classes/styles for the input of the DatePicker.
    /// </summary>
    public string? Input { get; set; } = "block flex-1 px-1 rounded-md w-full border-0 p-2 text-gray-900 dark:text-gray-400 dark:placeholder-gray-400 sm:text-xs disabled:bg-gray-100 disabled:text-gray-300 focus:outline-none focus:ring-0 focus:ring-offset-0";

    /// <summary>
    /// Custom CSS classes/styles for the icon's wrapper of the DatePicker.
    /// </summary>
    public string? IconWrapper { get; set; } = "flex select-none items-center sm:text-xs px-3 border-e-2";

    /// <summary>
    /// Custom CSS classes/styles for the icon of the DatePicker.
    /// </summary>
    public string? Icon { get; set; } = "w-4 h-4 text-gray-500 dark:text-gray-400";

    /// <summary>
    /// Custom CSS classes/styles for the start label of the DatePicker.
    /// </summary>
    public string? StartRangeLabel { get; set; } = "flex items-center sm:text-xs px-3 border-e-2";

    /// <summary>
    /// Custom CSS classes/styles for the end label of the DatePicker.
    /// </summary>
    public string? EndRangeLabel { get; set; } = "flex items-center sm:text-xs px-3 border-e-2";

    /// <summary>
    /// Custom CSS classes/styles for the picker's wrapper of the DatePicker.
    /// </summary>
    public string? PickerWrapper { get; set; } = "absolute top-1/2 start-1/3 z-50 pt-2 active block rounded-lg shadow-lg";

    /// <summary>
    /// Custom CSS classes/styles for the picker's wrapper of the DatePicker.
    /// </summary>
    //public string? PickerContainer{ get; set; } = "inline-block rounded-lg bg-white dark:bg-gray-700 shadow-lg p-4";
    public string? PickerContainer{ get; set; } = "inline-block bg-white dark:bg-gray-700 p-4";

    /// <summary>
    /// Custom CSS classes/styles for the picker's header label of the DatePicker.
    /// </summary>
    public string? PickerHeaderLabel { get; set; } = "bg-white dark:bg-gray-700 dark:text-white px-2 py-3 text-center font-semibold";

    /// <summary>
    /// Custom CSS classes/styles for the picker's header of the DatePicker.
    /// </summary>
    public string? PickerHeader { get; set; } = "flex justify-between mb-2";

    /// <summary>
    /// Custom CSS classes/styles for the Go to previous button of the DatePicker.
    /// </summary>
    public string? PrevNavButton { get; set; } = "bg-white dark:bg-gray-700 rounded-lg text-gray-500 dark:text-white hover:bg-gray-100 dark:hover:bg-gray-600 hover:text-gray-900 dark:hover:text-white text-lg p-2.5 focus:outline-none focus:ring-2 focus:ring-gray-200";

    /// <summary>
    /// Custom CSS classes/styles for the Go to previous icon of the DatePicker.
    /// </summary>
    public string? PrevNavIcon { get; set; } = "w-4 h-4 text-gray-800 dark:text-white";

    /// <summary>
    /// Custom CSS classes/styles for the Go to previous button of the DatePicker.
    /// </summary>
    public string? NextNavButton { get; set; } = "bg-white dark:bg-gray-700 rounded-lg text-gray-500 dark:text-white hover:bg-gray-100 dark:hover:bg-gray-600 hover:text-gray-900 dark:hover:text-white text-lg p-2.5 focus:outline-none focus:ring-2 focus:ring-gray-200";

    /// <summary>
    /// Custom CSS classes/styles for the Go to previous icon of the DatePicker.
    /// </summary>
    public string? NextNavIcon { get; set; } = "w-4 h-4 text-gray-800 dark:text-white";

    /// <summary>
    /// Custom CSS classes/styles for the header button of the DatePicker for switch the view.
    /// </summary>
    public string? HeaderButton { get; set; } = " text-xs rounded-lg text-gray-900 dark:text-white bg-white dark:bg-gray-700 font-semibold py-2.5 px-5 hover:bg-gray-100 dark:hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-200;";

    /// <summary>
    /// Custom CSS classes/styles for the footer's wrapper of the DatePicker.
    /// </summary>
    public string? FooterWrapper { get; set; } = "flex space-x-2 rtl:space-x-reverse mt-2";

    /// <summary>
    /// Custom CSS classes/styles for the Go to today button of the DatePicker.
    /// </summary>
    public string? GoToTodayButton { get; set; } = "button today-btn text-white bg-blue-700 !bg-primary-700 dark:bg-blue-600 dark:!bg-primary-600 hover:bg-blue-800 hover:!bg-primary-800 dark:hover:bg-blue-700 dark:hover:!bg-primary-700 focus:ring-4 focus:ring-blue-300 focus:!ring-primary-300 font-medium rounded-lg text-xs px-5 py-2 text-center w-1/2";

    /// <summary>
    /// Custom CSS classes/styles for the Clear button of the DatePicker.
    /// </summary>
    public string? ClearButton { get; set; } = "button clear-btn text-gray-900 dark:text-white bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 hover:bg-gray-100 dark:hover:bg-gray-600 focus:ring-4 focus:ring-blue-300 focus:!ring-primary-300 font-medium rounded-lg text-xs px-5 py-2 text-center w-1/2";

    /// <summary>
    /// Custom CSS classes/styles for the close button of the DatePicker.
    /// </summary>
    public string? CloseButton { get; set; } = "ms-auto focus:ring-2 p-1.5 inline-flex items-center justify-center float-end bg-gray-50 text-gray-500 focus:ring-gray-400 hover:bg-gray-200 dark:bg-gray-800 dark:text-gray-300 dark:hover:bg-gray-700 dark:hover:text-white";

    /// <summary>
    /// Custom CSS classes/styles for the close button icon of the DatePicker.
    /// </summary>
    public string? CloseButtonIcon { get; set; } = "w-2 h-2";

    /// <summary>
    /// Custom CSS classes/styles for the bodie's wrapper of the DatePicker.
    /// </summary>
    public string? BodyWrapper { get; set; } = "flex";

    /// <summary>
    /// Custom CSS classes/styles for the days of the DatePicker.
    /// </summary>
    public string? DaysRow { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the header row of the days of the DatePicker.
    /// </summary>
    public string? DaysHeaderRow { get; set; } = "mb-1";

    /// <summary>
    /// Custom CSS classes/styles for the header row of the days of the DatePicker.
    /// </summary>
    public string? DaysHeaderButton { get; set; } = "text-center h-6 leading-6 text-xs font-medium text-gray-500 dark:text-gray-400";

    /// <summary>
    /// Custom CSS classes/styles for each day button of the DatePicker.
    /// </summary>
    public string? DayButton { get; set; } = "block flex-1 leading-9 border-0 rounded-lg text-center font-semibold text-sm";

    /// <summary>
    /// Custom CSS classes/styles for each enable day button of the DatePicker.
    /// </summary>
    public string? EnableDayButton { get; set; } = "cursor-pointer";

    /// <summary>
    /// Custom CSS classes/styles for each disable day button of the DatePicker.
    /// </summary>
    public string? DisableDayButton { get; set; } = "text-gray-400 cursor-not-allowed";

    /// <summary>
    /// Custom CSS classes/styles for each normal day button of the DatePicker.
    /// </summary>
    public string? NormalDayButton { get; set; } = "text-gray-900 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-white";

    /// <summary>
    /// Custom CSS classes/styles for today day button of the DatePicker.
    /// </summary>
    public string? TodayDayButton { get; set; } = "bg-blue-200 !bg-primary-200 text-gray-500 dark:bg-blue-300 dark:!bg-primary-300";

    /// <summary>
    /// Custom CSS classes/styles for selected day button of the DatePicker.
    /// </summary>
    public string? SelectedDayButton { get; set; } = "bg-blue-700 !bg-primary-700 text-white dark:bg-blue-600 dark:!bg-primary-600 focused";

    /// <summary>
    /// Custom CSS classes/styles for in range day button of the DatePicker.
    /// </summary>
    public string? InRangeDayButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's input container of the DatePicker.
    /// </summary>
    public string? TimeInputContainer { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's hour input container of the DatePicker.
    /// </summary>
    public string? HourInputContainer { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's minute input container of the DatePicker.
    /// </summary>
    public string? MinuteInputContainer { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's wrapper of the DatePicker.
    /// </summary>
    public string? TimePickerWrapper { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's hour input of the DatePicker.
    /// </summary>
    public string? TimePickerHourInput { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's divider of the DatePicker.
    /// </summary>
    public string? TimePickerDivider { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's minute input of the DatePicker.
    /// </summary>
    public string? TimePickerMinuteInput { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's increase hour button of the DatePicker.
    /// </summary>
    public string? TimePickerIncreaseHourButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's increase hour icon of the DatePicker.
    /// </summary>
    public string? TimePickerIncreaseHourIcon { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's decrease hour button of the DatePicker.
    /// </summary>
    public string? TimePickerDecreaseHourButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's decrease hour icon of the DatePicker.
    /// </summary>
    public string? TimePickerDecreaseHourIcon { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's increase minute button of the DatePicker.
    /// </summary>
    public string? TimePickerIncreaseMinuteButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's increase minute icon of the DatePicker.
    /// </summary>
    public string? TimePickerIncreaseMinuteIcon { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's decrease minute button of the DatePicker.
    /// </summary>
    public string? TimePickerDecreaseMinuteButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's decrease minute icon of the DatePicker.
    /// </summary>
    public string? TimePickerDecreaseMinuteIcon { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's Am Pm container of the DatePicker.
    /// </summary>
    public string? TimePickerAmPmContainer { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's Am button of the DatePicker.
    /// </summary>
    public string? TimePickerAmButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's Pm button of the DatePicker.
    /// </summary>
    public string? TimePickerPmButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the main divider of the DatePicker.
    /// </summary>
    public string? Divider { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the year-month-picker's wrapper of the DatePicker.
    /// </summary>
    public string? YearMonthPickerWrapper { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the month-picker's header of the DatePicker.
    /// </summary>
    public string? MonthPickerHeader { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the time-picker's header of the DatePicker.
    /// </summary>
    public string? TimePickerHeader { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the year-picker's toggle button of the DatePicker.
    /// </summary>
    public string? YearPickerToggleButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the show time-picker button of the DatePicker.
    /// </summary>
    public string? ShowTimePickerButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the show time-picker icon of the DatePicker.
    /// </summary>
    public string? ShowTimePickerIcon { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the wrapper of the month-picker's nav buttons of the DatePicker.
    /// </summary>
    public string? MonthPickerNavWrapper { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the wrapper of the time-picker's nav buttons of the DatePicker.
    /// </summary>
    public string? TimePickerNavWrapper { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the Go to previous year button of the DatePicker.
    /// </summary>
    public string? PrevYearNavButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the Go to previous year icon of the DatePicker.
    /// </summary>
    public string? PrevYearNavIcon { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the Go to next year button of the DatePicker.
    /// </summary>
    public string? NextYearNavButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the Go to next year icon of the DatePicker.
    /// </summary>
    public string? NextYearNavIcon { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the months container of the DatePicker.
    /// </summary>
    public string? MonthsContainer { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for each row of the months of the DatePicker.
    /// </summary>
    public string? MonthsRow { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for each month button of the DatePicker.
    /// </summary>
    public string? MonthButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the year-picker's header of the DatePicker.
    /// </summary>
    public string? YearPickerHeader { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the month-picker's toggle button of the DatePicker.
    /// </summary>
    public string? MonthPickerToggleButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the wrapper of the year-picker nav buttons of the DatePicker.
    /// </summary>
    public string? YearPickerNavWrapper { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the Go to previous year-range button of the DatePicker.
    /// </summary>
    public string? PrevYearRangeNavButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the Go to previous year-range icon of the DatePicker.
    /// </summary>
    public string? PrevYearRangeNavIcon { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the Go to next year-range button of the DatePicker.
    /// </summary>
    public string? NextYearRangeNavButton { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for the Go to next year-range icon of the DatePicker.
    /// </summary>
    public string? NextYearRangeNavIcon { get; set; }

    /// <summary>
    /// Custom CSS classes/styles of the years container of the DatePicker.
    /// </summary>
    public string? YearsContainer { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for each row of the years of the DatePicker.
    /// </summary>
    public string? YearsRow { get; set; }

    /// <summary>
    /// Custom CSS classes/styles for each year button of the DatePicker.
    /// </summary>
    public string? YearButton { get; set; }
}
