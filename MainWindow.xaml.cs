using Practice.Models;
using Practice.Pages;
using System.Windows;

namespace Practice
{
	public partial class MainWindow : Window
	{
		private readonly PracticeContext _context;
		public MainWindow()
		{
			InitializeComponent();
			_context = new PracticeContext(); // Инициализация переменной _context
		}

		private void AssortmentFrame_Loaded(object sender, RoutedEventArgs e)
		{
			AssortmentFrame.Navigate(new AssortmentsPage(_context));
		}
		private void ShopFrame_Loaded(object sender, RoutedEventArgs e)
		{
			if (App.currentUser.IdRole == 2)
			{
				ShopFrame.Visibility = Visibility.Collapsed;
				ShopTab.Visibility = Visibility.Collapsed;	
			}
			else
			{
				ShopFrame.Navigate(new ShopsPage(_context));
			}
		}

		private void FloristFrame_Loaded(object sender, RoutedEventArgs e)
		{
			if (App.currentUser.IdRole == 2)
			{
				FloristFrame.Visibility = Visibility.Collapsed;
				FloristTab.Visibility = Visibility.Collapsed;
			}
			else
			{	
				FloristFrame.Navigate(new FloristPage(_context));
			}
		}
		private void PlantFrame_Loaded(object sender, RoutedEventArgs e)
		{
			PlantFrame.Navigate(new PlantsPage(_context));
		}

		private void FlowerFrame_Loaded(object sender, RoutedEventArgs e)
		{
			FlowerFrame.Navigate(new FlowersPage(_context));
		}

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			Autorization aut = new Autorization();
			aut.Show();
			Close();
        }
    }
}