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
using System.Windows.Shapes;

namespace Task_Wpf
{
    /// <summary>
    /// Логика взаимодействия для MineMenu.xaml
    /// </summary>
    public partial class MineMenu : Window
    {
        public MineMenu()
        {
            InitializeComponent();

            frame.Content = new Agents(frame);
        }

        public class HelpClass{
            public static Wpf_desktopEntities db;
            public static bool flag = false;
            public static int prioritet = 0;

            public static Wpf_desktopEntities GetContext()
            {
               
                    if (db == null)
                    {
                        db = new Wpf_desktopEntities();
                    }
                    return db;
            }
        }

        private void frame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                Agents pg = (Agents)e.Content;
                pg.SelectAgent();
            }
            catch { };

        }
    }
}
