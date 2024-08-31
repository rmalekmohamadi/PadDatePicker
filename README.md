<h1>
  <picture>
    <source media="(prefers-color-scheme: dark)" srcset="content/Nuget.png">
    <source media="(prefers-color-scheme: light)" srcset="content/Nuget.png">
    <img alt="PadDatePicker" src="content/Nuget.png">
  </picture>
</h1>

# PadDatePicker
Tailwind DatePicker components for Blazor



## Installation

Install Package
```
dotnet add package PadDatePicker
```
## Features

- Pick DateTime
- Pick Range of DateTime
- Different Culture such as Persian Culture

## Usage
```
<PadDatePicker.PadDatePickerDialog HighlightCurrent ShowOkButton ShowClearButton ShowToDayButton ShowLabelOnHeader Classes="@classStyles" @bind-Value="_value" Label="Please Choose Date" />

<PadDatePicker.PadDateRangePicker HighlightCurrent ShowOkButton ShowClearButton ShowToDayButton ShowLabelOnHeader @bind-Value="_valueRange" />

@code
{
    private DateTimeOffset? _value;
    private PadDateTimeRange _valueRange;
    private DatePickerClassStyles classStyles = new DatePickerClassStyles();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        classStyles.NormalDayButton = "bg-green-500";
    }
}
```
