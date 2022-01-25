using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Task_Wpf
{
    /// <summary>
    /// Логика взаимодействия для Agents.xaml
    /// </summary>
    public partial class Agents : Page
    {
        private int start = 0;
        private int fullCount = 0;
        Frame fr;
        Agent agent1;
        Agent ag;
        public Agents(Frame frame)
        {
            fr = frame;
            InitializeComponent();
            SelectAgent();
       
            List<AgentType> agents = new List<AgentType> { };
            agents = MineMenu.HelpClass.GetContext().AgentType.ToList();
            agents.Add(new AgentType { Title = "Все типы" });
            Type.ItemsSource = agents.OrderBy(AgentType => AgentType.ID);

        }
        public void turnButton()
        {
            if (start == 0) { Back.IsEnabled = false; }
            else { Back.IsEnabled = true; }
            if((start + 1) *10< fullCount) { forward.IsEnabled = false; }
            else { forward.IsEnabled = true; }
        }
       
        int order, iag;
        string fnd;

        public void SelectAgent()
        {

            agentTable.ItemsSource = MineMenu.HelpClass.GetContext().Agent.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
            int fullCount = MineMenu.HelpClass.GetContext().Agent.Count();
            full.Text=fullCount.ToString();
 
            int ost = fullCount % 10;
            int PageNum = (fullCount - ost) / 10;
            if (ost > 0) { PageNum++; }
            Paging.Children.Clear();

            try
            {
                fullCount = MineMenu.HelpClass.GetContext().Agent.Count();

                var ag = MineMenu.HelpClass.GetContext().Agent.Where(Agent => Agent.Title.Contains(fnd) || Agent.Phone.Contains(fnd) || Agent.Email.Contains(fnd));
                if (iag == 0)
                {
                    fullCount = ag.Count();
                    if (order == 0) agentTable.ItemsSource = MineMenu.HelpClass.GetContext().Agent.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
                    if (order == 1) agentTable.ItemsSource = MineMenu.HelpClass.GetContext().Agent.OrderBy(Agent => Agent.Title).Skip(start * 10).Take(10).ToList();
                    if (order == 2) agentTable.ItemsSource = MineMenu.HelpClass.GetContext().Agent.OrderByDescending(Agent => Agent.Title).Skip(start * 10).Take(10).ToList();
                    if (order == 3) agentTable.ItemsSource = MineMenu.HelpClass.GetContext().Agent.OrderBy(Agent => Agent.Priority).Skip(start * 10).Take(10).ToList();
                    if (order == 4) agentTable.ItemsSource = MineMenu.HelpClass.GetContext().Agent.OrderByDescending(Agent => Agent.Priority).Skip(start * 10).Take(10).ToList();
                }
                else
                {
                    var agg = ag.Where((Agent => Agent.AgentTypeID == iag));
                    fullCount = agg.Count();
                    if (order == 0) agentTable.ItemsSource = agg.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
                    if (order == 1) agentTable.ItemsSource = agg.OrderBy(Agent => Agent.Title).Skip(start * 10).Take(10).ToList();
                    if (order == 2) agentTable.ItemsSource = agg.OrderByDescending(Agent => Agent.Title).Skip(start * 10).Take(10).ToList();
                    if (order == 3) agentTable.ItemsSource = agg.OrderBy(Agent => Agent.Priority).Skip(start * 10).Take(10).ToList();
                    if (order == 4) agentTable.ItemsSource = agg.OrderByDescending(Agent => Agent.Priority).Skip(start * 10).Take(10).ToList();
                }
            }
            catch
            {
                return;
            };

            for (int i=0; i< PageNum; i++)
            {
                Button ChPag = new Button();
                ChPag.Height = 30;
                ChPag.Width = 20;
                ChPag.Content = i + 1;
                ChPag.HorizontalAlignment = HorizontalAlignment.Center;
                ChPag.Tag = i;
                ChPag.Click += new RoutedEventHandler(ChPag_Click);
                Paging.Children.Add(ChPag);
            }

            turnButton();
        }

        private void ChPag_Click(object sender, RoutedEventArgs e)
        {
            start=Convert.ToInt32(((Button)sender).Tag.ToString());
            SelectAgent();
            turnButton();

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (agentTable.SelectedItems.Count > 0)
            {
                int prt = 0;
                foreach (Agent agent in agentTable.SelectedItems)
                {
                    if (agent.Priority > prt) prt = agent.Priority;
                }
                
                UpdateAgent dlg = new UpdateAgent(prt);
                MineMenu.HelpClass.prioritet = prt;
                MineMenu.HelpClass.flag = false;
                dlg.ShowDialog();
                
                if (MineMenu.HelpClass.flag)
                {
                    foreach (Agent agent in agentTable.SelectedItems)
                    {
                        agent.Priority = MineMenu.HelpClass.prioritet;
                        MineMenu.HelpClass.GetContext().Entry(agent).State = EntityState.Modified;
                    }
                    MineMenu.HelpClass.GetContext().SaveChanges();
                }
            }

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            fr.Content = new AddUpdate(null);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            start--;
            turnButton();
            SelectAgent();
        }

        private void forward_Click(object sender, RoutedEventArgs e)
        {
            start++;
            turnButton();
            SelectAgent();
        }

        private void agentTable_LoadingRow(object sender, DataGridRowEventArgs e)
        {
         
            Agent agent = (Agent)e.Row.DataContext;

            agent.percent = 0;

            int sum = 0;
            double fsum = 0;
            foreach(ProductSale ps in agent.ProductSale)
            {
                List<ProductMaterial> mater = new List<ProductMaterial> { };
                mater = MineMenu.HelpClass.GetContext().ProductMaterial.Where(ProductMaterial => ProductMaterial.ProductID == ps.ProductID).ToList();

                foreach(ProductMaterial mat in mater)
                {
                    double fs = decimal.ToDouble(mat.Material.Cost);
                    fsum += fs * (double)mat.Material.Cost;
                }

                fsum = fsum * ps.ProductCount;
                if (ps.SaleDate.AddDays(365).CompareTo(DateTime.Today) > 0)
                sum+=ps.ProductCount;
            }
            agent.sale = sum;

            if (fsum >= 1000 && fsum < 50000) agent.percent = 5;
            if (fsum >= 50000 && fsum < 150000) agent.percent = 10;
            if (fsum >= 150000 && fsum < 500000) agent.percent = 20;
            if (fsum >= 500000) agent.percent = 25;

            if (agent.percent == 25)
            {
                SolidColorBrush scb=new SolidColorBrush(Colors.LightGreen);
                e.Row.Background= scb;
            }
            else
            {
                SolidColorBrush scb = new SolidColorBrush(Colors.White);
                e.Row.Background = scb;
            }
        }


        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem comboBoxItem = (ComboBoxItem)comboBox.SelectedItem;
            order = Convert.ToInt32(comboBoxItem.Tag.ToString());
            SelectAgent();

        }

        private void poisk_TextChanged(object sender, TextChangedEventArgs e)
        {
            fnd = ((TextBox)sender).Text;
            SelectAgent();

        }

        private void agentTable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (agentTable.SelectedItems.Count > 0)
            {
                Agent agent = agentTable.SelectedItems[0] as Agent;

                if (agent != null)
                {
                    fr.Content = new AddUpdate(agent);
                }
            }

        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            iag = ((AgentType)Type.SelectedItem).ID;
            SelectAgent();

        }
    }
}
