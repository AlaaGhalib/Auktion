using System.Collections.ObjectModel;
using Auktion.Core;
using Auktion.Core.Interfaces;
using Auktion.Models.Auctions;
using Microsoft.AspNetCore.Mvc;

namespace Auktion.Controllers
{
    public class AuctionController : Controller
    {
        private IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }
        
        // GET: AuctionController
        public ActionResult Index()
        {
            Collection<Auction> auctions = _auctionService.GetAuctions();
            Collection<AuctionVm> auctionsVm = new Collection<AuctionVm>();
            
            foreach (var auction in auctions)
            {
                AuctionVm auctionVm = AuctionVm.ToAuctionVm(auction);
                auctionsVm.Add(auctionVm);
            }
            return View("Index",auctionsVm);
        }

        // GET: AuctionController/Details/5
        public ActionResult Details(int id)
        {
            Auction auction = _auctionService.GetAuctionById(id);
            if (auction == null) return BadRequest();
            
            AuctionDetails ADVM = AuctionDetails.ToAuctionVm(auction);
            return View(ADVM);
        }
/*
        // GET: AuctionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuctionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuctionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuctionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuctionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuctionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        */
    }
    
}
