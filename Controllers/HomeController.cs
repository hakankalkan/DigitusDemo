using DigitusDemo.Data;
using DigitusDemo.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DigitusDemo.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }
        
        private DigitusDemoContext db = new DigitusDemoContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //Burada kullanıcılar login olunca isOnline kısmı true yapılacaktı. Session kullanılabilirdi
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            var encodedPass = Password.GetHashCode().ToString();
            var user = db.Users.FirstOrDefault(u => u.Email == Email && u.Password==encodedPass);
            
            if (user != null && user.IsAdmin)
            {
                return RedirectToAction("HomePageAdm");
            }
            else if (user != null)
            {
                return RedirectToAction("HomePage");
            }
            else
            {
                return RedirectToAction("WrongEmailorPass");
            }
            
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(string Name, string Surname, string Email,string Password)
        {
            
            int validationCode = new Random().Next(100000, 999999);
            User user = new User{
                Name= Name,
                Surname= Surname,
                Email= Email,
                Password= Password.GetHashCode().ToString(),
                IsAdmin=false,
                IsValid=false,
                ValidationCode = validationCode.ToString(),
                RegisteredDate=null,
                IsOnline=false,
                RegCodeSent=false,
                RegCodeSentTime=null
            };

            db.Users.Add(user);
            db.SaveChanges();
            int userId = db.Users.OrderByDescending(x => x.UserId).FirstOrDefault().UserId;
            SendVerificationEmail(userId);
            ViewData["uid"] = userId;
            return View();
        }

        //Verifiation mail işlemi. Burada google güvenlik açısından sorun çıkardığı için ethereal isimli siteden fake mail hesabı  kullanıldı
        private void SendVerificationEmail(int userId)
        {
            
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);
            var smtp = new SmtpClient("smtp.ethereal.email", 587);
            smtp.EnableSsl= true;
            smtp.Timeout = 100000;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential("monroe.moore32@ethereal.email", "RQW71W5Xx9JFcwKHM1");

            MailMessage mail = new MailMessage("monroe.moore32@ethereal.email", user.Email,"Verification","Your verification code is : " + user.ValidationCode);
            mail.IsBodyHtml = true;
            mail.BodyEncoding = UTF8Encoding.UTF8;
            try
            {
                smtp.Send(mail);

                user.RegCodeSent= true;
                user.RegCodeSentTime= DateTime.Now;
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                string s = ex.Message;
                
            
            }
            
            


        }


        //Validation işlemi
        [HttpPost]
        public ActionResult Validation(string ValidationCode, string uid)
        {
            int userId = Convert.ToInt32(uid);
            var user = db.Users.Where(x => x.UserId == userId).FirstOrDefault();
            if (user != null && user.ValidationCode == ValidationCode)
            {
                user.IsValid = true;
                user.IsOnline = true;
                user.RegisteredDate = DateTime.Now;
                db.SaveChanges();
            }
            
            return RedirectToAction("HomePage");
        }

        public ActionResult HomePage()
        {
            return View();
        }

        public ActionResult HomePageAdm()
        {
            return View();
        }

        //Bütün admin işlemleri burada yapıldı. 
        [HttpPost]
        public JsonResult NewUsers(string startDate, string endDate)
        {
            var users = db.Users.ToList();
            int newUserCount = 0;
            int RegAfterOneDay = 0;
            DateTime date1 = DateTime.Parse(startDate);
            DateTime date2 = DateTime.Parse(endDate);
            newUserCount = users.Where(x =>date1 <= x.RegisteredDate && date2 > x.RegisteredDate).Count();
            RegAfterOneDay = users.Where(x=>x.RegCodeSent == true && x.IsValid == false && x.RegCodeSentTime > DateTime.Now.AddDays(1)).Count();

            double totalTime = 0;
            foreach (var item in users)
            {
                totalTime += (item.RegisteredDate - item.RegCodeSentTime).Value.TotalSeconds * 1000;
            }

            int avgTotalTime = Convert.ToInt32(totalTime) / users.Count();
            List<int> result = new List<int>
            {
                newUserCount,
                RegAfterOneDay,
                avgTotalTime
            };
            return Json(result);
        }
    }
}