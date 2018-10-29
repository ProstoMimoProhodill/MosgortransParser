using System;
using Gtk;

public partial class MainWindow : Gtk.Window 
{
    private string Type { get; set; }
    private string Route { get; set; }
    private string Days { get; set; }
    private string Direction { get; set; }
    private string Waypoints { get; set; }

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        type_set();
        route_set();
        days_set();
        direction_set();
        waypoints_set();
    }


    protected void type_set()
    {
        ComboBox type = (ComboBox)combobox1;
        type.AppendText("Автобус");
        type.AppendText("Троллейбус");
        type.AppendText("Трамвай");
    }

    protected string type_get()
    {
        ComboBox type = (ComboBox)combobox1;
        string t = type.ActiveText;
        switch(t)
        {
            case "Автобус":
                t = "avto";
                break;
            case "Троллейбус":
                t = "trol";
                break;
            case "Трамвай":
                t = "tram";
                break;
        }

        return t;
    }

    protected void route_set()
    {
        ComboBox type = (ComboBox)combobox2;
        type.AppendText("288");
        type.AppendText("895");
        type.AppendText("117");
        type.AppendText("636");
        type.AppendText("94");
    }

    protected string route_get()
    {
        ComboBox route = (ComboBox)combobox2;
        return route.ActiveText;
    }

    protected void days_set()
    {
        ComboBox type = (ComboBox)combobox3;
        type.AppendText("Будни");
        type.AppendText("Выходные");
    }

    protected string days_get()
    {
        ComboBox days = (ComboBox)combobox3;
        string t = days.ActiveText;
        switch (t)
        {
            case "Будни":
                t = "1111100";
                break;
            case "Выходные":
                t = "0000011";
                break;
        }

        return t;
    }

    protected void direction_set()
    {
        ComboBox type = (ComboBox)combobox4;
        type.AppendText("AB");
        type.AppendText("BA");
    }

    protected string direction_get()
    {
        ComboBox direction = (ComboBox)combobox4;
        return direction.ActiveText;
    }

    protected void waypoints_set()
    {
        ComboBox type = (ComboBox)combobox5;
        type.AppendText("all");
    }

    protected string waypoints_get()
    {
        ComboBox waypoints = (ComboBox)combobox5;
        return waypoints.ActiveText;
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnSearchButtonClicked(object sender, EventArgs e)
    {
        Type = type_get();
        Route = route_get();
        Days = days_get();
        Direction = direction_get();
        Waypoints = waypoints_get();

        shedule_parser.MainClass.shedule_open(shedule_parser.MainClass.parse(Type, Route, Days, Direction, Waypoints));
    }
}
