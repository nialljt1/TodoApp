using System.Collections.Generic;
using Api.Models;
using Api.ClientModels;

namespace Api.Data
{
    public interface IBookingsRepository
    {
        int AddBooking(ClientBooking booking);
        Booking GetBookingById(int id);
        IList<ClientBooking> FilterBookings(int restaurantId, FilterCriteria filterCriteria);
    }
}