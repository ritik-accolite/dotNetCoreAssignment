using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyNotes.Data;
using MyNotes.Models;
using MyNotes.ViewModel;

namespace MyNotes.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public NoteController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new NoteViewModel());
        }

        //note/index
        [HttpGet]
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var notes = _context.Notes.Where(n => n.UserId == userId).ToList();
            return View(notes);
        }

        //note/create
        [HttpPost]
        public IActionResult Create(NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var note = new Note()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Color = model.Color,
                    UserId = userId
                };
                _context.Notes.Add(note);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), "Note");
            }
            return View(model);
        }

        //note/edit
        [HttpGet]
        public IActionResult Edit(int id) 
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var note = _context.Notes.FirstOrDefault(n => n.Id == id);
            if(note.UserId == userId)
            {
                var model = new NoteViewModel()
                {
                    Id = note.Id,
                    Title = note.Title,
                    Description = note.Description,
                    CreatedDate = note.CreatedDate,
                    UserId = userId,
                    Color = note.Color,
                };
                return View(model);   
            }
            else 
            {
                return Content("You are not authorised.");
            }
        }

        //note/edit
        [HttpPost]
        public IActionResult Edit(NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                if (model.UserId ==userId)
                {
                    var note = new Note()
                    {
                        Title = model.Title,
                        Id = model.Id,
                        CreatedDate=model.CreatedDate,
                        Description = model.Description,
                        Color = model.Color,
                        UserId = userId
                    };
                    _context.Notes.Update(note);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index), "Note");
                }
            }
            return View(model);
        }


        public IActionResult Delete(int id)
        {
            if(id == 0)
            {
                return Content("Id is Null.");
            };
            var userId = _userManager.GetUserId(HttpContext.User);
            var note = _context.Notes.FirstOrDefault(n => n.Id == id);
            if (note.UserId == userId)
            {
                
                _context.Notes.Remove(note);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Content("You are not authorised.");
            }
        }
    }
}
