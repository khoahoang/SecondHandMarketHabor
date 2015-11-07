using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using TraoDoiDoCu.DataAcessLayer;
using TraoDoiDoCu.Models.Account;

namespace TraoDoiDoCu.BusinessLayer
{
    public class AccountBUS
    {
        AccountDAL dal = new AccountDAL();
        #region # Phần login
        // kiểm tra login thành công ko?
        public bool LoginIsValid(string UserName, string Password)
        {
            return dal.LoginIsValid(UserName, Hash(Password));
        }
        // kiểm tra account đã active
        public bool IsActiveAccount(string UserName)
        {
            return dal.IsActivedAccount(UserName);
        }
        #endregion

        #region # phần register
        // kiểm tra tài khoản đã tồn tại hay chưa
        public bool CheckExistenceAccount(string Email, string UserName)
        {
            return dal.CheckExistenceAccount(Email, UserName);
        }
        // thêm tài khoản mới vào hệ thống
        public bool AddAccount(RegisterViewModel model)
        {
            model.Password = Hash(model.Password);
            string result = dal.AddAccount(model);
            if(result != null)
            {
                SendActivationEmail(model, result);
                return true;
            }
            return false;
        }
        // send mail active account
        private void SendActivationEmail(RegisterViewModel model, string Active_code)
        {
            using (MailMessage mm = new MailMessage("khoa.kiet.pttkpm@gmail.com", model.Email))
            {
                mm.Subject = "Email xác nhận tài khoản trên website Trao đổi đồ cũ";
                string body = "Thân chào, " + model.UserName + ",";
                body += "<br /><br />Cảm ơn bạn đã đăng ký tài khoản trên website Trao đổi đồ cũ, xin hãy click vào đường link bên dưới để kích hoạt tài khoản.";
                body += "<br /><a href = 'http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Account/Activate?Username=" + model.UserName + "&ActivationCode=" + Active_code + "'>Click vào đây để kích hoạt tài khoản.</a>";
                body += "<br /><br />Thanks";
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("khoa.kiet.pttkpm@gmail.com", "khoahoangtuankiet");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
        #endregion     
    
        #region #phần hash
        // hash password
        public string Hash(string password)
        {
            var hash = Encoding.ASCII.GetBytes("san" + password);
            var sha1 = new SHA1CryptoServiceProvider();
            byte[] sha1hash = sha1.ComputeHash(hash);
            var hashedPassword = System.Text.Encoding.ASCII.GetString(sha1hash);
            return hashedPassword;
        }
        #endregion

        #region #phần activate account
        // active account
        public bool ActivateAccount(string Username, string ActivationCode)
        {
            return dal.ActivateAccount(Username, ActivationCode);
        }
        #endregion
        #region #phần forgot password
        public bool CheckMailExistence(string Email)
        {
            return dal.CheckMailExistence(Email);
        }
        public bool SetUpResetPassword(string email)
        {
            string reset_code = dal.SetUpResetPassword(email);
            if (reset_code == null)
                return false;
            SendResetPasswordEmail(email, reset_code);
            return true;
        }
        private void SendResetPasswordEmail(string Email, string reset_code)
        {
            using (MailMessage mm = new MailMessage("khoa.kiet.pttkpm@gmail.com", Email))
            {
                mm.Subject = "Email reset tài khoản trên hệ thống Trao đổi đồ cũ";
                string body = "<br />Vừa có một yêu cầu lấy lại mật khẩu được gửi vào tài khoản của bạn, để lấy lại mật khẩu xin click vào đường link bên dưới.";
                body += "<br /><a href = '" + "http://localhost:3726/Account/ResetPassword?Email=" + Email + "&ResetCode=" + reset_code + "'>Click vào đây để lấy lại mật khẩu.</a>";
                body += "<br /><br />Thanks";
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("khoa.kiet.pttkpm@gmail.com", "khoahoangtuankiet");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
        public bool ChekcResetPassword(string Email, string ResetCode)
        {
            return dal.CheckResetPassword(Email, ResetCode);
        }
        public bool ChangePassword(ResetPasswordViewModel ressetPassVM)
        {
            return dal.ResetPassword(ressetPassVM);            
        }
        #endregion        
    
        
    }
}