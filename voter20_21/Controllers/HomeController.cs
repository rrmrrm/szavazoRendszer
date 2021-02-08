using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using voter20_21.Models;
using voter20_21.Services;
namespace voter20_21.Controllers
{
    public class HomeController : Controller
    {
        private readonly AccountService accService;
        private readonly VoterService voterService;
        public HomeController(AccountService _accountService, VoterService _voterService)
        {
            accService = _accountService;
            voterService = _voterService;
        }
        /// <summary>
        /// A ViewBag.votingAnswers-be visszaküldi a votingId-hoz tartozó Voting válaszlehetőségeit
        /// </summary>
        /// <param name="votingId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index(Int32? votingId)
        {
            string email = HttpContext.Session.GetString("user");
            //ha be van jelentkezve a felhasználó, akkor a rá vontakozó dolgokat megjelenítjük:
            if (email != null)
            {
                User user = voterService.tryFindUser(email);
                if (user != null)
                {
                    //ViewBag-be bele kell pakolni a felhasználóhoz rendelt aktív szavazásokat.
                    ViewBag.email = user.email;
                    //var activeVotings = voterService.findAssignedClosedVotings(user.Id);
                    var activeVotings = voterService.findAssignedOpenVotingsList(user.Id);
                    if (votingId != null)
                    {
                        ViewBag.votingAnswers = voterService.findAnswers(votingId);
                    }
                    return View("Index", activeVotings);
                }
            }
            return View("Index");
        }

        [HttpGet]
        public IActionResult Vote(Int32? votingId, Int32? answerId)
        {
            //TODO: cross-site request forgery ellen megvédeni a szavazatot
            string email = HttpContext.Session.GetString("user");
            if (email == null)
            {
                return View("Index");
            }
            User user = voterService.tryFindUser(email);
            if (votingId == null || answerId == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.email = email;
            if (voterService.Vote(user.Id, votingId, answerId)) {
                return View("VoteSuccessful");
            }
            else
            {
                return View("VoteUnsuccessful");
            }
            //TODO:ne felejtsem el az assignedUser-ben jelezni, hogy ez a felhasználó már szavazott
        }
        [HttpGet]
        public IActionResult ClosedVotings(Int32? votingId)
        {
            //TODO: ezt átirni hogy a closed és ne az open votingokat adja+ votingId-t a viewból átküldeni
            //TODO: title-t validálni? lehet, hogy gondot okozhat egy cselesen megadott string.
            string email = HttpContext.Session.GetString("user");
            if (email == null)
            {
                return RedirectToAction(nameof(Index));
            }
            User user = voterService.tryFindUser(email);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.email = user.email;
            if (votingId != null)
            {
                if (voterService.isUserIsAssignedToVoting((Int32)votingId, user.Id))
                {
                    ViewBag.votingStats = voterService.getVotingStats((Int32)votingId, user.Id);
                }
                //ViewBag.votingAnswers = voterService.findAnswers(votingId);
            }
            ViewBag.closedVotings = voterService.findAssignedClosedVotingsList(user.Id/*, title, start, end, filter*/);

            return View("ClosedVotings");
        }
        [HttpPost]
        public IActionResult ClosedVotings(VotingFilter filter)
        {
            //TODO: ezt átirni hogy a closed és ne az open votingokat adja+ votingId-t a viewból átküldeni
            //TODO: title-t validálni? lehet, hogy gondot okozhat egy cselesen megadott string.
            string email = HttpContext.Session.GetString("user");
            if (email == null)
            {
                return RedirectToAction(nameof(Index));
            }
            User user = voterService.tryFindUser(email);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.email = user.email;

            if (!ModelState.IsValid || !voterService.isFilterValid(filter))
            {
                ViewBag.closedVotings = voterService.findAssignedClosedVotingsList(user.Id);
                ModelState.AddModelError("", "Hibás szűrőfeltételek");
                return View("ClosedVotings", filter);
            }
            ViewBag.closedVotings = voterService.findAssignedClosedVotingsList(user.Id, filter/*, title, start, end, filter*/);

            return View("ClosedVotings");
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
