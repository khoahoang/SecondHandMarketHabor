using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TraoDoiDoCu.BusinessLayer;
using TraoDoiDoCu.Models.Account;

namespace TraoDoiDoCu.Controllers.Account
{
    public class AccountController : Controller
    {
        AccountBUS bus = new AccountBUS();
        //
        // GET: /Account/login
        public ActionResult Login()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View("Login");
        }

        //
        // POST: /Account/login
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {                 
                if (bus.LoginIsValid(model.UserName,model.Password))
                {                    
                    if(!bus.IsActiveAccount(model.UserName))
                    {
                        ModelState.AddModelError("", "Tài khoản chưa được kích hoạt");
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                        return RedirectToAction("Index", "Home");
                    }                    
                }
                else
                {
                    ModelState.AddModelError("", "Sai tên tài khoản hoặc mật khẩu");
                }
            }
            return View(model);
        }

        //
        // GET: /Account/logout
        public ActionResult Logout()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            FormsAuthentication.SignOut();
            return View("Logout");
        }

        //
        // GET: /Account/register
        public ActionResult Register()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View("Register");
        }

        //
        // Post: /Account/register
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (bus.CheckExistenceAccount(model.Email, model.UserName))
                {
                    if (bus.AddAccount(model))
                    {
                        ViewBag.Message = "Tài khoản đã được ghi nhận trong hệ thống. Xin vui lòng kiểm tra email để kích hoạt tài khoản.";
                        return View("Result");
                    }
                    else
                    {
                        ViewBag.Alert = "Chúng tôi không thể gửi email kích hoạt đến email của bạn. Xin lỗi vì sự bất tiện này.";
                        return View("Result");
                    }
                }
                else
                {
                    ViewBag.Message = "Tài khoản đã tồn tại trong hệ thống.";
                    return View("Register");
                }
            }
            return View(model);
        }
        // GET: /Account/Activate
        [HttpGet]
        public ActionResult Activate(string Username, string ActivationCode)
        {
            bool res = bus.ActivateAccount(Username, ActivationCode);
            ViewBag.Title = "Kết quả kích hoạt tài khoản";
            if (res)
            {                
                ViewBag.Message = "Kích hoạt tài khoản thành công. Bạn có thể đăng nhập tài khoản ngay bây giờ.";
                return View("Result");
            }
            else
            {
                ViewBag.Alert = "Kích hoạt tài khoản thất bại. Xin liên lạc với ban quản trị để biết thêm chi tiết.";
                return View("Result");
            }
        }
        // GET: /Account/ForgotPassword
        public ActionResult ForgotPassword()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View("ForgotPassword");
        }
        // POST: /Account/ForgotPassword
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (bus.CheckMailExistence(model.Email))
                {
                    ViewBag.Title = "Kết quả lấy lại mật khẩu";
                    if (bus.SetUpResetPassword(model.Email))
                    {
                        ViewBag.Message = "Xin vui lòng kiểm tra email để đổi lại mật khẩu.";
                        return View("Result");
                    }
                    else
                    {
                        ViewBag.Alert = "Chúng tôi không thể gửi mail lấy lại mật khẩu đến email của bạn. Chúng tôi xin lỗi về sự bất tiện này.";
                        return View("Result");
                    }
                }
                else
                {
                    ViewBag.Alert = "Email không tồn tại.";
                    return View("ForgotPassword");
                }
            }
            return View(model);
        }
        // GET: /Account/ResetPassword
        [HttpGet]
        public ActionResult ResetPassword(string Email, string ResetCode)
        {
            if (bus.ChekcResetPassword(Email, ResetCode))
            {
                return View("ResetPassword");
            }
            else
            {
                ViewBag.Title = "Kết quả đổi mật khẩu";
                ViewBag.Message = "Link đổi mật khẩu đã quá hạn.";
                return View("Result");
            }
        }
        // POST: /Account/ResetPassword
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPassVM)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Title = "Kết reset mật khẩu";
                if (bus.ChangePassword(resetPassVM))
                {
                    ViewBag.Message = "Đổi khẩu thành công.";
                    return View("Result");
                }
                else
                {
                    ViewBag.Message = "Đổi mật khẩu thất bại.";
                    return View("Result");
                }
            }
            return View(resetPassVM);
        }
	}
}