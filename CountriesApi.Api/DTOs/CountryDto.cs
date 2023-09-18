using System;
namespace CountriesApi.Api.DTOs
{
	public class CountryDto
	{
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
    }
    public class CountryIncludesCitiesDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public List<CityWithoutCountryDto> Cities { get; set; }
    }
}

