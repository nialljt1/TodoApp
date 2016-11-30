using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string OrganiserForename { get; set; }

        [Required, StringLength(50)]
        public string OrganiserSurname { get; set; }

        [Required, StringLength(50)]
        public string OrganiserTelephoneNumber { get; set; }

        public int NumberOfDiners { get; set; }
    }
}
