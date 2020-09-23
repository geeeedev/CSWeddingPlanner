using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSWeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace CSWeddingPlanner.Controllers      //needed in every controller?
{   

public class EventsController : Controller
    {
        private dbWeddingPlannerContext db;
        private int? currUid
        {
            get
            {   //make accessible to all other actions/methods for null checks
                return HttpContext.Session.GetInt32("currUid");
            }
        }
        public EventsController(dbWeddingPlannerContext dbContext)
        {
            db = dbContext;
        }



        [HttpGet("/Dashboard")]
        public IActionResult Dashboard()
        {
            if(currUid == null)
            {
                return RedirectToAction("Index","Home");        
            }

            List<Event> upcomingEvents = db.Events
                        .Include(e=>e.EventRSVPs)
                        .OrderByDescending(e=>e.EventId).ToList();
            return View(upcomingEvents);
        }

        [HttpGet("/Events/New")]
        public IActionResult New()
        {
            if(currUid == null)
            {
                return RedirectToAction("Index","Home");        
            }
            return View();
        }

        [HttpPost("/Events/Create")]
        public IActionResult Create(Event newWedding)
        {
            if(ModelState.IsValid)
            {
                if(db.Events.Any(e=>e.WedderOne == newWedding.WedderOne && e.WedderTwo == newWedding.WedderTwo))
                {
                    ModelState.AddModelError("WedderOne","These Wedding Names are taken.");
                };
            }

            if(ModelState.IsValid == false)
            {
                return View("New");
            }

            newWedding.UserId = (int)currUid;
            db.Events.Add(newWedding);
            db.SaveChanges();
            return RedirectToAction("Details", new{eventId = newWedding.EventId});
        }


        [HttpPost("/Events/{eventId}/Delete")]    
        public IActionResult Delete(int eventId)
        {
            Event deletedEvent = db.Events
                    .FirstOrDefault(e => e.EventId == eventId && e.UserId == currUid);
            if(deletedEvent == null)
            {
                return RedirectToAction("Index","Home");
            }
            db.Events.Remove(deletedEvent);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }


        [HttpGet("/Events/{eventId}")]
        public IActionResult Details(int eventId)     
        {
            Event selectedEvent = db.Events
                    .Include(e => e.Planner)
                    .Include(e => e.EventRSVPs)          //list of RSVPs for this event
                    .ThenInclude(rsvp => rsvp.Attendee)  //the Attendee OBJ for each RSVP
                    .FirstOrDefault(e => e.EventId == eventId)
                    ;
            
            if(selectedEvent == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(selectedEvent);
        }

        [HttpGet("/Events/{eventId}/Edit")]
        public IActionResult Edit(int eventId)
        {
            Event edittingEvent = db.Events
                    .FirstOrDefault(e=> e.EventId == eventId);
            
            if(edittingEvent == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(edittingEvent);
        }

        [HttpPost("/Events/{eventId}/Update")]
        public IActionResult Update(Event updatedEvent)     //do I need , int eventId here
        {
            if(ModelState.IsValid)
            {
                if(db.Events.Any(e=>e.WedderOne == updatedEvent.WedderOne && e.WedderTwo == updatedEvent.WedderTwo && e.EventId != updatedEvent.EventId))
                {
                    ModelState.AddModelError("WedderOne","These Wedding Names are taken.");
                };
            }

            if (ModelState.IsValid == false)
            {
                return View("Edit",updatedEvent);       //don't forget the obj
            }

            Event dbEvent = db.Events.FirstOrDefault(e=>e.EventId == updatedEvent.EventId);
            if(dbEvent == null)
            {
                return RedirectToAction("Index","Home");
            }

            dbEvent.WedderOne = updatedEvent.WedderOne;
            dbEvent.WedderTwo = updatedEvent.WedderTwo;
            dbEvent.Date = updatedEvent.Date;
            dbEvent.Address = updatedEvent.Address;
            dbEvent.UpdatedAt = updatedEvent.UpdatedAt;
            db.Events.Update(dbEvent);
            db.SaveChanges();
            return RedirectToAction("Details", new{eventId = dbEvent.EventId});
        }


        [HttpPost("/Events/{eventId}/RSVP")]
        public IActionResult RSVP(int eventId, bool isAttending)
        {
            RSVP existingRSVP = db.RSVPs.FirstOrDefault(r=>r.EventId==eventId && r.UserId == currUid);
            if(existingRSVP != null)
            {
                existingRSVP.IsAttending = isAttending;
                db.RSVPs.Update(existingRSVP);
            }
            else // null-not exist
            {
                RSVP newRSVP = new RSVP()
                {
                    UserId = (int)currUid,
                    EventId = eventId,
                    IsAttending = isAttending
                };
                db.RSVPs.Add(newRSVP);
            }

            db.SaveChanges();
            return RedirectToAction("Dashboard");

        }




        // fix - asp-route-XXXid -  WILL BLOW UP
        // EventId
        // eventId
        // evtId
        // id
        // custom validationssss !!!
        // null checks or any checks


    }

}
    // //Count on IEnum<> needs () - .Count()
    // IEnumerable<RSVP> ienumRSVP = db.RSVPs.OrderByDescending(r=>r.UserId);
    // int IEnumCountwithParan = ienumRSVP.Count();
    // //Count on List<> does NOT need () - .Count
    // List<RSVP> listRSVP = db.RSVPs.OrderByDescending(r=>r.UserId).ToList();
    // int ListCountwithOUTparan = listRSVP.Count;