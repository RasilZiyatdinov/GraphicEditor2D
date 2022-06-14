using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;

namespace GraphicEditor2D
{
    public class BrushSettings : INotifyPropertyChanged
    {
        private Color colorStroke;
        public Color ColorStroke
        { 
            get
            {
                return colorStroke;
            }
            set
            {
                colorStroke = value;
                brushStroke = new SolidColorBrush(colorStroke);

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColorStroke"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BrushStroke"));
            }
        }

        private SolidColorBrush brushStroke;
        public Brush BrushStroke
        {
            get
            {
                return brushStroke;
            }
            set
            {
                brushStroke = (SolidColorBrush)value;
                colorStroke = brushStroke.Color;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BrushStroke"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColorStroke"));
            }
        }

        private  int thickness { get; set; }
        public int Thickness
        {
            get
            {
                return thickness;
            }
            set
            {
                thickness = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Thickness"));
            }
        }

        public BrushSettings(int thick, Color c)
        {
            thickness = thick;
            colorStroke = c;
            brushStroke = new SolidColorBrush(c);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
