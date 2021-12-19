using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebASP.Models;
using Microsoft.EntityFrameworkCore;
using WebASP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebASP.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        private IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _app;

        public HomeController(ApplicationContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment app)
        {
            db = context;
            _httpContextAccessor = httpContextAccessor;
            _app = app;
        }

        public async Task<IActionResult> Index(string email, string login, int? id, int page = 1, SortState sortOrder = SortState.IdAsc)
        {

            IQueryable<User> users = db.Users;
            //фильтрация или поиск
            if (id != null && id > 0)
            {
                users = users.Where(p => p.Id == id);
            }
            if (!String.IsNullOrEmpty(email))
            {
                users = users.Where(p => p.Email.Contains(email));
            }
            if (!String.IsNullOrEmpty(login))
            {
                users = users.Where(p => p.Login.Contains(login));
            }
            //сортировка
            switch (sortOrder)
            {
                case SortState.IdAsc:
                    {
                        users = users.OrderBy(m => m.Id);
                        break;
                    }
                case SortState.IdDesc:
                    {
                        users = users.OrderByDescending(m => m.Id);
                        break;
                    }
                case SortState.EmailAsc:
                    {
                        users = users.OrderBy(m => m.Email);
                        break;
                    }
                case SortState.EmailDesc:
                    {
                        users = users.OrderByDescending(m => m.Email);
                        break;
                    }
                case SortState.LoginAsc:
                    {
                        users = users.OrderBy(m => m.Login);
                        break;
                    }
                case SortState.LoginDesc:
                    {
                        users = users.OrderByDescending(m => m.Login);
                        break;
                    }
                default:
                    {
                        users = users.OrderBy(m => m.Id);
                        break;
                    }
            }
            //пагинация
            int pageSize = 10;
            var count = await users.CountAsync();
            var item = await users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilteViewModel = new FilteViewModel(id, email, login),
                Users = item
            };


            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model, IFormFile file)
        {
            string path;
            if (file != null)
            {
                path = "/Files/" + model.Email + file.FileName;
                using (FileStream fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create)) ;
            }
            else path = "/Files/avatar.jpg";
            var user = new User
            {
                Login = model.Login,
                birth_day = model.birth_day,
                Surname = model.Surname,
                Name = model.Name,
                middle_Name = model.middle_Name,
                Email = model.Email,
                Password = model.Password,
                Role = false,
                Picture = path
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (User != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (User != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (User != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user, IFormFile file, bool checkResp = false)
        {
            if (file != null)
            {
                String path = "/Files/" + user.Id + file.FileName;
                using (FileStream fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                user.Picture = path;
            }
            if (checkResp == true)
            {
                String path = "/Files/avatar.jpg";
                user.Picture = path;
            }
            db.Users.Update(user);
            await db.SaveChangesAsync();
            if (_httpContextAccessor.HttpContext.Request.Cookies["role"] == "True")
            {
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Profile");
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (User != null)
                    return View(user);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile file)
        {
            string path = null;
            User userLogin = db.Users.FirstOrDefault(p => p.Login == model.Login);
            User userEmail = db.Users.FirstOrDefault(p => p.Email == model.Email);
            if (userLogin == null && userEmail == null)
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        path = "/Files/" + model.Email + file.FileName;
                        using (FileStream fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                    else
                        path = "/Files/avatar.jpg";

                    var user = new User
                    {
                        Login = model.Login,
                        birth_day = model.birth_day,
                        Surname = model.Surname,
                        Name = model.Name,
                        middle_Name = model.middle_Name,
                        Email = model.Email,
                        Password = model.Password,
                        Role = false,
                        Picture = path
                    };
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }
        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }
        [HttpGet]
        public ViewResult Auth()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Auth(User user)
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["user"] != null)
            {
                Response.Cookies.Delete("user");
                Response.Cookies.Delete("login");
                Response.Cookies.Delete("role");

            }
            User userAuth = db.Users.FirstOrDefault(p => p.Email == user.Email && p.Password == user.Password);
            if (userAuth != null)
            {
                user = db.Users.FirstOrDefault(predicate => predicate.Email == user.Email);
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Append("user", userAuth.Id.ToString(), cookie);
                Response.Cookies.Append("login", userAuth.Login.ToString(), cookie);
                Response.Cookies.Append("role", userAuth.Role.ToString(), cookie);
                return RedirectToAction("Profile");
            }
            else if (userAuth == null)
            {
                return RedirectToAction("Register");
            }
            return View();
        }


        public async Task<IActionResult> Post(string title, string message, int user_id, string user, string picture, int? id, DateTime date, int page = 1, SortStatePost sortOrder = SortStatePost.IdAsc)
        {
            IQueryable<Post> posts = db.Posts.Include(p => p.User);
            IQueryable<User> users = db.Users.Include(p => p.Picture);
            //фильтрация или поиск
            if (id != null && id > 0)
            {
                posts = posts.Where(p => p.id == id);
            }
            if (!String.IsNullOrEmpty(title))
            {
                posts = posts.Where(p => p.Title.Contains(title));
            }
            if (user_id != null && user_id > 0)
            {
                posts = posts.Where(p => p.User_ID == user_id);
            }
            if (!String.IsNullOrEmpty(user))
            {
                posts = posts.Where(p => p.User.Login == user);
            }
            //сортировка
            switch (sortOrder)
            {
                case SortStatePost.IdAsc:
                    {
                        posts = posts.OrderBy(m => m.id);
                        break;
                    }
                case SortStatePost.IdDesc:
                    {
                        posts = posts.OrderByDescending(m => m.id);
                        break;
                    }
                case SortStatePost.TitleAsc:
                    {
                        posts = posts.OrderBy(m => m.Title);
                        break;
                    }
                case SortStatePost.TitleDesc:
                    {
                        posts = posts.OrderByDescending(m => m.Title);
                        break;
                    }
                case SortStatePost.User_IDAsc:
                    {
                        posts = posts.OrderBy(m => m.User_ID);
                        break;
                    }
                case SortStatePost.User_IDDesc:
                    {
                        posts = posts.OrderByDescending(m => m.User_ID);
                        break;
                    }
                case SortStatePost.Post_DateAsc:
                    {
                        posts = posts.OrderBy(m => m.Post_Date);
                        break;
                    }
                case SortStatePost.Post_DateDesc:
                    {
                        posts = posts.OrderByDescending(m => m.Post_Date);
                        break;
                    }

                default:
                    {
                        posts = posts.OrderBy(m => m.id);
                        break;
                    }
            }
            //пагинация
            int pageSize = 10;
            var count = await posts.CountAsync();
            var item = await posts.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            PostViewModel viewModel = new PostViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortPostViewModel(sortOrder),
                FilteViewModel = new FilterPostViewModel(id, title, message, user_id, user, picture, date),
                Posts = item
            };

            return View(viewModel);
        }

        public IActionResult Create_Post()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create_Post(Post view, IFormFile file)
        {
            if (file != null)
            {
                string path = "/Files/" + view.Title + view.id + view.User_ID + file.FileName;
                using (FileStream fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                view.Picture = path;
            }
            else
            {
                view.Picture = null;
            }
            if (_httpContextAccessor.HttpContext.Request.Cookies["role"] != "True")
            {
                view.User_ID = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Cookies["user"]);
            }
            view.Post_Date = DateTime.Now;
            db.Posts.Add(view);
            await db.SaveChangesAsync();
            return RedirectToAction("Post");
        }

        [HttpGet]
        [ActionName("Delete_Post")]
        public async Task<IActionResult> ConfirmDelete_Post(int? id)
        {
            if (id != null)
            {
                Post post = await db.Posts.FirstOrDefaultAsync(predicate => predicate.id == id);
                if (post != null)
                    return View(post);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete_Post(int? id)
        {
            if (id != null)
            {
                Post post = await db.Posts.FirstOrDefaultAsync(predicate => predicate.id == id);
                if (post != null)
                {
                    db.Posts.Remove(post);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Post");
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> Edit_Post(int? id)
        {
            if (id != null)
            {
                Post post = await db.Posts.FirstOrDefaultAsync(predicate => predicate.id == id);
                string pic = post.Picture;
                if (post != null)
                    return View(post);

            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit_Post(Post post, IFormFile file, bool checkResp = false)
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["role"] != "True")
            {
                post.User_ID = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Cookies["user"]);
            }
            if (file != null)
            {
                string path = "/Files/" + post.id + post.User_ID + file.FileName;
                using (FileStream fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                post.Picture = path;

            }
            if (checkResp == true)
            {
                post.Picture = null;
            }
            db.Posts.Update(post);
            await db.SaveChangesAsync();
            return RedirectToAction("Post");
        }

        public async Task<IActionResult> Profile()
        {
            string cookievalue = _httpContextAccessor.HttpContext.Request.Cookies["user"];
            if (cookievalue != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == Convert.ToInt32(cookievalue));
                return View(user);
            }
            return NotFound();
        }

        public async Task<IActionResult> Exit()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["user"] != null)
            {
                Response.Cookies.Delete("user");
                Response.Cookies.Delete("login");
                Response.Cookies.Delete("role");
            }
            return RedirectToAction("Auth");
        }
    }
        
}
