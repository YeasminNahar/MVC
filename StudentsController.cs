using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Practice_one.Models;
using Practice_one.Models.VM;
using System.IO;
using System.Diagnostics;

namespace Practice_one.Controllers
{
    public class StudentsController : Controller
    {
        practice1Entities1 db = new practice1Entities1();
        public ActionResult Index()
        {
            var students = db.Students.Include(c => c.Addmissions.Select(b => b.Subject)).OrderByDescending(x => x.StudentId).ToList();
            return View(students);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(StudentVM studentVM, int[] subjectId)
        {
            if (ModelState.IsValid)
            {
                Student student = new Student
                {
                    StudentName = studentVM.StudentName,
                    Age = studentVM.Age,
                    DateOfBirth = studentVM.DateOfBirth,
                    IsAddmitted = studentVM.IsAddmitted


                };
                HttpPostedFileBase file = studentVM.PictureFile;
                if (file != null)
                {
                    string FilePath = Path.Combine("/Images/",
                        DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(FilePath));
                    student.Picture = FilePath;

                }
                foreach (var item in subjectId)
                {
                    Addmission addmission = new Addmission
                    {

                        Student = student,
                        StudentId = student.StudentId,
                        SubjectId = item


                    };
                    db.Addmissions.Add(addmission);


                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult AddNewSubject(int? id)
        {
            ViewBag.subjects = new SelectList(db.Subjects.ToList(), "SubjectId", "SubjectName", (id != null) ? id.ToString() : "");
            return PartialView("Add");

        }
        public ActionResult Delete(int? id)
        {
            var stdnt = db.Students.FirstOrDefault(s => s.StudentId == id);
            var addmissioninfo = db.Addmissions.Where(s => s.StudentId == id).ToList();
            db.Addmissions.RemoveRange(addmissioninfo);
            db.Students.Remove(stdnt);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Edit(int? id)
        {
            ViewBag.subject = db.Subjects.ToList();
            if (id != null)
            {
                var student = db.Students.Find(id);
                if (student != null)
                {
                    var newstudent = new StudentVM()
                    {
                        StudentName = student.StudentName,
                        Age = student.Age,
                        IsAddmitted = student.IsAddmitted,
                        Picture = student.Picture,
                        DateOfBirth = student.DateOfBirth,
                        StudentId = student.StudentId,
                    };
                    var sublist = db.Addmissions.Where(x => x.StudentId == student.StudentId).ToList();
                    newstudent.SubjectList = sublist;
                    return View(newstudent);
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(StudentVM studentVM, int[] subjectId)
        {
            if (ModelState.IsValid)
            {
                var student = db.Students.Find(studentVM.StudentId);
                if (student == null)
                {
                    return HttpNotFound();
                }


                student.StudentName = studentVM.StudentName;
                student.Age = studentVM.Age;
                student.IsAddmitted = studentVM.IsAddmitted;
                student.DateOfBirth = studentVM.DateOfBirth;


                HttpPostedFileBase file = studentVM.PictureFile;
                if (file != null)
                {
                    string fileName = Path.Combine("/Images/", DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(fileName));
                    student.Picture = fileName;
                }
                else
                {
                    student.Picture = studentVM.Picture;
                }

                var rs = db.Addmissions.Where(t => t.StudentId == student.StudentId).ToList();
                foreach (var t in rs)
                {
                    db.Addmissions.Remove(t);
                }
                foreach (var stat in subjectId)
                {
                    var s = db.Subjects.FirstOrDefault(st => st.SubjectId == stat);
                    if (s != null)
                    {
                        var addmission = new Addmission()
                        {
                            Student = student,
                            SubjectId = s.SubjectId,
                            StudentId = student.StudentId,

                        };
                        db.Addmissions.Add(addmission);

                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentVM);
        }
            
           
        }

    }
        
    
