using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSWeddingPlanner.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Display(Name="First Name")]
        [Required(ErrorMessage=" is required."),MinLength(2,ErrorMessage=" requires a min of {1} characters.")]
        public string First { get; set; }


        [Display(Name="Last Name")]
        [Required(ErrorMessage=" is required."),MinLength(2,ErrorMessage=" requires a min of {1} characters.")]
        public string Last { get; set; }


        [Display(Name="Email Address")]
        [Required(ErrorMessage=" is required."),EmailAddress(ErrorMessage=" is not in correct format.")]
        public string Email { get; set; }   


        [Required(ErrorMessage=" is required."),MinLength(8,ErrorMessage=" requires a min of {1} characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        [NotMapped]
        [Display(Name="Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPswd { get; set; }


        public String FullName()
        {
            return $"{First} {Last}";
        }
        

        public List<Event> PlannedEvents { get; set; }
        //1 User can plan M Events.  
        //ea Event is planned by 1 User.

        public List<RSVP> AttendingRSVPs { get; set; }
        //1 User can RSVP to M Events.
        //ea RSVP is for 1 User.

    }
}


