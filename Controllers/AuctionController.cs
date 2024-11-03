using System.Collections.ObjectModel;
using System.Security.Claims;
using Auktion.Core;
using Auktion.Core.Interfaces;
using Auktion.Models.Auctions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Auktion.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionService _auctionService;
        private readonly IBidService _bidService; 
        private readonly IMapper _mapper;

        // Inject IMapper in the constructor
        public AuctionController(IAuctionService auctionService, IBidService bidService, IMapper mapper)
        {
            _auctionService = auctionService;
            _bidService = bidService;
            _mapper = mapper;
        }
        
        // GET: AuctionController
        public ActionResult Index()
        {
            // Use _mapper to map Collection<Auction> to Collection<AuctionVm>
            var auctions = _auctionService.GetAuctions();
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
            
            // Map Auction to AuctionDetails
            var auctionDetailsVm = _mapper.Map<AuctionVm>(auction);
            
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
                return View(bidVm);
            }
            var auction = _auctionService.GetAuctionById(auctionId);
            if (auction == null)
            {
                return NotFound();
            }
            var bid = _mapper.Map<Bid>(bidVm);
            bid.Time = DateTime.Now;
            bid.AuctionId = auctionId;
            bid.UserId = "AnonymousUser";
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


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var auction = _auctionService.GetAuctionById(id);
            if (auction == null)
            {
                return NotFound();
            }
            var auctionVm = _mapper.Map<AuctionVm>(auction);
            return View(auctionVm);
        }

        // POST: AuctionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AuctionVm auctionVm)
        {
            if (!ModelState.IsValid)
            {
                return View(auctionVm);
            }
            try
            {
                var auction = _mapper.Map<Auction>(auctionVm);
                auction.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _auctionService.UpdateAuction(auction);
                return RedirectToAction(nameof(Index)); 
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while updating the auction.");
                return View(auctionVm);
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
    }
}
