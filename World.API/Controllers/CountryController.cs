using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using World.API.Data;
using World.API.DTO.Country;
using World.API.Models;

namespace World.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public CountryController(
            ApplicationDbContext dbContext, 
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<CountryDTO>> GetAll()
        {
            var countries = _dbContext.Countries.ToList();
            var countriesDto = _mapper.Map<List<CountryDTO>>(countries);
            if(countries == null)
            {
                return NoContent();
            }
            return Ok(countriesDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<CountryDTO> GetById(int id)
        {
            var country = _dbContext.Countries.Find(id);
            var countryDTO = _mapper.Map<CountryDTO>(country);
            if (country == null)
            {
                return NoContent();
            }
            return Ok(countryDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<CreateCountryDTO> Create([FromBody] CreateCountryDTO countryDTO)
        {
            var result = _dbContext.Countries.AsQueryable().Where(x => x.Name.ToLower() == countryDTO.Name.ToLower()).Any();
            if (result)
            {
                return Conflict("Country Already Exists in Database");
            }

            //Country country = new Country();
            //country.Name = countryDTO.Name;
            //country.ShortName = countryDTO.ShortName;
            //country.CountryCode = countryDTO.CountryCode;

            var country = _mapper.Map<Country>(countryDTO); 

            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();
            return CreatedAtAction("GetById", new { id = country.Id});
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Country> Update(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            if(countryDTO == null || id != countryDTO.Id)
            {
                return BadRequest();
            }

            //var countryFromDb = _dbContext.Countries.Find(id);

            //if (countryFromDb == null)
            //{
            //    return NotFound();
            //}

            //countryFromDb.Name = country.Name;
            //countryFromDb.ShortName = country.ShortName;
            //countryFromDb.CountryCode = country.CountryCode;

            var country = _mapper.Map<Country>(countryDTO);

            _dbContext.Countries.Update(country);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult DeleteById(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var country = _dbContext.Countries.Find(id);
            if(country == null)
            {
                return NotFound();
            }

            _dbContext.Remove(country);
            _dbContext.SaveChanges();
            return NoContent();
        }

    }
}
