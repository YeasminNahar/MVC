using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice_one.Models.VM
{
    public class StudentVM
    {
        public StudentVM()
        {
            this.SubjectList = new List<Addmission>();
        }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public bool IsAddmitted { get; set; }
        public string Picture { get; set; }
        public HttpPostedFileBase PictureFile { get; set; }
        public List<Addmission> SubjectList { get; set; }
       /* public List<Addmission> Addmissions { get; set; } */// Add this
        //public List<Subject> listSubjects { get; set; } // Add this
    }
}