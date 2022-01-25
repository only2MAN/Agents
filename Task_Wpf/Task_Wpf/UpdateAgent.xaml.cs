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
    /// Логика взаимодействия для UpdateAgent.xaml
    /// </summary>
    public partial class UpdateAgent : Window
    {
        public UpdateAgent(int pr)
        {
            InitializeComponent();
            priorety.Text = pr.ToString();
        }

        private void okbtn_Click(object sender, RoutedEventArgs e)
        {
            MineMenu.HelpClass.flag = true;
            MineMenu.HelpClass.prioritet = Convert.ToInt32(priorety.Text);
            MineMenu.HelpClass.GetContext().SaveChanges();
            MessageBox.Show("Обновление приоритета агента завершено");
            this.Close();
        }

        private void otmen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
