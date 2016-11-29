using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public class Booking
    {
        public int Id { get; set; }

        public int OrganiserUserId { get; set; }

        [Required]
        public DateTime BookingTime { get; set; }

        [Required]
        public int NumberOfDiners { get; set; }

        [Required, StringLength(50)]
        public string OrganiserForename { get; set; }

        [Required, StringLength(50)]
        public string OrganiserSurname { get; set; }

        [Required, StringLength(50)]
        public string OrganiserTelephoneNumber { get; set; }
    }
}
