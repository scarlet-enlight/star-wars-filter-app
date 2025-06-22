using StarWarsFilterApp.Model;
using StarWarsFilterApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StarWarsFilterApp.View
{
    /// <summary>
    /// Logika interakcji dla klasy FilterView.xaml
    /// </summary>
    public partial class FilterCharacterView : UserControl
    {
        public FilterCharacterView(MainWindowViewModel main)
        {
            InitializeComponent();
            DataContext = new FilterCharacterViewModel(main);
        }

        private void Lb_characters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.DataContext is Character ch)
                (DataContext as FilterCharacterViewModel)?.ShowDetailCommand.Execute(ch);
        }
    }
}
