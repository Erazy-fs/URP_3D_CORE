public interface ISetting
{
    public object Value { get; set; }

    public void Load();
    public void Save();
    public void Cancel();
    //public void OnChange(object value);
}
