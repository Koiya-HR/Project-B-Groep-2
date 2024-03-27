

public class Film
{

    public string Title;


    private int _length;

    public int Length
    {
        get { return _length; }
        set { _length = value <= 0 ? 1 : value; }
    }

    public Film(string title, int length)
    {
        Title = title;
        Length = length;
    }
}
