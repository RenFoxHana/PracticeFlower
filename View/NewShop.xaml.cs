using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Practice.View
{
	public partial class NewShop : Window
	{
		public NewShop()
		{
			InitializeComponent();
		}
		private void Save_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtIndex.Text) || txtIndex.Text.Length != 6)
			{
				MessageBox.Show("Индекс должен состоять из 6 цифр.");
				txtIndex.BorderBrush = Brushes.Red;
				return;
			}
			else
				txtIndex.BorderBrush = Brushes.Black;

			if (string.IsNullOrWhiteSpace(txtCity.Text) || string.IsNullOrWhiteSpace(txtStreet.Text) ||
	string.IsNullOrWhiteSpace(txtBuild.Text) || string.IsNullOrWhiteSpace(txtName.Text) ||
	string.IsNullOrWhiteSpace(txtArea.Text) || (cmbSale.SelectedItem as ComboBoxItem) == null ||
	(cmbOrder.SelectedItem as ComboBoxItem) == null)
			{
				MessageBox.Show("Пожалуйста, заполните все поля!");

				if (string.IsNullOrWhiteSpace(txtCity.Text))
					txtCity.BorderBrush = Brushes.Red;
				else 
				txtCity.BorderBrush = Brushes.Black;

				if (string.IsNullOrWhiteSpace(txtStreet.Text))
					txtStreet.BorderBrush = Brushes.Red;
				else
					txtStreet.BorderBrush = Brushes.Black;

				if (string.IsNullOrWhiteSpace(txtBuild.Text))
					txtBuild.BorderBrush = Brushes.Red;
				else
					txtBuild.BorderBrush = Brushes.Black;

				if (string.IsNullOrWhiteSpace(txtName.Text))
					txtName.BorderBrush = Brushes.Red;
				else
					txtName.BorderBrush = Brushes.Black;

				if (string.IsNullOrWhiteSpace(txtArea.Text))
					txtArea.BorderBrush = Brushes.Red;
				else
					txtArea.BorderBrush = Brushes.Black;

				return;
			}

			if (int.TryParse(txtArea.Text, out int area))
			{
				if (area <= 0)
				{
					MessageBox.Show("Введите положительное число в поле 'Площадь'.");
					txtArea.BorderBrush = Brushes.Red;
					return;
				}
				else
				{
					txtArea.BorderBrush = Brushes.Black;
				}
			}

			DialogResult = true;
		}

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Закрытие окна приведет к потере данных. Желаете продолжить?", "Предупреждение", MessageBoxButton.YesNo);

			if (result == MessageBoxResult.Yes)
			{
				Close();
			}

		}

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			TextBox? textBox = sender as TextBox;

			if (textBox.Name == "txtIndex")
			{
				if (textBox.Text.Length == 0 && e.Text == "0")
				{
					MessageBox.Show("Введите число не начиная с нуля.");
					e.Handled = true;
				}
				else if (!char.IsDigit(e.Text, 0) || textBox.Text.Length == 6)
				{
					MessageBox.Show("Введите шестизначное число.");
					e.Handled = true;
				}
			}

			if (textBox.Name == "txtCity")
			{
				if (char.IsDigit(e.Text, 0))
				{
					MessageBox.Show("Вводите только текст.");
					e.Handled = true;
				}
			}

			if (textBox.Name == "txtArea")
			{
				if (!char.IsDigit(e.Text, 0) && e.Text != "." && e.Text != "")
				{
					MessageBox.Show("Введены недопустимые символы или цифры!");
					e.Handled = true;
				}
				else
				{
					string text = textBox.Text.Insert(textBox.CaretIndex, e.Text);
					string[] parts = text.Split('.');

					if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length > 2) || (parts.Length == 1 && parts[0].Length > 3))
					{
						MessageBox.Show("Вводите не более трех цифр до точки, и не более двух после точки");
						e.Handled = true;
					}

					if (text.StartsWith("0"))
					{
						MessageBox.Show("Число не может начинаться с нуля!");
						e.Handled = true;
					}
				}
			}

		}

	}
}
