using Microsoft.AspNetCore.Components;

namespace PadDatePicker;

public abstract class PadComponentBase : ComponentBase
{
    [CascadingParameter] protected Direction? CascadingDir { get; set; }

    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }

    // protected ElementClassBuilder ClassBuilder { get; private set; } = new ElementClassBuilder();
    // protected ElementClassBuilder StyleBuilder { get; private set; } = new ElementClassBuilder();

    private Direction? dir;

    [Parameter]
    public Direction? Dir
    {
        get => dir ?? CascadingDir;
        set
        {
            if (dir == value) return;

            dir = value;
        }
    }

    /// <summary>
    /// The additional HTML attributes to apply to this component.
    /// </summary>
    /// <remarks>
    /// This property is typically used to provide additional HTML attributes during rendering such as ARIA accessibility tags or a custom ID.
    /// </remarks>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes { get; set; } = new Dictionary<string, object?>();
}
