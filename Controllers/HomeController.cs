using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSWeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace CSWeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private dbWeddingPlannerContext db;
        public HomeController(dbWeddingPlannerContext dbContext)
        {
            db = dbContext;
        }



        [HttpGet("")]
        public IActionResult Index()
        {
            int? currUid = HttpContext.Session.GetInt32("currUid");
            if(currUid != null)
            {
                return RedirectToAction("Dashboard","Events");
            }
            return View();
        }
        
        // [HttpPost("/_Register")]                     //turned off, route is specified as: http://localhost:5000/Home/Register
        public IActionResult Register(User newUser)     //specify model to specify which validation to run
        {
            if(ModelState.IsValid)                      //initial data entry checks out
            {
                //Email already exists?  
                if(db.Users.Any(u=>u.Email == newUser.Email))
                {
                    //exists - error, send to ModalState == false
                    ModelState.AddModelError("Email"," already exists.");
                }
            }

            if(ModelState.IsValid == false)             //activate validation checks - data entry (2nd round)
            {
                return View("Index");                   //vs. RedirectToAction("Index)
            }

            //new email - good -keep going
            //hash password
            //create save to db
            //retrieve and save id to session
            PasswordHasher<User> pHasher = new PasswordHasher<User>();
            newUser.Password = pHasher.HashPassword(newUser, newUser.Password);

            db.Users.Add(newUser);
            db.SaveChanges();

            HttpContext.Session.SetInt32("currUid",newUser.UserId);
            HttpContext.Session.SetString("currUname",newUser.First);
            return RedirectToAction("Dashboard","Events");
            // return RedirectToAction("Success");

        }

        [HttpPost("/_Login")]                       //route is specified as: http://localhost:5000/_Login
        public IActionResult Login(LoginUser loggedInUser)
        {
            //Email already exist?
            //password unhash and matching?
            //Fail: sent to ModelState with error
            //Pass: keep going 
            int? dbUid = null;          //nullable - could I just make it int ... = 0; as long as I don't id anyone as 0?
            string dbUname = "";
            if(ModelState.IsValid)      //initial entry checks out
            {
                string loginFailed = "Invalid Email/Password!";
                User dbUser = db.Users.FirstOrDefault(u => u.Email == loggedInUser.LoginEmail);     //check user exists
                if (dbUser == null)
                {
                    ModelState.AddModelError("LoginEmail", loginFailed);
                }
                else
                {
                    dbUid = dbUser.UserId;
                    dbUname = dbUser.First;
                    PasswordHasher<LoginUser> phasher = new PasswordHasher<LoginUser>();
                    PasswordVerificationResult compResult = phasher.VerifyHashedPassword(loggedInUser, dbUser.Password, loggedInUser.LoginPassword);
                    if (compResult == 0)        // 0 = Failed
                    {
                        ModelState.AddModelError("LoginEmail", loginFailed);
                    }
                    // else
                    // {
                    //     //all good - keep going
                    //     HttpContext.Session.SetInt32("currUid",dbUser.UserId);       //move to very bottom because we need a final return statement - syntax req
                    //     return RedirectToAction("Success");      
                    // } 
                }
            }

            if(ModelState.IsValid == false)     //2nd separate check
            {
                return View("Index");
            }

            //all good - keep going
            HttpContext.Session.SetInt32("currUid",(int)dbUid);
            HttpContext.Session.SetString("currUname",dbUname);
            return RedirectToAction("Dashboard","Events");
            // return RedirectToAction("Success");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
            // return View("Index");            
            // - can I do this instead? - what's the diff from above line?
        }


        [HttpGet("/Home/Success")]
        public IActionResult Success()
        {
            //Prevent Unauthorized users from viewing Success page and crashing
            //if(currUid == null)    
            if(HttpContext.Session.GetInt32("currUid") == null)     
            {
                return View("Index");
            }
            return View();
        }


















        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
