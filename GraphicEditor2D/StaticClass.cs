using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace GraphicEditor2D
{
    public class StaticClass
    {
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        public static Shape DeletedGroupShape;

        private static string shapeName;
        public static string ShapeName
        {
            get => shapeName;
            set
            {
                shapeName = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(ShapeName)));
            }
        }

        private static ObservableCollection<string> grList;
        public static ObservableCollection<string> GrList
        {
            get => grList;
            set
            {
                grList = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(GrList)));

            }
        }
        public static Visibility buttonVisible;
        public static Visibility ButtonVisible
        {
            get => buttonVisible;
            set
            {
                buttonVisible = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(ButtonVisible)));
            }
        }
        public static Visibility buttonDeleteVisible;
        public static Visibility ButtonDeleteVisible
        {
            get => buttonVisible;
            set
            {
                buttonVisible = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(ButtonDeleteVisible)));
            }
        }
        private static string groupName;
        public static string GroupName
        {
            get => groupName;
            set
            {
                groupName = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(GroupName)));
            }
        }
    }
}
