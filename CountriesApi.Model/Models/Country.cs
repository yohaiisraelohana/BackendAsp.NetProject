using System.ComponentModel.DataAnnotations;
namespace CountriesApi.Model.Models
{
	public class Country
	{
		[Key]
		public int Id { get; set; }

		public string FullName { get; set; }
		
		public string ShortName { get; set; }

		public virtual IList<City> Cities { get; set; }

	}
}

