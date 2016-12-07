using System;
using System.Collections.Generic;
using System.Linq;
using Api.Models;
using Api.ClientModels;

namespace Api.Data
{
    public class BookingsRepository : IBookingsRepository
    {
        private readonly AppContext _appContext;
        public BookingsRepository(
            AppContext appContext)
        {
            _appContext = appContext;
        }

        public int AddBooking(ClientBooking clientBooking)
        {
            var booking = new Booking();
            booking.OrganiserForename = clientBooking.FirstName;
            booking.OrganiserSurname = clientBooking.Surname;
            booking.OrganiserTelephoneNumber = clientBooking.TelephoneNumber;
            booking.OrganiserEmailAddress = clientBooking.EmailAddress;
            booking.StartingAt = clientBooking.StartingAt;
            booking.NumberOfDiners = clientBooking.NumberOfDiners;
            booking.CreatedById = "0d20e665-2859-418e-ae12-bece795627df";
            booking.LastUpdatedById = "0d20e665-2859-418e-ae12-bece795627df";
            booking.LastUpdatedAt = DateTimeOffset.Now;
            booking.CreatedAt = DateTimeOffset.Now;
            _appContext.Bookings.Add(booking);
            _appContext.SaveChanges();
            return booking.Id;
        }

        public Booking GetBookingById(int id)
        {
            return _appContext.Bookings.Find(id);
        }
    }
}