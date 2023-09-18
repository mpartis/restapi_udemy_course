using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    // Tha mporousa na valw kai api/[controller]
    // Me afto tha epairne to onoma tou controller class
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private ILogger<VillaAPIController> _logger;
        //private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbVilla;

        // dependency injection gia to logger kai to db, pleon kai gia to mapper
        public VillaAPIController(ILogger<VillaAPIController> logger, IVillaRepository dbVilla, IMapper mapper
            )
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapper = mapper;
        }

        // xwris async
        //[HttpGet]
        //public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        //{
        //    _logger.LogInformation("Get all villas");
        //    return Ok(_db.Villas.ToList()); // epistrefei oles tis villes apo to database
        //}

        // me async await kai Task<>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation("Get all villas");
            //return Ok(await _db.Villas.ToListAsync());
            //IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();

            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }



        [HttpGet("{id:int}", Name ="GetVilla")] // perimenei ena id parameter
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Get villa error with Id: " + id);
                return BadRequest();
            }

            // var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            //var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);

            if (villa == null)
            {
                return NotFound(); // to gnwsto 404
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody]VillaCreateDTO createDTO)
        {


            if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

            // to id edw afou valoume db den xreiazetai
            // villaDTO.Id = VillaStore.VillaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            //VillaStore.VillaList.Add(villaDTO);

            //Villa model = new()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft,
            //    CreatedDate = DateTime.Now
            //};

            Villa model = _mapper.Map<Villa>(createDTO);

            //await _db.Villas.AddAsync(model);
            // await _db.SaveChangesAsync();

            await _dbVilla.CreateAsync(model);

            return CreatedAtRoute("GetVilla", new { id = model.Id} ,model);
        }



        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            // vriskw thn villa me to id, to idio kai otan exw db
            var villa = await _dbVilla.GetAsync(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            //VillaStore.VillaList.Remove(villa);
             await _dbVilla.RemoveAsync(villa); // den exei async:(
            //await _db.SaveChangesAsync();

            // afto epistrefoume synhthws otan kanoume delete kati (204 status code)
            return NoContent();

        }


        // genika vazw IActionResult anti gia ActionResult otan kanw delete h update
        // giati den epistrefw kati kai den xreiazetai to ActionResult

        // epishs to PUT einai gia otan kanw update olo to object, alliws kanw PATCH

        [HttpPut("{id:int}", Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id) 
            {
                return BadRequest();
            }

            //var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            //villa.Name = villaDTO.Name;
            //villa.Occupancy = villaDTO.Occupancy;
            //villa.Sqft = villaDTO.Sqft;

            //Villa model = new()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id = villaDTO.Id,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft,
            //    UpdatedDate = DateTime.Now
            //};

            Villa model = _mapper.Map<Villa>(updateDTO);

            // mono afto xreiazetai
            //_db.Villas.Update(model); // oute afto exei async
            //await _db.SaveChangesAsync();
            await _dbVilla.UpdateAsync(model);

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            // Otan vriskw mia villa me to id sthn ousia to Entity kanei track afto to id
            // Kathe id mporei na to kanei track mono mia fora, den mporei na exei 2 villes
            // me to idio id kai na tis kanei track taftoxrona
            // opote pio katw otan pame na kanoume Update() varaei error
            // gia afto edw vazoume AsNoTracking(), opote apla kanoume
            // thn anakthsh xwris omws na kanei track to Entity to id meta
            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            //VillaUpdateDTO villaDTO = new()
            //{
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //    Id = villa.Id,
            //    ImageUrl = villa.ImageUrl,
            //    Name = villa.Name,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft
            //};

            if (villa == null)
            {
                return NotFound();
            }

            patchDTO.ApplyTo(villaDTO, ModelState); // ta errors pou mporei na prokypsoun mpainoun sto model state

            Villa model = _mapper.Map<Villa>(villaDTO);

            //Villa model = new()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id = villaDTO.Id,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft
            //};

            //_db.Villas.Update(model);
            //await _db.SaveChangesAsync();

            await _dbVilla.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();

        }



    }
}
