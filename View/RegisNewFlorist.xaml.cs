using Practice.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace Practice.View
{
	public partial class RegisNewFlorist : Window
    {
		PracticeContext db = new PracticeContext();
		public ObservableCollection<ShopsForSale> ListShop { get; set; }
		public RegisNewFlorist()
		{
			InitializeComponent();
			ListShop = new ObservableCollection<ShopsForSale>(db.ShopsForSales.ToList());
			cbShop.ItemsSource = ListShop;
			cbShop.DisplayMemberPath = "NameOfShop";
		}

		private void Registration_Click(object sender, RoutedEventArgs e)
		{
			string pass = txtPassword.Password;
			string enteredLogin = txtLogin.Text;
			User existingUser = db.Users.FirstOrDefault(u => u.UserLogin == enteredLogin);
			if (existingUser != null)
			{
				MessageBox.Show("Логин уже существует!");
				return;
			}
			else
			{
				bool isValid = ValidatePass(pass);
				if (isValid)
				{
					if (string.IsNullOrWhiteSpace(txtFName.Text) || string.IsNullOrWhiteSpace(txtLName.Text) 
						|| string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Password) || 
						cbShop.SelectedItem == null)
					{
						MessageBox.Show("Заполните все поля!");

						if (string.IsNullOrWhiteSpace(txtFName.Text))
							txtFName.BorderBrush = Brushes.Red;
						else
							txtFName.BorderBrush = Brushes.Black;

						if (string.IsNullOrWhiteSpace(txtLName.Text))
							txtLName.BorderBrush = Brushes.Red;
						else
							txtLName.BorderBrush = Brushes.Black;

						if (string.IsNullOrWhiteSpace(txtLogin.Text))
							txtLogin.BorderBrush = Brushes.Red;
						else
							txtLogin.BorderBrush = Brushes.Black;

						if(string.IsNullOrWhiteSpace(txtPassword.Password))
							txtPassword.BorderBrush = Brushes.Red;
						else
							txtPassword.BorderBrush = Brushes.Black;

						return;
					}
					else
					{
						User newUser = new User
						{
							UserLogin = txtLogin.Text,
							UserPassword = txtPassword.Password,
							IdRole = 2
						};
						db.Users.Add(newUser);
						db.SaveChanges();

						int newUserId = newUser.IdUser;
						ShopsForSale selectedShop = (ShopsForSale)cbShop.SelectedItem;
						int shopId = selectedShop.IdShop;
						Florist newFlorist = new Florist
						{
							LName = txtLName.Text,
							FName = txtFName.Text,
							Patronymic = txtPatronymic.Text,
							IdUser = newUserId,
							IdShop = shopId
						};
						db.Florists.Add(newFlorist);
						db.SaveChanges();

						MessageBox.Show("Данные успешно сохранены в базу данных.");
						OnDataUpdated();
						Close();
					}
				}
				else
				{
					txtPassword.Background = Brushes.Red;
				}
			}
		}

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private bool ValidatePass(string pass) 
		{
			if (pass.Length < 6)
			{
				MessageBox.Show("Введите минимум 6 символов.");
				return false;
			}

			if (!Regex.IsMatch(pass, @"[A-ZА-Я]"))
			{
				MessageBox.Show("Введите минимум одну прописную букву.");
				return false;
			}

			if (!Regex.IsMatch(pass, @"[0-9]"))
			{
				MessageBox.Show("Введите минимум одну цифру.");
				return false;
			}

			if (!Regex.IsMatch(pass, @"[!@#$%^.]"))
			{
				MessageBox.Show("Введите минимум один специальный символ - '!,@,#,$,%,^,.'.");
				return false;
			}
			return true;
		}
		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			TextBox? textBox = sender as TextBox;

			if (textBox.Name == "txtLName" || textBox.Name == "txtFName" || textBox.Name == "txtPatronymic")
			{
				if (char.IsDigit(e.Text, 0))
				{
					e.Handled = true;
				}
			}

		}

		public delegate void DataUpdatedEventHandler(object sender, EventArgs e);

		public event DataUpdatedEventHandler DataUpdated;

		private void OnDataUpdated()
		{
			if (DataUpdated != null)
				DataUpdated(this, EventArgs.Empty);
		}
	}
}
