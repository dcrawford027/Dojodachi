using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dojodachi.Models;
using Microsoft.AspNetCore.Http;

namespace dojodachi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Happiness") == null)
            {
                NewGame();
            }

            int? happiness = HttpContext.Session.GetInt32("Happiness");
            int? fullness = HttpContext.Session.GetInt32("Fullness");
            int? energy = HttpContext.Session.GetInt32("Energy");
            int? meals = HttpContext.Session.GetInt32("Meals");

            if (happiness >= 100 && fullness >= 100 && energy >= 100)
            {
                HttpContext.Session.SetString("Message", "Congratulations! You Won!");
            }

            if (fullness == 0 || happiness == 0)
            {
                HttpContext.Session.SetString("Message", "Your Dojodachi has passed away.");
            }

            ViewBag.Happiness = HttpContext.Session.GetInt32("Happiness");
            ViewBag.Fullness = HttpContext.Session.GetInt32("Fullness");
            ViewBag.Energy = HttpContext.Session.GetInt32("Energy");
            ViewBag.Meals = HttpContext.Session.GetInt32("Meals");
            ViewBag.Message = HttpContext.Session.GetString("Message");

            return View("Index");
        }

        [HttpPost("newGame")]
        public IActionResult NewGame()
        {
            Dojodachi newPet = new Dojodachi();

            string message = "Meet your new Dojodachi!";

            HttpContext.Session.SetInt32("Happiness", newPet.Happiness);
            HttpContext.Session.SetInt32("Fullness", newPet.Fullness);
            HttpContext.Session.SetInt32("Energy", newPet.Energy);
            HttpContext.Session.SetInt32("Meals", newPet.Meals);
            HttpContext.Session.SetString("Message", message);

            return RedirectToAction("Index");
        }

        [HttpPost("feed")]
        public IActionResult Feed()
        {
            int? currentMeals = HttpContext.Session.GetInt32("Meals");
            int? currentFullness = HttpContext.Session.GetInt32("Fullness");

            double doesntLike = new Random().NextDouble();
            string message = "";

            if (currentMeals > 0)
            {
                currentMeals--;
                HttpContext.Session.SetInt32("Meals", (int)currentMeals);

                if (doesntLike * 100 > 25)
                {
                    int addedFullness = new Random().Next(5, 11);
                    currentFullness += addedFullness;
                    message = $"You fed your Dojodachi. Fullness +{addedFullness}, Meals -1.";

                    HttpContext.Session.SetInt32("Fullness", (int)currentFullness);
                    HttpContext.Session.SetString("Message", message);
                }

                else 
                {
                    message = "Your Dojodachi did not like the food. Fullness +0, Meals -1.";
                    HttpContext.Session.SetString("Message", message);
                }
                
                return RedirectToAction("Index");
            }

            else
            {
                message = $"You do not have any meals to feed your Dojodachi.";
                HttpContext.Session.SetString("Message", message);

                return RedirectToAction("Index");
            }
        }

        [HttpPost("play")]
        public IActionResult Play()
        {
            int? currentEnergy = HttpContext.Session.GetInt32("Energy");
            int? currentHappiness = HttpContext.Session.GetInt32("Happiness");

            double doesntLike = new Random().NextDouble();
            string message = "";
            currentEnergy -= 5;

            if (currentEnergy < 0)
            {
                currentEnergy = 0;
                message = "Your Dojodachi does not have enough energy to play.";
                HttpContext.Session.SetString("Message", message);
            }

            else
            {
                if (doesntLike * 100 > 25)
                {
                    int addedHappiness = new Random().Next(5, 11);
                    currentHappiness += addedHappiness;
                    message = $"You played with your Dojdachi. Happiness +{addedHappiness}, Energy -5";

                    HttpContext.Session.SetInt32("Happiness", (int)currentHappiness);
                    HttpContext.Session.SetString("Message", message);
                }

                else 
                {
                    message = "Your Dojodachi didn't want to play. Energy -5";
                    HttpContext.Session.SetString("Message", message);
                }
            }
            
            HttpContext.Session.SetInt32("Energy", (int)currentEnergy);

            return RedirectToAction("Index");
        }

        [HttpPost("work")]
        public IActionResult Work()
        {
            int? currentEnergy = HttpContext.Session.GetInt32("Energy");
            int? currentMeals = HttpContext.Session.GetInt32("Meals");

            currentEnergy -= 5;
            string message = "";

            if (currentEnergy < 0)
            {
                currentEnergy = 0;
                message = "Your Dojodachi does not have enough energy to work.";

                HttpContext.Session.SetInt32("Energy", (int)currentEnergy);
                HttpContext.Session.SetString("Message", message);

                return RedirectToAction("Index");
            }

            int addedMeals = new Random().Next(1, 4);
            currentMeals += addedMeals;
            message = $"You worked your Dojodachi. Meals +{addedMeals}, Energy -5";

            HttpContext.Session.SetInt32("Energy", (int)currentEnergy);
            HttpContext.Session.SetInt32("Meals", (int)currentMeals);
            HttpContext.Session.SetString("Message", message);

            return RedirectToAction("Index");
        }

        [HttpPost("sleep")]
        public IActionResult Sleep()
        {
            int? currentEnergy = HttpContext.Session.GetInt32("Energy");
            int? currentFullness = HttpContext.Session.GetInt32("Fullness");
            int? currentHappiness = HttpContext.Session.GetInt32("Happiness");

            currentEnergy += 15;
            currentFullness -= 5;
            currentHappiness -= 5;
            string message = $"Your Dojodachi slept. Energy +15, Fullness -5, Happiness -5";

            if (currentFullness <= 0)
            {
                currentFullness = 0;
            }

            if (currentHappiness <= 0)
            {
                currentHappiness = 0;
            }

            HttpContext.Session.SetInt32("Energy", (int)currentEnergy);
            HttpContext.Session.SetInt32("Fullness", (int)currentFullness);
            HttpContext.Session.SetInt32("Happiness", (int)currentHappiness);
            HttpContext.Session.SetString("Message", message);

            return RedirectToAction("Index");
        }
    }
}
