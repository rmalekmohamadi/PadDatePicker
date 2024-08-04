using Microsoft.AspNetCore.Components;

namespace PadDatePicker;

public abstract class PadComponentBase : ComponentBase
{
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }

    protected ElementClassBuilder ClassBuilder { get; private set; } = new ElementClassBuilder();
    protected ElementClassBuilder StyleBuilder { get; private set; } = new ElementClassBuilder();

    private Direction? dir = Direction.LTR;

    [Parameter]
    public Direction? Dir
    {
        get => dir;
        set
        {
            if (dir == value) return;

            dir = value;
        }
    }
}
