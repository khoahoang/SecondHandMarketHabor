using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using TraoDoiDoCu.Models;
using TraoDoiDoCu.Models.Account;

namespace TraoDoiDoCu.DataAcessLayer
{
    public class AccountDAL
    {
        TraoDoiDoCuEntities db = new TraoDoiDoCuEntities();

        #region # phần login
        public bool LoginIsValid(string UserName, string HashPassword)
        {
            User user = db.Users.FirstOrDefault(x => x.UserName == UserName && x.PassWord == HashPassword);
            if (user != null)
                return true;
            return false;
        }
        public bool IsActivedAccount(string UserName)
        {
            User user = db.Users.FirstOrDefault(x => x.UserName == UserName);
            if (user == null)
                return false;
            else if (user.ActiveCode == "active")
                return true;
            return false;
        }
        #endregion

        #region # phần register
        public bool CheckExistenceAccount(string Email, string UserName)
        {
            User user = db.Users.FirstOrDefault(x => x.UserName == UserName || x.Email == Email);
            if (user == null)
                return true;
            return false;
        }
        public string AddAccount(RegisterViewModel model)
        {            
            User user = new User();
            user.UserName = model.UserName;
            user.PassWord = model.Password;
            user.Email = model.Email;
            user.Admin = false;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Phone = model.Phone;

            user.Products = null;

            string activation_code = Guid.NewGuid().ToString();
            user.ActiveCode = activation_code;

            try
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                return null;
            }
            return activation_code;
        }
        #endregion

        #region # phần active account
        public bool ActivateAccount(string UserName, String ActivationCode)
        {
            User user = db.Users.FirstOrDefault(x => x.UserName == UserName && x.ActiveCode == ActivationCode);
            if (user.UserName == null)
                return false;
            user.ActiveCode = "active";
            db.SaveChanges();
            return true;
        }
        #endregion        
    
        private float TIME_OVERDUE_RESET_PASSWORD = 2;
        #region # phần forgot password
        public bool CheckMailExistence(string Email)
        {
            User user = db.Users.FirstOrDefault(x => x.Email == Email);
            if (user != null)
                return true;
            return false;
        }
        public string SetUpResetPassword(string Email)
        {
            User user = db.Users.FirstOrDefault(x => x.Email == Email);
            string reset_pass_code = Guid.NewGuid().ToString();
            user.ResetPassword = reset_pass_code;
            DateTime time = new DateTime();
            time = DateTime.Now;
            user.DateRequest = time;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return null;
            }
            return user.ResetPassword;
        }
        public bool CheckResetPassword(string Email, string ResetCode)
        {
            User user = db.Users.FirstOrDefault(x => x.Email == Email && x.ResetPassword == ResetCode);
            if (user != null)
            {
                DateTime now = DateTime.Now;
                DateTime time = user.DateRequest.Value.ToLocalTime();
                TimeSpan span = now.Subtract(time);
                if (span.Hours > TIME_OVERDUE_RESET_PASSWORD)
                    return false;
                else
                    return true;
            }
            return false;
        }
        public bool ResetPassword(ResetPasswordViewModel ressetPassVM)
        {
            User user = db.Users.FirstOrDefault(x => x.Email == ressetPassVM.Email && x.ResetPassword == ressetPassVM.ResetCode);
            if (user != null)
            {
                try
                {
                    var hash = Encoding.ASCII.GetBytes("san" + ressetPassVM.Password);
                    var sha1 = new SHA1CryptoServiceProvider();
                    byte[] sha1hash = sha1.ComputeHash(hash);
                    var hashedPassword = System.Text.Encoding.ASCII.GetString(sha1hash);

                    user.PassWord = hashedPassword;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }
        #endregion
    }
}