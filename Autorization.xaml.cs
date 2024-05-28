using Practice.Models;
using System.Windows;
using Practice.NewFlorist;

namespace Practice
{
	public partial class Autorization : Window
	{
		private PracticeContext _db = new PracticeContext();

		public Autorization()
		{
			InitializeComponent();
		}

		private void Login_Click(object sender, RoutedEventArgs e)
		{
			List<User> users = _db.Users.ToList();

			string login = txtLogin.Text;
			string password = txtPassword.Password;
			bool isAuthenticated = false;

			foreach (User user in users)
			{
				if (user.UserLogin == login && user.UserPassword == password)
				{
					App.currentUser = user;
					MainWindow mainWindow = new MainWindow();
					mainWindow.Show();
					Close();
					isAuthenticated = true;
					break; 
				}
			}

			if (!isAuthenticated)
			{
				MessageBox.Show("Неверный логин или пароль");
			}
		}

		private void Authorization_Click(object sender, RoutedEventArgs e)
		{
			AddFlorist newWindow = new AddFlorist();
			newWindow.Show();
			Close();
		}
	}
}
