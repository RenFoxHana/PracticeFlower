using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Practice.Models;
using System.Windows;

namespace Practice
{
	public partial class App : Application
	{
		public static User currentUser = null;
		private readonly IServiceProvider _serviceProvider;
		public App()
		{
			ServiceCollection services = new ServiceCollection();
			ConfigureServices(services);
			_serviceProvider = services.BuildServiceProvider();
		}

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<PracticeContext>(options =>
			options.UseSqlServer("Data Source=NATBOK\\MSSQLSERVER2;Initial Catalog=Practice;Integrated Security=True;Encrypt=False;"));

			services.AddSingleton<Autorization>();
		}
		protected override void OnStartup(StartupEventArgs e)
		{
		}

	}

}
