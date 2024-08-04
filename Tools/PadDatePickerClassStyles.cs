namespace PadDatePicker.Tools;

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
    public string? IconWrapper { get; set; } = "flex select-none items-center sm:text-xs px-3";

    /// <summary>
    /// Custom CSS classes/styles for the icon of the DatePicker.
    /// </summary>
    public string? Icon { get; set; } = "w-4 h-4 text-gray-500 dark:text-gray-400";

    /// <summary>
    /// Custom CSS classes/styles for the picker's wrapper of the DatePicker.
    /// </summary>
    public string? PickerWrapper { get; set; } = "absolute top-1/2 start-1/3 z-50 pt-2 active block datepicker-orient-bottom datepicker-orient-left";

    /// <summary>
    /// Custom CSS classes/styles for the picker's wrapper of the DatePicker.
    /// </summary>
    public string? PickerContainer { get; set; } = "inline-block rounded-lg bg-white dark:bg-gray-700 shadow-lg p-4";

    /// <summary>
    /// Custom CSS classes/styles for the picker's header label of the DatePicker.
    /// </summary>
    public string? PickerHeaderLabel { get; set; } = "datepicker-title bg-white dark:bg-gray-700 dark:text-white px-2 py-3 text-center font-semibold";

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
    /// Custom CSS classes/styles for the header row of the days of the DatePicker.
    /// </summary>
    public string? DaysHeaderRow { get; set; } = "grid grid-cols-7 mb-1";

    /// <summary>
    /// Custom CSS classes/styles for the header col of row of the days of the DatePicker.
    /// </summary>
    public string? DaysHeaderRowCol { get; set; } = "text-center h-6 leading-6 text-xs font-medium text-gray-500 dark:text-gray-400";

    /// <summary>
    /// Custom CSS classes/styles for the grid of the body of the days of the DatePicker.
    /// </summary>
    public string? DaysBodyGrid { get; set; } = "w-64 grid grid-cols-7";

    /// <summary>
    /// Custom CSS classes/styles for the grid of the body of the months of the DatePicker.
    /// </summary>
    public string? MonthsBodyGrid { get; set; } = "flex w-64 grid grid-cols-4";

    /// <summary>
    /// Custom CSS classes/styles for the grid of the body of the years of the DatePicker.
    /// </summary>
    public string? YearsBodyGrid { get; set; } = "flex w-64 grid grid-cols-4";

    /// <summary>
    /// Custom default CSS classes/styles for buttons of the body of the DatePicker.
    /// </summary>
    public string? BodyBtnDefault { get; set; } = "block flex-1 leading-9 border-0 rounded-lg text-center font-semibold text-sm";

    /// <summary>
    /// Custom normal CSS classes/styles for buttons of the body of the DatePicker.
    /// </summary>
    public string? BodyBtnNormal { get; set; } = "text-gray-900 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-white";


    /// <summary>
    /// Custom enable CSS classes/styles for buttons of the body of the DatePicker.
    /// </summary>
    public string? BodyBtnEnable { get; set; } = "cursor-pointer";

    /// <summary>
    /// Custom selected CSS classes/styles for buttons of the body of the DatePicker.
    /// </summary>
    public string? BodyBtnSelected { get; set; } = "bg-blue-700 !bg-primary-700 text-white dark:bg-blue-600 dark:!bg-primary-600 focused";

    /// <summary>
    /// Custom current CSS classes/styles for buttons of the body of the DatePicker.
    /// </summary>
    public string? BodyBtnCurrent { get; set; } = "bg-blue-200 !bg-primary-200 text-gray-500 dark:bg-blue-300 dark:!bg-primary-300";

    /// <summary>
    /// Custom disable CSS classes/styles for buttons of the body of the DatePicker.
    /// </summary>
    public string? BodyBtnDisable { get; set; } = "text-gray-400 cursor-not-allowed";
}
