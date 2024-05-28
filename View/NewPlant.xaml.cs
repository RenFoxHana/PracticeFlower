using Practice.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Practice.View
{
	public partial class NewPlant : Window
	{
		private PracticeContext db = new PracticeContext();
		public ObservableCollection<ShopsForSale> ListShop { get; set; }
		public ObservableCollection<Florist> ListFlorist { get; set; }
		public NewPlant()
		{
			InitializeComponent();
			if (App.currentUser.IdRole == 1)
			{
				ListShop = new ObservableCollection<ShopsForSale>(db.ShopsForSales.ToList());
				cmbShop.ItemsSource = ListShop;
				cmbShop.DisplayMemberPath = "NameOfShop";
				cmbShop.SelectionChanged += CmbShop_SelectionChanged;
			}
			else
			{
				cmbShop.Visibility = Visibility.Collapsed;
				cmbFlorist.Visibility = Visibility.Collapsed;
				textFlorist.Visibility = Visibility.Collapsed;
				textShop.Visibility = Visibility.Collapsed;
				this.Height = 455;
				Grid.SetRow(btSave, 8);
				Grid.SetRow(btClose, 8);
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			if (App.currentUser.IdRole == 1)
			{
				if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtColor.Text) ||
				string.IsNullOrWhiteSpace(txtSoil.Text) || string.IsNullOrWhiteSpace(txtFlowering.Text) ||
				string.IsNullOrWhiteSpace(txtShape.Text) || string.IsNullOrWhiteSpace(txtSize.Text) ||
				string.IsNullOrWhiteSpace(txtLife.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) ||
				cmbFlorist.SelectedItem == null || cmbShop.SelectedItem == null)
				{
					MessageBox.Show("Заполните все поля!");

					if (string.IsNullOrWhiteSpace(txtName.Text))
						txtName.BorderBrush = Brushes.Red;
					else
						txtName.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtColor.Text))
						txtColor.BorderBrush = Brushes.Red;
					else
						txtColor.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtSoil.Text))
						txtSoil.BorderBrush = Brushes.Red;
					else
						txtSoil.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtFlowering.Text))
						txtFlowering.BorderBrush = Brushes.Red;
					else
						txtFlowering.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtShape.Text))
						txtShape.BorderBrush = Brushes.Red;
					else
						txtShape.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtSize.Text))
						txtSize.BorderBrush = Brushes.Red;
					else
						txtSize.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtLife.Text))
						txtLife.BorderBrush = Brushes.Red;
					else
						txtLife.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtPrice.Text))
						txtPrice.BorderBrush = Brushes.Red;
					else
						txtPrice.BorderBrush = Brushes.Black;

					return;
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtColor.Text) ||
				string.IsNullOrWhiteSpace(txtSoil.Text) || string.IsNullOrWhiteSpace(txtFlowering.Text) ||
				string.IsNullOrWhiteSpace(txtShape.Text) || string.IsNullOrWhiteSpace(txtSize.Text) ||
				string.IsNullOrWhiteSpace(txtLife.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
				{
					MessageBox.Show("Заполните все поля!");

					if (string.IsNullOrWhiteSpace(txtName.Text))
						txtName.BorderBrush = Brushes.Red;
					else
						txtName.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtColor.Text))
						txtColor.BorderBrush = Brushes.Red;
					else
						txtColor.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtSoil.Text))
						txtSoil.BorderBrush = Brushes.Red;
					else
						txtSoil.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtFlowering.Text))
						txtFlowering.BorderBrush = Brushes.Red;
					else
						txtFlowering.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtShape.Text))
						txtShape.BorderBrush = Brushes.Red;
					else
						txtShape.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtSize.Text))
						txtSize.BorderBrush = Brushes.Red;
					else
						txtSize.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtLife.Text))
						txtLife.BorderBrush = Brushes.Red;
					else
						txtLife.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtPrice.Text))
						txtPrice.BorderBrush = Brushes.Red;
					else
						txtPrice.BorderBrush = Brushes.Black;

					return;
				}
			}
			decimal price;
			if (decimal.TryParse(txtPrice.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price))
			{
				Plant plant = new Plant
				{
					Name = txtName.Text,
					Color = txtColor.Text,
					RequieredSoil = txtSoil.Text,
					FloweringPeriod = txtFlowering.Text,
					FlowerShape = txtShape.Text,
					FlowerSize = txtSize.Text,
					ShelfLife = txtLife.Text,

					Price = price,
				};

				db.Plants.Add(plant);
				db.SaveChanges();

				if (App.currentUser.IdRole == 1)
				{
					Florist selectedFlorist = (Florist)cmbFlorist.SelectedItem;
					var plantsAssortmentFlorist = new PlantsAssortmentFlorist
					{
						IdPlant = plant.IdPlant,
						IdFlorist = selectedFlorist.IdFlorist
					};

					db.PlantsAssortmentFlorists.Add(plantsAssortmentFlorist);
					db.SaveChanges();
				}
				else
				{
					Florist florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
					int floristId = florist.IdFlorist;
					var plantsAssortmentFlorist = new PlantsAssortmentFlorist
					{
						IdPlant = plant.IdPlant,
						IdFlorist = floristId,
					};
					db.PlantsAssortmentFlorists.Add(plantsAssortmentFlorist);
					db.SaveChanges();
				}

				MessageBox.Show("Новое растение успешно добавлено!");
				Close();
			}

		}

		private void CmbShop_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ShopsForSale selectedShop = (ShopsForSale)cmbShop.SelectedItem;

			if (selectedShop != null)
			{
				ListFlorist = new ObservableCollection<Florist>(db.Florists.Where(f => f.IdShop == selectedShop.IdShop).ToList());
			}
			else
			{
				ListFlorist = new ObservableCollection<Florist>();
			}

			cmbFlorist.ItemsSource = ListFlorist;
			cmbFlorist.DisplayMemberPath = "FName";
		}

		private Florist selectedFlorist;

		private void CmbFlorist_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedFlorist = (Florist)cmbFlorist.SelectedItem;
		}

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			TextBox? textBox = sender as TextBox;
			if (e.Text == "0" && textBox.Text.Length == 0)
			{
				MessageBox.Show("Введите число не начиная с нуля.");
				e.Handled = true;
			}
			else if (!char.IsDigit(e.Text, 0) && e.Text != ".")
			{
				MessageBox.Show("Вводите только числа через точку.");
				e.Handled = true;
			}
			else
			{
				string text = textBox.Text.Insert(textBox.CaretIndex, e.Text);
				string[] parts = text.Split('.');

				if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length > 2) || (parts.Length == 1 && parts[0].Length > 5))
				{
					MessageBox.Show("Вводите не более пять цифр до точки, и не более двух после.");
					e.Handled = true;
				}
			}
		}
	}
}

