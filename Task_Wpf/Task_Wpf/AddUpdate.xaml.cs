using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Task_Wpf
{
    /// <summary>
    /// Логика взаимодействия для AddUpdate.xaml
    /// </summary>
    public partial class AddUpdate : Page
    {


        Agent agent;
        Agent agent1;
        int curSelPr, curTypAg;

        public AddUpdate(Agent ag)
        {
            InitializeComponent();

            try
            {
                Type.ItemsSource = MineMenu.HelpClass.GetContext().AgentType.ToList();
                product.ItemsSource = MineMenu.HelpClass.GetContext().Product.ToList();
            }
            catch { };


            if (ag != null)
            {
                agent1 = ag;

                Type.SelectedItem = ag.AgentType;
                this.Title.Text = ag.Title;
                this.Adress.Text = ag.Address;
                this.Inn.Text = ag.INN;
                this.Kpp.Text = ag.KPP;
                this.Director.Text = ag.DirectorName;
                this.Phone.Text = ag.Phone;
                this.Emale.Text = ag.Email;
                this.Logo.Text = ag.Logo;
                this.Prioritet.Text = Convert.ToString(ag.Priority);
                btnWriteAg.Visibility = Visibility.Collapsed;


                historyGrid.ItemsSource = MineMenu.HelpClass.GetContext().ProductSale.Where(ProductSale => ProductSale.AgentID == ag.ID).ToList();
            }
            else
            {
                btnUpdateAg.Visibility = Visibility.Collapsed;
                btnDelAg.IsEnabled = false;
                btnWritHi.IsEnabled = false;
                btnDelHi.IsEnabled = false;
            }
            this.DataContext = agent;
        }

        public void valEmail() {
            string pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            if (!Regex.IsMatch(this.Emale.Text, pattern))
            {
              MessageBox.Show("Неверный адрес электронной почты! Email должен иметь формат: x@gmail.com");
            }
        }

        public void valInn()
        {
            string Innpattern = @"\d{10}|\d{12}";
            if (!Regex.IsMatch(this.Inn.Text, Innpattern))
            {
                MessageBox.Show("Недействительный ИНН");
            }
        }

        public void valKpp()
        {
            string Kpppattern = @"\d{10}|\d{12}";

            if (!Regex.IsMatch(this.Kpp.Text, Kpppattern))
            {
                MessageBox.Show("Недействительный КПП");
            }
        }


        public void valPhone()
        {
            string Phonepattern = @"^\+[1-9]\d{3}-\d{3}-\d{4}$";

            if (!Regex.IsMatch(this.Phone.Text, Phonepattern))
            {
                MessageBox.Show("Номер телефона должен иметь формат: +xxxx-xxx-xxxx");
            }
        }

        private void btnWriteAg_Click(object sender, RoutedEventArgs e)
        {
   
            try
            {
              if (Prioritet.Text != null && Title.Text != null && curTypAg!= 0 && Adress.Text != null && Emale.Text != null)
                {
              

                    Agent agent2 = new Agent();
                  
                   

                    agent2.Priority = Convert.ToInt32(this.Prioritet.Text);
                    agent2.Title = this.Title.Text;
                    agent2.AgentTypeID = curTypAg;
                    agent2.Address = this.Adress.Text;
                    agent2.INN = this.Inn.Text;
                    agent2.KPP = this.Kpp.Text;
                    agent2.Phone = this.Phone.Text;
                    agent2.DirectorName = this.Director.Text;
                    agent2.Phone = this.Phone.Text;
                    agent2.Email = this.Emale.Text;
                    agent2.Logo = this.Logo.Text;

                    MineMenu.HelpClass.GetContext().Agent.Add(agent2);
                    MineMenu.HelpClass.GetContext().SaveChanges();
                    MessageBox.Show("Добавление информации об агенте завершено!");


                    btnDelAg.IsEnabled = true;
                    btnWritHi.IsEnabled = true;
                    btnDelHi.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Заполните пустые поля!");

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void btnDelAg_Click(object sender, RoutedEventArgs e)
        {
            if (agent1.ProductSale.Count > 0)
            {
                MessageBox.Show("Удаление не возможно!");
                return;
            }

            foreach (Shop shop in agent1.Shop)
            {
                MineMenu.HelpClass.GetContext().Shop.Remove(shop);
            }

            foreach (AgentPriorityHistory apr in agent1.AgentPriorityHistory)
            {
                MineMenu.HelpClass.GetContext().AgentPriorityHistory.Remove(apr);
            }

            MineMenu.HelpClass.GetContext().Agent.Remove(agent1);
            MineMenu.HelpClass.GetContext().SaveChanges();
            MessageBox.Show("Удаление информации об агенте завешено!");
            this.NavigationService.GoBack();

        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curTypAg = ((AgentType)Type.SelectedItem).ID;
        }

        private void historyGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void product_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curSelPr = ((Product)product.SelectedItem).ID;
        }

        private void btnWritHi_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;
            try
            {
                cnt = Convert.ToInt32(count.Text);
            }
            catch
            {
                return;
            }
            string dt = date.ToString();
            if (curSelPr > 0 && dt != "" && cnt > 0)
            {
                ProductSale pr = new ProductSale();
                pr.AgentID = agent1.ID;
                pr.ProductID = curSelPr;
                pr.SaleDate = (DateTime)date.SelectedDate;
                pr.ProductCount = cnt;
                try
                {
                    MineMenu.HelpClass.GetContext().ProductSale.Add(pr);
                    MineMenu.HelpClass.GetContext().SaveChanges();
                    historyGrid.ItemsSource = MineMenu.HelpClass.GetContext().ProductSale.Where(ProductSale => ProductSale.AgentID == agent1.ID).ToList();
                }
                catch
                {
                    return;
                }
            }
        }

        private void btnDelHi_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < historyGrid.SelectedItems.Count; i++)
            {
                ProductSale prs = historyGrid.SelectedItems[i] as ProductSale;
                if (prs != null)
                {
                    MineMenu.HelpClass.GetContext().ProductSale.Remove(prs);
                }
            }
            try
            {
                MineMenu.HelpClass.GetContext().SaveChanges();
                historyGrid.ItemsSource = MineMenu.HelpClass.GetContext().ProductSale.Where(ProductSale => ProductSale.AgentID == agent.ID).ToList();
            }
            catch { return; };

        }

        private void mask_TextChanged(object sender, TextChangedEventArgs e)
        {
            string fnd = ((TextBox)sender).Text;
            try
            {
                product.ItemsSource = MineMenu.HelpClass.GetContext().Product.Where(Product => Product.Title.Contains(fnd)).ToList();
            }
            catch { };

        }

        private void btnUpdateAg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Prioritet.Text != null && Title.Text != null && Adress.Text != null && Emale.Text != null)
                {
                    agent1.Priority = Convert.ToInt32(this.Prioritet.Text);
                    agent1.Title = this.Title.Text;
                    agent1.AgentTypeID = curTypAg;
                    agent1.Address = this.Adress.Text;
                    agent1.INN = this.Inn.Text;
                    agent1.KPP = this.Kpp.Text;
                    agent1.Phone = this.Phone.Text;
                    agent1.DirectorName = this.Director.Text;
                    agent1.Phone = this.Phone.Text;
                    agent1.Email = this.Emale.Text;

                    MineMenu.HelpClass.GetContext().SaveChanges();
                    MessageBox.Show("Обновление информации об агенте завершено");
                }
                else
                {
                    MessageBox.Show("Заполните пустые поля!");

                }

            }
            catch
            {
                return;
            }
        }
    }
}
