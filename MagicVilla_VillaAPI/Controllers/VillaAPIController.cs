using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    // Tha mporousa na valw kai api/[controller]
    // Me afto tha epairne to onoma tou controller class
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private ILogger<VillaAPIController> _logger;

        // dependency injection gia to logger
        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Get all villas");
            return Ok(VillaStore.VillaList);
        }

        [HttpGet("{id:int}", Name ="GetVilla")] // perimenei ena id parameter
        //[ProducesResponseType(200, Type = typeof(VillaDTO))]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Get villa error with Id: " + id);
                return BadRequest();
            }

            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound(); // to gnwsto 404
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {


            if (VillaStore.VillaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDTO.Id = VillaStore.VillaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.VillaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id} ,villaDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            // vriskw thn villa me to id
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            VillaStore.VillaList.Remove(villa);

            // afto epistrefoume synhthws otan kanoume delete kati (204 status code)
            return NoContent();

        }


        // genika vazw IActionResult anti gia ActionResult otan kanw delete h update
        // giati den epistrefw kati kai den xreiazetai to ActionResult

        // epishs to PUT einai gia otan kanw update olo to object, alliws kanw PATCH

        [HttpPut("{id:int}", Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id) 
            {
                return BadRequest();
            }

            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            patchDTO.ApplyTo(villa, ModelState); // ta errors pou mporei na prokypsoun mpainoun sto model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();

        }



    }
}
