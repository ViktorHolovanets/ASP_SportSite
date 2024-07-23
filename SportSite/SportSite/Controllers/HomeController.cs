using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SportSite.Models;
using SportSite.Models.Db;
using SportSite.Models.SignalR;
using SportSite.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SportSite.Controllers
{
    public class HomeController : Controller
    {
        Db db;
        IHubContext<MessageHub> hubContext;
        private readonly ILogger<HomeController> _logger;
        public HomeController(Db db, ILogger<HomeController> logger, IHubContext<MessageHub> hubContext)
        {
            this.db = db;
            this.hubContext = hubContext;
            if (db.Services.Count() < 1)
            {
                db.AddTypeSport();
            }
            _logger = logger;
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsLogin(string login)
        {

            return db.Accounts.FirstOrDefault(u => u.Login == login) != null ? Json(false) : Json(true);
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsCode(string createcode)
        {
            return db.Code.FirstOrDefault(code => code.Code.ToString() == createcode) == null ? Json(false) : Json(true);
        }
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAccount(Account user)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(new Client { Account = user });
                db.SaveChanges();

                return Login(new EnterUserView() { Login = user.Login, Password = user.Password }).Result;
            }
            return View();
        }

        public IActionResult CreateAccountCoach()
        {
            var tempServices = new List<ClassPersonList>();
            foreach (var item in db.Services.Where(s => s.IsTypeSport))
            {
                tempServices.Add(new ClassPersonList()
                {
                    Id = item.Id,
                    Name = $"{item.Name}"
                });
            }
            ViewBag.Services = new SelectList(tempServices, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult CreateAccountCoach(CreateAccountCoachView coach)
        {
            if (ModelState.IsValid)
            {
                coach.account.Role = Role.coach;

                db.Coaches.Add(new Coach { Account = coach.account, Details = coach.Details, typeSports = db.Services.FirstOrDefault(s => s.Id.ToString() == coach.IdService) });
                db.Code.Remove(db.Code.FirstOrDefault(c => c.Code == coach.CreateCode));
                db.SaveChanges();

                return Login(new EnterUserView() { Login = coach.account.Login, Password = coach.account.Password }).Result;
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(EnterUserView? model)
        {
            if (ModelState.IsValid)
            {
                Account user = await db.Accounts.Include(a => a.Client).FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация
                    _logger.LogInformation($"Enter User: {model.Login}", DateTime.UtcNow.ToLongTimeString());
                    return RedirectToRoute("areas", new { area = "Edit", controller = "Home", action = "ViewProfile" });
                }
                ViewBag.Message = "Incorrect login and (or) password";
                _logger.LogError($"Incorrect enter", DateTime.UtcNow.ToLongTimeString());
            }
            else
            {
                _logger.LogWarning($"Invalid enter", DateTime.UtcNow.ToLongTimeString());
            }
            return View("Index", db.Services);
        }
        public async Task<JsonResult> NewMessageAsync()
        {
            try
            {
                var m = new Message()
                {
                    Name = Request.Form["name"],
                    Tel = Request.Form["tel"],
                    Comments = Request.Form["comments"]
                };
                db.Messages.Add(m);
                db.SaveChanges();
                var message = new MessageSignalR()
                {
                    Message = m,
                    CountMessage = db.Messages.Count(m=>!m.IsRead)
                };
                await hubContext.Clients.All.SendAsync("Receive", message);
            }
            catch (Exception)
            {

                return Json(false);
            }
            return Json(true);
        }

        public IActionResult ViewServicesDetails(string? id)
        {
            return View(db.Services.FirstOrDefault(t => t.Id.ToString() == id));
        }

        public IActionResult ViewCoachOfTrainings(string? id)
        {
            var coaches = db.Coaches.Include(c => c.Account.Client).Where(coach => coach.typeSports.Id.ToString() == id);
            return View(coaches);
        }
        public IActionResult ViewTraning(string? id)
        {
            ViewBag.Add = true;
            var trainings = db.Trainings.Include(tr => tr.dayofWeeks).Where(tr => tr.coach.Id.ToString() == id && tr.Clients.FirstOrDefault(cl=>cl.Account.Login == User.Identity.Name)==null).ToList();
            return PartialView(trainings);
        }
        [Route("")]
        public IActionResult Index()
        {
            return View(db.Services);
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
            //RedirectToRoute("Account", new { area = "Account", controller = "Home", action = "Index" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private async Task Authenticate(Account user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
        public IActionResult AddClientTraining(string id)
        {
            var client = db.Clients.FirstOrDefault(c => c.Account.Login == User.Identity.Name);
            if(client != null)
            {
                var training= db.Trainings.FirstOrDefault(tr=>tr.Id.ToString()==id);
                if(training != null)
                {
                    training.Clients.Add(client);
                    db.SaveChanges();
                    return Json(true);
                }
            }
            return Json(false);
        }
    }
}