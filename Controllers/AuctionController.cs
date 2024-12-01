using System.Collections.ObjectModel;
using System.Security.Claims;
using Auktion.Areas.Identity.Data;
using Auktion.Core;
using Auktion.Core.Interfaces;
using Auktion.Models.Auctions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auktion.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionService _auctionService;
        private readonly IBidService _bidService; 
        private readonly UserManager<AuktionUser> _userManager;
        private readonly IMapper _mapper;

        // Inject IMapper in the constructor
        public AuctionController(IAuctionService auctionService, IBidService bidService, IMapper mapper, UserManager<AuktionUser> userManager)
        {
            _auctionService = auctionService;
            _bidService = bidService;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        // GET: AuctionController
        public ActionResult Index()
        {
            var auctions = _auctionService.GetAuctions();
            ViewBag.AuctionOwners = auctions.ToDictionary(a => a.AuctionId, a => a.OwnerId);
            ViewBag.userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var auctionsVm = _mapper.Map<Collection<AuctionVm>>(auctions);
            return View("Index", auctionsVm);
        }

        // GET: AuctionController/Details/5
        public ActionResult Details(int id)
        {
            var auction = _auctionService.GetAuctionById(id);
            if (auction == null)
            {
                return NotFound();
            }
            var bidderNames = auction.Bids
                .Select(b => b.UserId)
                .Distinct()
                .ToDictionary(userId => userId, userId => _userManager.FindByIdAsync(userId).Result?.UserName ?? "Unknown");
            ViewBag.BidderNames = bidderNames;
            ViewBag.BidUserIds = auction.Bids.Select(b => b.UserId).ToList();
            var auctionDetailsVm = _mapper.Map<AuctionVm>(auction);
            ViewBag.CreatorUserId = auction.OwnerId;
            return View(auctionDetailsVm);
        }
        
        // GET: AuctionController/PlaceBid
        public ActionResult PlaceBid(int id)
        {
            var auction = _auctionService.GetAuctionById(id);
            if (auction == null)
            {
                return NotFound();
            }

            var bidVm = new BidVm(); 
            ViewBag.AuctionId = id;  
            return View(bidVm); 
        }

        // POST: AuctionController/PlaceBid
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceBid(int auctionId, BidVm bidVm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AuctionId = auctionId; 
                return View(bidVm);
                
            }
            var auction = _auctionService.GetAuctionById(auctionId);
            if (auction == null)
            {
                return NotFound();
            }
            var highestBid = auction.Bids.OrderByDescending(b => b.Amount).FirstOrDefault();
            if (highestBid != null && bidVm.Amount <= highestBid.Amount)
            {
                ModelState.AddModelError("Amount", "Your bid must be higher than the current highest bid.");
                ViewBag.AuctionId = auctionId;
                return View(bidVm);
            }

            var bid = _mapper.Map<Bid>(bidVm);
            bid.Time = DateTime.Now;
            bid.AuctionId = auctionId;
            bid.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _bidService.CreateBid(bid);
            return RedirectToAction("Details", new { id = auctionId });
        }
        
        // GET: AuctionController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AuctionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AuctionVm auctionVm)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(auctionVm);
            }

            try
            { 
                var auction = _mapper.Map<Auction>(auctionVm);
                auction.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
                _auctionService.CreateAuction(auction);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw new Exception("Could not save auction: " + ex.Message, ex);
             
            }
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        { 
            var auction = _auctionService.GetAuctionById(id);
            if (auction == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (auction.OwnerId != userId)
            {
                return Forbid();
            }
            var auctionVm = _mapper.Map<AuctionVm>(auction);
            return View(auctionVm);
        }

        // POST: AuctionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuctionVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                _auctionService.UpdateAuction(model.AuctionId, model.Description);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating auction: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the auction.");
                return View(model);
            }
        }


        
        // GET: AuctionController/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var auction = _auctionService.GetAuctionById(id);
            if (auction == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (auction.OwnerId != userId)
            {
                return Forbid();
            }
            var auctionVm = _mapper.Map<AuctionVm>(auction);
            return View(auctionVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _auctionService.DeleteAuction(id);
                return RedirectToAction(nameof(Index)); 
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while deleting the auction.");
                return RedirectToAction(nameof(Delete), new { id }); 
            }
        }
        
        [Authorize]
        public IActionResult UserBids(string search)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); 
            }
            var auctions = _auctionService.GetUserBidsOnOngoingAuctions(userId, search);
            var auctionsVm = _mapper.Map<List<AuctionVm>>(auctions);
            return View(auctionsVm);
        }
        
        [Authorize]
        public IActionResult UserWonAuctions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var wonAuctions = _auctionService.GetWonAuctions(userId);
            var auctionVms = _mapper.Map<List<AuctionVm>>(wonAuctions);
            return View(auctionVms);
        }

    }

}
