using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace E_commerce_23TH0024.Controllers
{
    //[Authorize(Roles = "admin")]
    [Area("Admin")]
    public class AspNetUsers_23TH0024Controller : Controller
    {
        private ApplicationDbContext db;
        private RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AspNetUsers_23TH0024Controller(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public ActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();
            var model = users.Select(user => new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = (List<string>)_userManager.GetRolesAsync(user).Result
            }).ToList();

            return View(model);
        }

        public ActionResult CreatePassWord(string UserId)
        {
            CreatePassword model = new CreatePassword();
            model.UserId = UserId;
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CreatePassWord(CreatePassword model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return View(model);
            }
            var result = await _userManager.RemovePasswordAsync(user);
            if (result.Succeeded)
            {
                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (addPasswordResult.Succeeded)
                {
                    ViewBag.Success = "Tạo mật khẩu thành công";
                    return RedirectToAction("Details", new { id = model.UserId });
                }
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Code);
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Code);
            }
            return View(model);
        }
        public async Task<ActionResult> AssignRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = _roleManager.Roles.ToList();
            var assignedRoles = await _userManager.GetRolesAsync(user);

            var model = new AssignRoleViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                Roles = roles.Select(role => new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Name,
                    Selected = assignedRoles.Contains(role.Name)
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            var user = await  _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                bool selected = model.SelectedRoles.Contains(role.Name);
                bool inRole = userRoles.Contains(role.Name);

                if (selected && !inRole)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                    if (role.Name == "khachhang")
                    {
                        var khachhang = new KhachHang { UserID = user.Id };
                        db.KhachHangs.Add(khachhang);
                    }
                    else if (role.Name == "nhanvien")
                    {
                        var nhanvien = new NhanVien { UserID = user.Id };
                        db.NhanViens.Add(nhanvien);
                    }
                }
                else if (!selected && inRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (role.Name == "khachhang")
                    {
                        var khachHangs = db.KhachHangs.Where(x => x.UserID == user.Id);
                        db.KhachHangs.RemoveRange(khachHangs);
                    }
                    else if (role.Name == "nhanvien")
                    {
                        var nhanViens = db.NhanViens.Where(x => x.UserID == user.Id);
                        db.NhanViens.RemoveRange(nhanViens);
                    }
                }
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

       
        // GET: AspNetUsers_23TH0024/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new BadRequestResult(); 
            }
            AspNetUsers aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers_23TH0024/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers_23TH0024/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user =  new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                //_userManager.Add(user);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Code);
                }
            }

            return View(model);
        }

        // GET: AspNetUsers_23TH0024/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            AspNetUsers aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers_23TH0024/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,UserName,Email, PhoneNumber")] AspNetUsers aspNetUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(aspNetUser.Id);
                user.UserName = aspNetUser.UserName;
                user.Email = aspNetUser.Email;
                user.PhoneNumber = aspNetUser.PhoneNumber;
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Code);
                    }
                }
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers_23TH0024/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            AspNetUsers aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers_23TH0024/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
