using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountriesApi.Model.Models;
using CountriesApi.Api.DTOs;
using Microsoft.AspNetCore.OData.Query;

namespace CountriesApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableQuery]
    public class CountriesController : ControllerBase
    {
        private readonly CountriesDbContext _context;

        public CountriesController(CountriesDbContext context)
        {
            _context = context;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
        {
          if (_context.Countries == null)
              return NotFound();
   
          var countries = await _context.Countries.ToListAsync();

          if (countries == null || countries.Count == 0)
                return NotFound();

          var countryDtos = countries.Select(country => new CountryDto
            {
                Id = country.Id,
                FullName = country.FullName,
                ShortName = country.ShortName
            }).ToList();

            return countryDtos;
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryIncludesCitiesDto>> GetCountry(int id)
        {
          if (_context.Countries == null)
          {
              return NotFound();
          }
            //var country = await _context.Countries.FindAsync(id);
            var country = await _context.Countries
                .Include(c => c.Cities ) // Eagerly load the Cities collection
                .FirstOrDefaultAsync(c => c.Id == id);

            if (country == null)
            {
                return NotFound();
            }

            var countryDto = new CountryIncludesCitiesDto
            {
                Id = country.Id,
                FullName = country.FullName,
                ShortName = country.ShortName,
                Cities = country.Cities.Select(city => new CityWithoutCountryDto
                {
                    Id = city.Id,
                    Name = city.Name
                }).ToList()
            };

            return countryDto;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
          if (_context.Countries == null)
          {
              return Problem("Entity set 'CountriesDbContext.Countries'  is null.");
          }
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
