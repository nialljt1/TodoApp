using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Api.Data;
using Api.Models;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Api.ClientModels;
using System.Linq;

namespace Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        public IBookingsRepository Repo { get; set; }

        public BookingsController([FromServices] IBookingsRepository repo)
        {
            Repo = repo;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ClientBooking booking)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var bookingId = Repo.AddBooking(booking);
                var url = Url.RouteUrl("GetBookingByIdRoute", new { id = bookingId }, Request.Scheme,
                    Request.Host.ToUriComponent());
                return Created(url, booking);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET api/bookings/2
        [HttpGet("{id}")]
        [Route("{id}", Name = "GetBookingByIdRoute")]
        public Booking Get(int id)
        {
            return Repo.GetBookingById(id);
        }
    }
}