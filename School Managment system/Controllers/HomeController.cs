using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using School_Managment_system.Data;
using School_Managment_system.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.Mail;



namespace School_Managment_system . Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _Context;
        public HomeController(ApplicationDbContext context)
        {
            _Context = context;
        }

        public void SendSimpleEmail(string studentEmail, string StudentName)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("obaidqureshi702@gmail.com", "The Knowledge");
            mail.To.Add(studentEmail);
            string url = Url.Action("Home", "Home", null, Request.Scheme);
            mail.Subject = "Welcome to The Knowledge School";
            mail.Body = $"Hello <a href ='url'> Click Here </a>";
            mail.IsBodyHtml = true; 
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("obaidqureshi702@gmail.com", "tykvmpanypbhjwwi");
            smtp.Send(mail);
        }

        public IActionResult Data() 
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var data = _Context.StudentData.ToList();
            ViewBag.IsAdmin = "True";
            return View(data);
        }
        public IActionResult Ranking()
        {
            var Rank = _Context.StudentData.OrderByDescending(X => X.OverallRate).Take(10).ToList();
            

            return View(Rank);
        }
        private bool IsStudent() 
        {
            return HttpContext.Session.GetString("Studentrole") == "True";
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Admin") == "true";
            
        }
        public IActionResult Home() 
        {
            return View();
        }
        public IActionResult Dashboard() 
        {
            var Studnt = HttpContext.Session.GetInt32("Student");
            if(Studnt == null)
            {
                return RedirectToAction("Login");
            }
            var std = _Context.StudentData.Find(Studnt);

            if(std == null) 
            {
                return NotFound();
            }
            return View(std);
        }
        public IActionResult DeleteCard(int id) 
        {
            var remove = _Context.Carddata.Find(id);
            if (remove != null) 
            { 
            _Context.Carddata.Remove(remove);
                _Context.SaveChanges();
            }
            return RedirectToAction ("About");
        }
        public IActionResult Rules() 
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var student = _Context.StudentData.Find(id);


            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpGet]
        public IActionResult Studentd(int Id) 
        {
            var find = _Context.StudentData.Find(Id);
            return View(find);
        }

        [HttpPost]
        public IActionResult Studentd(Student std ) 
        {
            var student = _Context.StudentData.Find(std.Id);
            if (student != null) 
            {
            student.fullName = std.fullName;
                student.email = std.email;
                student.Class = std.Class;
                student.phone = std.phone;
                student.Age = std.Age;
                student.profile = std.profile;
                _Context.SaveChanges();


            }


            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Card(string Title, IFormFile Image, int date, string Description, IFormFile Video)
        {

            try
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Card");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            string imagePath = null;
            string videoPath = null;

            if (Image != null && Image.Length > 0)
            {

                    string imageName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Image.FileName); 
                    string path = Path.Combine(uploadFolder, imageName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
                imagePath = "/Card/" + imageName;


            }

            if (Video != null && Video.Length > 0)
            {
                    string videoName = Guid.NewGuid().ToString() + "_" +Path.GetFileName(Video.FileName);
                    string path1 = Path.Combine(uploadFolder, videoName);

                    using (var stream = new FileStream(path1, FileMode.Create))
                    {
                        Video.CopyTo(stream);
                    }
                    videoPath = "/Card/" + videoName;
                


            }

            var card = new Cards
            {
                Title = Title,
                Image = imagePath,
                Description = Description,
                date = date,
                Video = videoPath
            };

            _Context.Carddata.Add(card);
            _Context.SaveChanges(); 
            }
            catch (Exception ex)
                {
                ViewBag.ErrorMessage = " Upload a short video " + ex.Message;
                return View("About");
            }

            return RedirectToAction("About");
        }
        public IActionResult About()
        {
            ViewBag.IsAdmin = HttpContext.Session.GetString("Admin") == "true";
            var list = _Context.Carddata.ToList();

            return View(list); 
        }
        //public IActionResult CreateEdit( Cards card , IFormFile Image , IFormFile Video )
        //{
        //    var cardd = _Context.Carddata.Find(card.Id);
        //    if(cardd == null) { return NotFound(); }
        //    cardd.Title = card.Title;
        //    cardd.Image = card.Image;
        //    cardd.Description = card.Description;
        //    cardd.Video = card.Video;

        //    if (Image != null && Image.Length > 0)
        //    {
        //        string filename = Guid.NewGuid().ToString() + "_" + Image.FileName; // Unique name banane ke liye
        //        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Card", filename);

        //        using (var stream = new FileStream(path, FileMode.Create))
        //        {
        //            Image.CopyTo(stream);
        //        }

        //        cardd.Image = "/Card/" + filename; // Leading slash (/) zaroori hai
        //    }

        //    // 2. VIDEO UPLOAD LOGIC
        //    if (Video != null && Video.Length > 0)
        //    {
        //        string videoFilename = Guid.NewGuid().ToString() + "_" + Video.FileName;
        //        string videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Card", videoFilename);

        //        using (var stream = new FileStream(videoPath, FileMode.Create))
        //        {
        //            Video.CopyTo(stream);
        //        }

        //        cardd.Video = "/Card/" + videoFilename;
        //    }
        //    _Context.SaveChanges();

        //    return RedirectToAction("About"); 
        //}


        public IActionResult Login()
        {
            return View(); 
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            
                return RedirectToAction("Login" , "Home");
            
            
        }

        [HttpPost]
        public IActionResult Login(string email , string password)
        {
            if(email == "Admin" && password == "salma007") { 
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("Data" , "Home");
           
            }
           var student = _Context.StudentData.FirstOrDefault(X => X.email == email && X.password == password);
           if(student != null) 
            {
                 HttpContext.Session.SetInt32("Student", student.Id);
                HttpContext.Session.SetString("Studentrole" , "True");

                return RedirectToAction("Dashboard", "Home");
            }

            ViewBag.Message = "Invalid email or password";


            return View(); 
        }
        public IActionResult Daycare()
        {
            return View(); 
        }


        [HttpPost]
        public IActionResult Edit(Student student , int stddays , int days , int testrating , int Rating) 
        {
            var estudent = _Context.StudentData.Find(student.Id);
            if (estudent == null) {
                return NotFound();
            }

            double attendence = (double)stddays / days * 100;
            double rating = Rating * 10;
            double Testrating = testrating * 10;

            var OverallRate = (attendence + rating + Testrating) / 3;


            estudent.fullName = student.fullName;
            estudent.fatherName = student.fatherName;
            estudent.email = student.email;
            estudent.phone = student.phone;
            estudent.Age = student.Age;
            estudent.gender = student.gender;
            estudent.Class = student.Class;
            estudent.admissionType = student.admissionType;
            estudent.comments = student.comments;
            estudent.feesstatus = student.feesstatus;
            estudent.OverallRate = (int)OverallRate;

            
            // profile ko touch nahi karna
            // purani image wahi rahegi


            _Context.SaveChanges();
            
            return RedirectToAction("Data" );
        } 
        
        public IActionResult Form()
        {
            return View();
        }
        public IActionResult Delete(int id ) 
        {
            var delete = _Context.StudentData.Find(id);
            if (delete != null) 
            {
                _Context.StudentData.Remove(delete);
                _Context.SaveChanges();

            }
            return RedirectToAction("Data");
        }



        [HttpPost]
        public IActionResult Form(
     string fullName,
     string fatherName,
     string email,
     int phone,
     int Age,
     string gender,
     string Class,
     string admissionType,
     string address,
     string comments,
     string password,
     string confirmPassword,
     IFormFile profile)
        {

            if (password != confirmPassword)
            {
                ViewBag.Message = "Password and Confirm Password do not match";
                return View("Form");
            }


            if (profile == null)
            {
                ViewBag.Message = "Please select a profile image";
                return View("Form");
            }

            // Image ka naam
            string filename = profile.FileName;

            // Image ka path
            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Profile",
                filename
            );

            // Image save
            using (var stream = new FileStream(path, FileMode.Create))
            {
                profile.CopyTo(stream);
            }

            // Student
            var student = new Student
            {
                profile = "/Profile/" + filename,
                fullName = fullName,
                fatherName = fatherName,
                email = email,
                Age = Age,
                phone = phone,
                gender = gender,
                Class = Class,
                admissionType = admissionType,
                comments = comments,
                password = password
            };
         
                _Context.StudentData.Add(student);
            _Context.SaveChanges();


                    SendSimpleEmail(email, fullName);
                
                
            
            return RedirectToAction("Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
