using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Hotels;
using Selu383.SP24.Api.Features.Users;
using System.Security.Claims;
using Selu383.SP24.Api.Controllers;
using System.Data;

namespace Selu383.SP24.Api.Controllers;

[Route("api/hotels")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly DbSet<Hotel> hotels;
    private readonly DataContext dataContext;
    private readonly UserManager<User> userManager;


    public HotelsController(DataContext dataContext, UserManager<User> userManager)
    {
        this.dataContext = dataContext;
        hotels = dataContext.Set<Hotel>();
        this.userManager = userManager;

    }

    [HttpGet]
    public IQueryable<HotelDto> GetAllHotels()
    {
        return GetHotelDtos(hotels);
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<HotelDto> GetHotelById(int id)
    {
        var result = GetHotelDtos(hotels.Where(x => x.Id == id)).FirstOrDefault();
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
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
        hotels.Add(hotel);

        dataContext.SaveChanges();

        dto.Id = hotel.Id;

        return CreatedAtAction(nameof(GetHotelById), new { id = dto.Id }, dto);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize]
    public async Task<ActionResult<HotelDto>> UpdateHotelAsync(int id, HotelDto dto)
    {
        if (IsInvalid(dto))
        {
            return BadRequest();
        }

        var hotel = hotels.FirstOrDefault(x => x.Id == id);
        if (hotel == null)
        {
            return NotFound();
        }

        var user = User.Identity.Name;

        var userdto = await AuthenticationController.GetDto(userManager.Users).SingleAsync(x => x.UserName == user);

        if (!User.IsInRole("Admin") && userdto.Id != hotel.ManagerId )
        {
            return Forbid();
        }
        

        

        hotel.Name = dto.Name;
        hotel.Address = dto.Address;
        hotel.ManagerId = dto.ManagerId;

        dataContext.SaveChanges();

        dto.Id = hotel.Id;

        return Ok(dto);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult DeleteHotel(int id)
    {
        var hotel = hotels.FirstOrDefault(x => x.Id == id);
        if (hotel == null)
        {
            return NotFound();
        }

        hotels.Remove(hotel);

        dataContext.SaveChanges();

        return Ok();
    }

    private static bool IsInvalid(HotelDto dto)
    {
        return string.IsNullOrWhiteSpace(dto.Name) ||
               dto.Name.Length > 120 ||
               string.IsNullOrWhiteSpace(dto.Address);
    }

    private static IQueryable<HotelDto> GetHotelDtos(IQueryable<Hotel> hotels)
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




