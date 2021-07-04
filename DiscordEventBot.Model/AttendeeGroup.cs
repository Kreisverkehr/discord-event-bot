using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Model
{
    public class AttendeeGroup
    {
        [Key]
        public Guid GroupID { get; set; }
        public string Name { get; set; }
        public int? MaxCapacity { get; set; }
        public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}
