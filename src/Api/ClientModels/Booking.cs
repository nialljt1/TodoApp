using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ClientModels
{
    public class ClientBooking
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string BookingDate { get; set; }
        public string BookingTime { get; set; }
    }
}
