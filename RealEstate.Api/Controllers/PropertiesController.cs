using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.Repositories;
using RealEstate.Api.Domain;
using RealEstate.Api.Dtos; 

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyRepository _repo;
        private readonly ILogger<PropertiesController> _logger;

        public PropertiesController(IPropertyRepository repo, ILogger<PropertiesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get( 
            [FromQuery] string? name,
            [FromQuery] string? address,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var properties = await _repo.GetPropertiesAsync(name, address, minPrice, maxPrice, page, pageSize);
                
                var propertiesDto = properties.Select(p => new PropertyDto
                {
                    Id = p.Id,
                    IdOwner = p.IdOwner,
                    Name = p.Name,
                    Address = p.Address,
                    Price = p.Price,
                    ImageUrl = p.Image?.File 
                }).ToList();
                
                return Ok(propertiesDto); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching properties");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id)
        {
            var property = await _repo.GetByIdAsync(id);
            if (property == null) return NotFound();

            var propertyDto = new PropertyDto
            {
                Id = property.Id,
                IdOwner = property.IdOwner,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                ImageUrl = property.Image?.File
            };
            
            return Ok(propertyDto); 
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Property property)
        {
            
            await _repo.CreateAsync(property);

            var propertyDto = new PropertyDto
            {
                Id = property.Id,
                IdOwner = property.IdOwner,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                ImageUrl = property.Image?.File
            };

            return CreatedAtAction(nameof(GetById), new { id = property.Id }, propertyDto);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> CreateBatch([FromBody] List<Property> properties)
        {
            try
            {
               if (properties == null || properties.Count == 0)
                return BadRequest("No hay propiedades para agregar");

            await _repo.CreateManyAsync(properties);
            return Ok(new { count = properties.Count });   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding batch properties");
                return StatusCode(500, "An error occurred");
            }    
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Property property)
        {
            var exists = await _repo.GetByIdAsync(id);
            if (exists == null) return NotFound();
            property.Id = id;
            await _repo.UpdateAsync(id, property);
            return NoContent();
        }

        [HttpPut("{id:length(24)}/image")]
        public async Task<IActionResult> UpdateImage(string id, [FromBody] PropertyImage image)
        {
            var exists = await _repo.GetByIdAsync(id);
            if (exists == null) return NotFound();
            
            await _repo.AddImageAsync(id, image); 
            return NoContent();
        }


        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var exists = await _repo.GetByIdAsync(id);
            if (exists == null) return NotFound();
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}