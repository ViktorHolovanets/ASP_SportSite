using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SportSite.Areas.Edit.ViewModels;
using SportSite.Models;
using SportSite.Models.Db;
using SportSite.ViewModels;
using System.Linq;

namespace SportSite.Areas.Edit.Controllers
{
    [Authorize]
    [Area("Edit")]
    public class HomeController : Controller
    {
        Db _context;
        public HomeController(Db context)
        {
            _context = context;
        }
        private Account GetAccount(string? id = null)
        {
            return _context.Accounts.Include(a => a.Client).FirstOrDefault(id != null ? a => a.Id.ToString() == id : a => a.Login == User.Identity!.Name);
        }
        public int UnreadMessage()
        {
            return _context.Messages.Where(m => !m.IsRead).Count();
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult IncorrectPassword(string oldpassword)
        {
            var account = GetAccount();
            return account?.Password == oldpassword ? Json(true) : Json(false);
        }
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }
        [Route("Edit/DetailsUser")]
        // GET: HomeController/Details/5
        public ActionResult DetailsUser(string? id)
        {
            return View(GetAccount(id)?.Client);
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(string? id)
        {
            return View(_context.GetUser(id) ?? GetAccount(id).Client);
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("DetailsUser");
            }
            catch
            {
                return View("DetailsUser");
            }
        }
        // GET: HomeController/Edit/5
        public ActionResult EditPassword()
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(ViewEditPassword? newpassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var account = GetAccount();
                    account.Password = newpassword!.Password;
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("DetailsUser");
            }
            catch
            {
                return View("DetailsUser");
            }
        }

        public ActionResult ViewProfile()
        {
            try
            {
                ViewBag.UnreadMessage = UnreadMessage();
                return View(GetAccount()?.Client);
            }
            catch (Exception)
            {
                return View("/Views/Home/Logout");
            }
            
        }
        [HttpGet]
        [Route("ViewUser")]
        public IActionResult ViewUsers(Role? role = Role.client)
        {
            var getlist = (from cl in _context.Accounts
                           where cl.Role == role
                           select cl.Client).ToList();
            return PartialView(getlist);
        }
        public IActionResult ViewCode()
        {
            var account = GetAccount();
            return PartialView(_context.Code.Where(c => c.Сreator == account.Id));
        }

