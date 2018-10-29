using System;

namespace shedule_parser
{
    public partial class Shedule : Gtk.Window
    {
        public Shedule(string data) : base(Gtk.WindowType.Toplevel)
        {
            Build();
            textview1.Buffer.Text = data;
        }
    }
}
