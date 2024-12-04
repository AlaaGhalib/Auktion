using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Auktion.Areas.Identity.Data;
using Auktion.Core;
using Auktion.Core.Interfaces;
using Auktion.Models.Auctions;
using AutoMapper;
using MySqlX.XDevAPI;

namespace Auktion.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<AuktionUser> _userManager;
        private readonly IAuctionService _auctionService;
        private readonly IMapper _mapper;

        public AdminController(UserManager<AuktionUser> userManager, IAuctionService auctionService, IMapper mapper)
        {
            _userManager = userManager;
            _auctionService = auctionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        public ActionResult ManageAuctions()
        {
            var auctions = _auctionService.GetAllAuctions();
            var auctionsVm = _mapper.Map<IEnumerable<AuctionVm>>(auctions);
            return View("ManageAuctions", auctionsVm);
        }
        [HttpGet]
        public IActionResult UserAuctions(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be null or empty.");
            }
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            if (!User.IsInRole("Administrator"))
            {
                return Forbid();
            }
            var auctions = _auctionService.GetAuctionsByUserId(id);
            var auctionsVm = _mapper.Map<IEnumerable<AuctionVm>>(auctions);
            ViewBag.UserName = user.UserName;
            return View("UserAuctions", auctionsVm);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID '{id}' not found.");
            }
            if (!User.IsInRole("Administrator"))
            {
                return Forbid();
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["Error"] = "Unable to delete the user.";
                return RedirectToAction(nameof(ManageUsers));
            }

            TempData["Success"] = $"User {user.Email} deleted successfully.";
            return RedirectToAction(nameof(ManageUsers));
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteAuction(int id)
        {
            var auction = _auctionService.GetAuctionById(id);
            if (auction == null)
            {
                return NotFound();
            }
            if (!User.IsInRole("Administrator"))
            {
                return Forbid();
            }
            var auctionVm = _mapper.Map<AuctionVm>(auction);
            return View(auctionVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Administrator"))
            {
                return Forbid(); 
            }
            try
            {
                _auctionService.DeleteAuction(id);
                return RedirectToAction(nameof(ManageAuctions)); 
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while deleting the auction.");
                return RedirectToAction(nameof(DeleteAuction), new { id }); 
            }
        }
    }
}