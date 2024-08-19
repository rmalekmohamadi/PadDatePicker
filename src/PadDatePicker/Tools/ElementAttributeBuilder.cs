namespace PadDatePicker
{
    public class ElementStyleBuilder : ElementAttributeBuilder
    {
        protected override char Separator { get => ';'; }
    }

    public class ElementClassBuilder : ElementAttributeBuilder
    {
        protected override char Separator { get => ' '; }
    }

    public abstract class ElementAttributeBuilder
    {
        private string _value = string.Empty;
        private List<string> _attrs = new();

        protected abstract char Separator { get; }

        public string Value
        {
            get
            {
                Build();
                return _value;
            }
        }

        public ElementAttributeBuilder Add(string attr)
        {
            if(!_attrs.Contains(attr))
            {
                _attrs.Add(attr);
            }
            return this;
        }

        public void Reset()
        {
            _attrs = new();
        }

        private void Build()
        {
            _value = string.Join(Separator, _attrs.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }
}
