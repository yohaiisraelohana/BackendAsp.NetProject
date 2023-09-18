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
    public class CitiesController : ControllerBase
    {
        private readonly CountriesDbContext _context;

        public CitiesController(CountriesDbContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
        {
            if (_context.Cities == null)
              return NotFound();
  
            var cities = await _context.Cities
                      .Include(c => c.Country)
                      .Select(City => new CityDto
                      {
                          Id = City.Id,
                          Name = City.Name,
                          CountryId = City.CountryId,
                          Country = new CountryDto
                          {
                              Id = City.Country.Id,
                              FullName = City.Country.FullName,
                              ShortName = City.Country.ShortName
                          }
                      })
                      .ToListAsync();

            if (cities == null || cities.Count == 0)
                return NotFound();

            return cities;
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetCity(int id)
        {
          if (_context.Cities == null)
          {
              return NotFound();
          }
            var city = await _context.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            var cityDto = new CityDto
            {
                Id = city.Id,
                Name = city.Name,
                Country = new CountryDto
                {
                    Id = city.Country.Id,
                    FullName = city.Country.FullName,
                    ShortName = city.Country.ShortName
                }
            };

            return cityDto;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int id, City city)
        {
            if (id != city.Id)
            {
                return BadRequest();
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
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

        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
          if (_context.Cities == null)
          {
              return Problem("Entity set 'CountriesDbContext.Cities'  is null.");
          }
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.Id }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            if (_context.Cities == null)
            {
                return NotFound();
            }
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(int id)
        {
            return (_context.Cities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
