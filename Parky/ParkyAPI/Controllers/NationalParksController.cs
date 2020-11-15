using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParksController(INationalParkRepository npRepo,IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// Get National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParks = _npRepo.GetNationalParks();
            var parkDto = new List<NationalParkDto>();
            foreach(var obj in nationalParks)
            {
                parkDto.Add(_mapper.Map<NationalParkDto>(obj));
            }
            return Ok(parkDto);
        }
        /// <summary>
        /// Get National Park
        /// </summary>
        /// <param name="nationalParkId">The ID of the National Park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}",Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _npRepo.GetNationalPark(nationalParkId);
            if (nationalPark == null)
            {
                return NotFound();
            }
            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);
            return Ok(nationalParkDto);
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto npDto)
        {
            if (npDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_npRepo.NationalParkExists(npDto.Name))
            {
                ModelState.AddModelError("", "NationalPark Already Exists");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var npObj = _mapper.Map<NationalPark>(npDto);
            if (!_npRepo.CreateNationalPark(npObj))
            {
                ModelState.AddModelError("", $"Something went wrong while creating {npObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { npId = npObj.Id }, npObj);
        }
        [HttpPatch("{nationalParkId:int}", Name ="UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, NationalParkDto npDto)
        {
            if (npDto == null || nationalParkId != npDto.Id)
            {
                return BadRequest(ModelState);
            }
            var npObj = _mapper.Map<NationalPark>(npDto);
            if (!_npRepo.UpdateNationalPark(npObj))
            {
                ModelState.AddModelError("", $"Error while updating the {npObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{nationalParkId:int}", Name ="DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }
            var npObj = _npRepo.GetNationalPark(nationalParkId);
            if (!_npRepo.DeleteNationalPark(npObj))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(404, ModelState);
            }
            return NoContent();
        }
    }
}
