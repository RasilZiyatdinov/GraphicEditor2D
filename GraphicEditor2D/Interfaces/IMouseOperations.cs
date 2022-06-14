using System.Windows.Input;

namespace GraphicEditor2D
{
    public interface IMouseOperations
    {
        void MouseDown(object sender, MouseButtonEventArgs e);

        void MouseMove(object sender, MouseEventArgs e);

        void MouseUp(object sender, MouseButtonEventArgs e);

    }
}
