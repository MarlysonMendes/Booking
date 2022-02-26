using CwkBooking.Dal;
using CwkBooking.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CwkBooking.Api.Controllers
{

    //CRUD
    //Create
    //Read - get all, get by id
    //Update
    //Delete
    // /hotels
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : Controller
    {
        private readonly ILogger<HotelsController> _logger;
        private readonly HttpContext _http;
        private readonly DataContext _ctx;
        public HotelsController(ILogger<HotelsController> logger, IHttpContextAccessor httpContextAccessor,
            DataContext ctx)
        {
            _logger = logger;
            _http = httpContextAccessor.HttpContext;
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _ctx.Hotels.ToListAsync();
            return Ok(hotels);
        }


        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await _ctx.Hotels.FirstOrDefaultAsync(h => h.HotelId == id);
            return Ok(hotel);
        }

        [HttpPost]
        public async Task <IActionResult> CreateHotel([FromBody] Hotel hotel)
        {
            _ctx.Hotels.Add(hotel);   //save in memory
            await _ctx.SaveChangesAsync();  // save DataBase
            return CreatedAtAction(nameof(GetHotelById), new {id = hotel.HotelId}, hotel);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateHotel([FromBody] Hotel updated, int id)
        {
            var hotel = await _ctx.Hotels.FirstOrDefaultAsync(h =>h.HotelId == id);
            hotel.Stars = updated.Stars;
            hotel.Name = updated.Name;
            hotel.Description = updated.Description;
            
            _ctx.Hotels.Update(hotel);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _ctx.Hotels.FirstOrDefaultAsync(h => h.HotelId == id);
            _ctx.Hotels.Remove(hotel);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}