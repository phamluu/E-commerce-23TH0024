//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Helpers;
//using System.Web.Mvc;
//using E_commerce_23TH0024.Models;
//using E_commerce_23TH0024.Data;
//using E_commerce_23TH0024.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

//namespace E_commerce_23TH0024.Controllers
//{
//    [Authorize(Roles = "admin")]
//    public class AspNetUsers_23TH0024Controller : Controller
//    {
//        private ApplicationDbContext db;
//        private RoleManager<IdentityRole> _roleManager;
//        private readonly UserManager<ApplicationUser> _userManager;
//        public AspNetUsers_23TH0024Controller(ApplicationDbContext context)
//        {
//            db = context;
//            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
//            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
//        }
//        public ActionResult CreatePassWord(string UserId)
//        {
//            CreatePassword model = new CreatePassword();
//            model.UserId = UserId;
//            return View(model);
//        }
//        [ValidateAntiForgeryToken]
//        [HttpPost]
//        public async Task<ActionResult> CreatePassWord(CreatePassword model) {
//            var user = await _userManager.FindByIdAsync(model.UserId);
//            if (user == null) {
//                return View(model);
//            }
//                var result = await _userManager.RemovePasswordAsync(user);
//            if (result.Succeeded)
//            {
//                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
//                if (addPasswordResult.Succeeded)
//                {
//                    ViewBag.Success = "Tạo mật khẩu thành công";
//                    return RedirectToAction("Details", new { id = model.UserId });
//                }
//                foreach (var error in addPasswordResult.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error);
//                }
//            }
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error);
//            }
//            return View(model);
//        }
//        public ActionResult AssignRole(string userId)
//        {
//            var user = _userManager.FindByIdAsync(userId);
//            var roles = _roleManager.Roles.ToList();
//            var assignedRoles = _userManager.GetRolesAsync(user);

//            var model = new AssignRoleViewModel
//            {
//                UserId = userId,
//                UserName = user.UserName,
//                Roles = roles.Select(role => new SelectListItem
//                {
//                    Text = role.Name,
//                    Value = role.Name,
//                    Selected = assignedRoles.Contains(role.Name)
//                }).ToList()
//            };

//            return View(model);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult AssignRole(AssignRoleViewModel model)
//        {
//            var user = _userManager.FindByIdAsync(model.UserId);
//            var roles = _roleManager.Roles.ToList();

//            foreach (var role in roles)
//            {
//                if (model.SelectedRoles.Contains(role.Name))
//                {
//                    if (!_userManager.IsInRoleAsync(user, role.Name))
//                    {
//                        _userManager.AddToRoleAsync(user.Id, role.Name);
//                        // Thêm thông tin vào table có role tương ứng
//                        if (role.Name == "khachhang")
//                        {
//                            var khachhang = new KhachHang
//                            {
//                                UserID = user.Id,
//                            };
//                            db.KhachHangs.Add(khachhang);
//                        }
//                        if (role.Name == "nhanvien")
//                        {
//                            var nhanvien = new NhanVien
//                            {
//                                UserID = user.Id,
//                            };
//                            db.NhanViens.Add(nhanvien);
//                        }
//                    }
//                }
//                else
//                {
//                    if (_userManager.IsInRoleAsync(user, role.Name))
//                    {
//                        _userManager.RemoveFromRole(user.Id, role.Name);
//                        // Xóa thông tin từ table có role tương ứng
//                        if (role.Name == "khachhang")
//                        {
//                            db.KhachHangs.RemoveRange(db.KhachHangs.Where(x => x.UserID == user.Id));
//                        }
//                    }
//                }
//            }
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }
    
//        // GET: AspNetUsers_23TH0024
//        public ActionResult Index()
//        {
//            var users = _userManager.Users.ToList();
//            var roles = _roleManager.Roles.ToList();
//            var model = users.Select(user => new UserRolesViewModel
//            {
//                UserId = user.Id,
//                UserName = user.UserName,
//                Email = user.Email,
//                PhoneNumber = user.PhoneNumber,
//                Roles = (List<string>)_userManager.GetRoles(user.Id)
//            }).ToList();

//            return View(model);
//        }

//        // GET: AspNetUsers_23TH0024/Details/5
//        public ActionResult Details(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
//            if (aspNetUser == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetUser);
//        }

//        // GET: AspNetUsers_23TH0024/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: AspNetUsers_23TH0024/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Create([Bind(Include = "UserName,Email,PasswordHash, PhoneNumber")] AspNetUser aspNetUser)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = new ApplicationUser { UserName = aspNetUser.UserName, Email = aspNetUser.Email, PhoneNumber = aspNetUser.PhoneNumber };
//                var result = await _userManager.CreateAsync(user, aspNetUser.PasswordHash);
//                if (result.Succeeded) {
//                    return RedirectToAction("Index");
//                }
//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError("", error);
//                }
//            }

//            return View(aspNetUser);
//        }

//        // GET: AspNetUsers_23TH0024/Edit/5
//        public ActionResult Edit(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
//            if (aspNetUser == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetUser);
//        }

//        // POST: AspNetUsers_23TH0024/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,Email, PhoneNumber")] AspNetUser aspNetUser)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await _userManager.FindByIdAsync(aspNetUser.Id);
//                user.UserName = aspNetUser.UserName;
//                user.Email = aspNetUser.Email;
//                user.PhoneNumber = aspNetUser.PhoneNumber;
//                var updateResult = await _userManager.UpdateAsync(user);
//                if (updateResult.Succeeded)
//                {
//                    return RedirectToAction("Index");
//                }
//                else
//                {
//                    foreach (var error in updateResult.Errors)
//                    {
//                        ModelState.AddModelError(string.Empty, error);
//                    }
//                }
//            }
//            return View(aspNetUser);
//        }

//        // GET: AspNetUsers_23TH0024/Delete/5
//        public ActionResult Delete(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
//            if (aspNetUser == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aspNetUser);
//        }

//        // POST: AspNetUsers_23TH0024/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(string id)
//        {
//            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
//            db.AspNetUsers.Remove(aspNetUser);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
