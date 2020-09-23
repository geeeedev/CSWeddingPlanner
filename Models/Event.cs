using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSWeddingPlanner.Validations;



namespace CSWeddingPlanner.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }


        [Required(ErrorMessage=" is required.")]
        [FutureDateOnly]        //Custom Validation
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }



        [Required(ErrorMessage=" is required."),MinLength(2,ErrorMessage=" requires a min of {1} characters.")]
        [Display(Name="Wedder One")]
        public string WedderOne { get; set; }

        [Required(ErrorMessage=" is required."),MinLength(2,ErrorMessage=" requires a min of {1} characters.")]
        [Display(Name="Wedder Two")]
        public string WedderTwo { get; set; }


        
        


        [Display(Name="Wedding Address")]
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public String WeddingName()
        {
            return $"{WedderOne} & {WedderTwo}";
        }



        public int UserId { get; set; }
        public User Planner { get; set; }


        public List<RSVP> EventRSVPs { get; set; }



    }
}

// add validations
// add not mapped