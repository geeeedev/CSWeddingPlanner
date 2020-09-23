

using System;

namespace CSWeddingPlanner.Models
{
    public class RSVP
    {
        public int RSVPid { get; set; }

        public bool IsAttending { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        public int UserId { get; set; }
        public User Attendee { get; set; }


        public int EventId { get; set; }
        public Event TheEvent { get; set; }
    }
}