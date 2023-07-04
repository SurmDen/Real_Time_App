using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Context_Authorization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Context_Authorization.Controllers
{
    public class HomeController : Controller
    {
        private IHubContext<ChatRoom> context;

        public HomeController(IHubContext<ChatRoom> context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(string message)
        {
            await context.Clients.All.SendAsync("Receive", message);
            return RedirectToAction(nameof(Index));
        }
    }
}