        public IActionResult CreateCode()
        {
            _context.Code.Add(new CreateCodeAccounts()
            {
                Code = Guid.NewGuid(),
                Сreator = GetAccount().Id
            });
            _context.SaveChanges();
            var account = GetAccount();
            return PartialView("Viewcode", _context.Code.Where(c => c.Сreator == account.Id));
        }
        public IActionResult DeleteCode(string? id)
        {
            var code = _context.Code.FirstOrDefault(a => a.Id.ToString() == id);
            var account = GetAccount();
            if (code != null)
            {
                _context.Code.Remove(code);
                _context.SaveChanges();
            }
            
            var message = _context.Code.Where(c => c.Сreator == account.Id).ToList();
            return PartialView("Viewcode", message);
        }
        public IActionResult Delete(string? id)
        {
            try
            {
                var account = _context.Accounts.FirstOrDefault(a => a.Client!.Id.ToString() == id);
                if (account != null)
                {
                    _context.Accounts.Remove(account);
                    _context.SaveChanges();
                }

            }
            catch
            {
                return View("ViewProfile");
            }
            return View("ViewProfile");
        }
        //
        public IActionResult CreateTraining()
        {
            var tempCoaches = new List<ClassPersonList>();
            var listClients = new List<ClassPersonList>();
            var clients = _context.Clients.Include(c => c.Account.Client);
            foreach (var item in clients)
            {
                listClients.Add(new ClassPersonList()
                {
                    Id = item.Id,
                    Name = $"{item.Account.Client!.Name} {item.Account.Client.Surname}"
                });
            }
            var coaches = _context.Coaches.Include(c => c.Account.Client);
            foreach (var item in coaches)
            {
                tempCoaches.Add(new ClassPersonList()
                {
                    Id = item.Id,
                    Name = $"{item.Account.Client!.Name} {item.Account.Client.Surname}"
                });
            }
            ViewBag.Clients = new SelectList(listClients, "Id", "Name");
            ViewBag.Coaches = new SelectList(tempCoaches, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult CreateTraining(ViewCreateTraining viewCreateTraining)
        {
            List<DayOfWeekTraining> dayOfWeekTrainings = new List<DayOfWeekTraining>();
            var c = _context.Coaches.FirstOrDefault(c => c.Id == viewCreateTraining.IdCoach);
            foreach (var item in viewCreateTraining.dayofWeeks)
            {
                dayOfWeekTrainings.Add(new DayOfWeekTraining()
                {
                    dayofWeek = item,
                    Time = viewCreateTraining.Time
                });
            }
            var training = new Training()
            {
                coach = _context.Coaches.FirstOrDefault(c => c.Id == viewCreateTraining.IdCoach),
                training = viewCreateTraining.typeTraining,
                dayofWeeks = dayOfWeekTrainings
            };
            if (viewCreateTraining.typeTraining==TypeTraining.Individual&& viewCreateTraining.IdClient!=null)
            {
                var client = _context.Clients.FirstOrDefault(c => c.Id == viewCreateTraining.IdClient);
                if (client != null)
                {
                    training.Clients.Add(client);
                }
            }
            _context.Trainings.Add(training);
            _context.SaveChanges();
            return CreateTraining();
        }
        [Authorize(Roles = "manager")]
        public IActionResult GetMessage()
        {
            return PartialView(_context.Messages.OrderBy(m => m.IsRead));
        }
        [Authorize(Roles = "manager")]
        public IActionResult ReadMessage(string? id)
        {
            var message = _context.Messages.FirstOrDefault(m => m.Id.ToString() == id && !m.IsRead);
            if (message != null)
            {
                message.IsRead = true;
                _context.SaveChanges();
                ViewBag.UnreadMessage = UnreadMessage();
                return Json(UnreadMessage());
            }
            return Json(false);
        }
        [Authorize(Roles = "client")]
        public IActionResult ViewProfileClient()
        {
            return View(GetAccount().Client);
        }
        [Authorize(Roles = "client")]
        public IActionResult ViewClientTraining(bool IsView=true)
        {
            ViewBag.Add = IsView;
            var account = GetAccount();
            if (account == null)
            {
                return View("/Views/Home/Logout");
            }
            var client = _context.Clients.FirstOrDefault(c => c.Account.Id == account.Id);
            var trainings = _context.Trainings.Include(tr => tr.dayofWeeks). Where(tr => tr.Clients.Where(cl=>cl.Id == client!.Id).Count()>0).ToList();
            if (trainings != null && trainings.Count() > 0)
            {
                return PartialView("/Views/Home/ViewTraning.cshtml", trainings);
            }
            return Json("Can't find workouts");
        }
        public IActionResult DeleteClientTraining(string id)
        {
            var client = _context.Clients.Include(cl=>cl.trainings).FirstOrDefault(c => c.Account.Login == User.Identity!.Name);
            if (client != null)
            {
                var training = _context.Trainings.FirstOrDefault(tr => tr.Id.ToString() == id);
                if (training != null)
                {
                    client.trainings.Remove(training);
                    _context.SaveChanges();
                    return ViewClientTraining(false);
                }
            }
            return Json(false);
        }
        public IActionResult ProfileCoach()
        {
            var coach = _context.Coaches.Include(c=>c.Account).FirstOrDefault(c => c.Account.Login == User.Identity!.Name);
            if(coach!= null)
            {
                var training = _context.Trainings
                    .Include(tr=>tr.Clients)
                    .ThenInclude(cl=>cl.Account)
                    .ThenInclude(a=>a.Client)
                    .Include(tr=>tr.dayofWeeks)
                    .Where(tr => tr.coach!.Id == coach.Id);
                return PartialView(training);
            }
            return PartialView();
        }
        public IActionResult ViewTrainings()
        {
            return View();
        }
        [Authorize(Roles = "manager")]
        public IActionResult CreateManager()
        { 
            return View();
        }
        [Authorize(Roles = "manager")]
        [HttpPost]
        public IActionResult CreateManager(Account user)
        {
            if (ModelState.IsValid)
            {
                user.Role = Role.manager;
                _context.Accounts.Add(user);
                _context.SaveChanges();
                return RedirectToAction("ViewProfile");
            }
            return View();
        }
    }
}
