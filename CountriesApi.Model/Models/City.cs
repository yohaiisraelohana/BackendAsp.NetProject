using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CountriesApi.Model.Models
{
	public class City
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
		[JsonIgnore]
        public Country Country { get; set; }
	}
}