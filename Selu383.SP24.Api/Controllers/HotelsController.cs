using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Hotels;
using System;
using System.Linq;

namespace Selu383.SP24.Api.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    
    public class HotelsController : ControllerBase
    {
        private readonly DbSet<Hotel> _hotels;
        private readonly DataContext _dataContext;

        public HotelsController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _hotels = dataContext.Set<Hotel>();
        }

        [HttpGet]
        public IQueryable<HotelDto> GetAllHotels()
        {
            return GetHotelDtos(_hotels);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<HotelDto> GetHotelById(int id)
        {
            var result = GetHotelDtos(_hotels.Where(x => x.Id == id)).FirstOrDefault();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<HotelDto> CreateHotel(HotelDto dto)
        {
            if (IsInvalid(dto))
            {
                return BadRequest();
            }

            var hotel = new Hotel
            {
                Name = dto.Name,
                Address = dto.Address,
                ManagerId = dto.ManagerId,
            };
            _hotels.Add(hotel);

            _dataContext.SaveChanges();

            dto.Id = hotel.Id;

            return CreatedAtAction(nameof(GetHotelById), new { id = dto.Id }, dto);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<HotelDto> UpdateHotel(int id, HotelDto dto)
        {
            var hotel = _hotels.FirstOrDefault(x => x.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            if (IsInvalid(dto))
            {
                return BadRequest();
            }

            hotel.Name = dto.Name;
            hotel.Address = dto.Address;
            hotel.ManagerId = dto.ManagerId;

            _dataContext.SaveChanges();

            return Ok(dto);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteHotel(int id)
        {
            var hotel = _hotels.FirstOrDefault(x => x.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            _hotels.Remove(hotel);
            _dataContext.SaveChanges();

            return Ok();
        }

       
        private bool IsInvalid(HotelDto dto)
        {
            return string.IsNullOrWhiteSpace(dto.Name) ||
                   dto.Name.Length > 120 ||
                   string.IsNullOrWhiteSpace(dto.Address);
        }

        private IQueryable<HotelDto> GetHotelDtos(IQueryable<Hotel> hotels)
        {
            return hotels
                .Select(x => new HotelDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    ManagerId = x.ManagerId,
                });
        }
    }
}
