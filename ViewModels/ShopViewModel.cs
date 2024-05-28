using Practice.Models;
using Practice.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Version3.Helper;

namespace Practice.ViewModels
{
	public class ShopViewModel
    {
		public ShopsForSale SelectedShop {  get; set; }
		private PracticeContext db = new PracticeContext();
		public ObservableCollection<ShopsForSale> ListShop { get; set; }

		public ShopViewModel()
		{
			ListShop = new ObservableCollection<ShopsForSale>(db.ShopsForSales.ToList());
		}

		private RelayCommand addShop;
		public RelayCommand AddShop
		{
			get
			{
				return addShop ??
				(addShop = new RelayCommand(obj =>
				{
					NewShop wnShop = new NewShop
					{
						Title = "Добавление магазина",
					};
					ShopsForSale shop = new ShopsForSale();
					wnShop.DataContext = shop;
					if (wnShop.ShowDialog() == true)
					{
						ListShop.Add(shop);
						db.ShopsForSales.Add(shop);
						db.SaveChanges();
						SelectedShop = shop;
					}
					else
					{
						SelectedShop = null;
					}
				}));
			}
		}

		private RelayCommand editShop;
		public RelayCommand EditShop
		{
			get
			{
				return editShop ??
				(editShop = new RelayCommand(obj =>
				{
					NewShop wnShop = new NewShop
					{
						Title = "Редактирование магазина",
					};
					ShopsForSale shop = SelectedShop;
					ShopsForSale tempshop = new ShopsForSale();
					tempshop = shop.ShallowCopy();
					wnShop.DataContext = tempshop;
					if (wnShop.ShowDialog() == true)
					{
						shop.NameOfShop = tempshop.NameOfShop;
						shop.Index = tempshop.Index;
						shop.City = tempshop.City;
						shop.Street = tempshop.Street;
						shop.Building = tempshop.Building;
						shop.AreaOfTradingFloor = tempshop.AreaOfTradingFloor;
						shop.TypeOfSale = tempshop.TypeOfSale;
						shop.AvailabilityOfOrders = tempshop.AvailabilityOfOrders;
						db.SaveChanges();
						OnPropertyChanged("SelectedShop");
						ICollectionView view = CollectionViewSource.GetDefaultView(ListShop);
						view.Refresh();
					}
				}, (obj) => SelectedShop != null && ListShop.Count > 0));
			}
		}

		private RelayCommand deleteShop;
		public RelayCommand DeleteShop
		{
			get
			{
				return deleteShop ??
				(deleteShop = new RelayCommand(obj =>
				{
					ShopsForSale shop = SelectedShop;

					bool hasReferences = db.Florists.Any(f => f.IdShop == shop.IdShop);

					if (hasReferences)
					{
						MessageBox.Show("Удаление магазина невозможно, так как существуют связанные записи в таблице флористов.");
					}
					else
					{
						MessageBoxResult result = MessageBox.Show("Удалить данные по магазину: " + shop.NameOfShop,
							"Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

						if (result == MessageBoxResult.OK)
						{
							ListShop.Remove(shop);
							db.ShopsForSales.Remove(shop);
							db.SaveChanges();
						}
					}
				}, (obj) => SelectedShop != null && ListShop.Count > 0));
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
