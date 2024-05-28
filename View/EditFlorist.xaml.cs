using Practice.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Practice.View
{
	public partial class EditFlorist : Window
	{
		private PracticeContext _db;
		public Florist SelectedFlorist { get; set; }
		public ObservableCollection<ShopsForSale> ListShop { get; set; }
		public EditFlorist(Florist selectedFlorist)
		{
			SelectedFlorist = selectedFlorist;
			InitializeComponent();
			_db = new PracticeContext();

			if (selectedFlorist != null)
			{
				txtFName.Text = selectedFlorist.FName;
				txtLName.Text = selectedFlorist.LName;
				txtPatronymic.Text = selectedFlorist.Patronymic;
				ListShop = new ObservableCollection<ShopsForSale>(_db.ShopsForSales.ToList());
				cbShop.ItemsSource = ListShop;
				cbShop.DisplayMemberPath = "NameOfShop";
				cbShop.Text = selectedFlorist.IdShopNavigation.NameOfShop;
				txtLogin.Text = selectedFlorist.IdUserNavigation.UserLogin;
				txtPassword.Password = selectedFlorist.IdUserNavigation.UserPassword;
			}
		}

		public void Save_Click(object sender, RoutedEventArgs e)
		{
			string pass = txtPassword.Password;
			bool isValid = ValidatePass(pass);
			if (isValid)
			{
				if (string.IsNullOrWhiteSpace(txtFName.Text) || string.IsNullOrWhiteSpace(txtLName.Text) ||
	string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Password) ||
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

					if (string.IsNullOrWhiteSpace(txtPassword.Password))
						txtPassword.BorderBrush = Brushes.Red;
					else
						txtPassword.BorderBrush = Brushes.Black;

					return;
				}

				else
				{
					Florist existingFlorist = _db.Florists.FirstOrDefault(f => f.IdUserNavigation.UserLogin == SelectedFlorist.IdUserNavigation.UserLogin && f.IdUserNavigation.UserPassword == SelectedFlorist.IdUserNavigation.UserPassword);
					if (existingFlorist != null)
					{
						User existingUser = _db.Users.FirstOrDefault(u => u.UserLogin == txtLogin.Text && u.IdUser != existingFlorist.IdUser);
						if (existingUser != null)
						{
							MessageBox.Show("Логин уже используется другим пользователем. Выберите другой логин.");
							return;
						}

						existingFlorist.FName = txtFName.Text;
						existingFlorist.LName = txtLName.Text;
						existingFlorist.Patronymic = txtPatronymic.Text;
						ShopsForSale selectedShop = (ShopsForSale)cbShop.SelectedItem;
						existingFlorist.IdShop = selectedShop.IdShop;
						User associatedUser = _db.Users.FirstOrDefault(u => u.IdUser == existingFlorist.IdUser);
						if (associatedUser != null)
						{
							associatedUser.UserLogin = txtLogin.Text;
							associatedUser.UserPassword = txtPassword.Password;

							_db.SaveChanges();
							MessageBox.Show("Данные успешно обновлены в базе данных.");
							OnDataUpdated();
							Close();
						}
					}
					else
					{
						MessageBox.Show("Флорист не найден в базе данных.");
					}
				}
			}
			else
			{
				txtPassword.Background = Brushes.Red;
				MessageBox.Show("Пароль не соответствует требованиям.");
			}
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

		public void Close_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
