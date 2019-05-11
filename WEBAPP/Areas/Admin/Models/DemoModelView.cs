using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WEBAPP.Areas.Admin.Models
{
    public class DemoModelView
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDay { get; set; }
        public float Saraly { get; set; }

        public List<string> DDL1 { get; set; }

        public IEnumerable<DDL1> modelDDL1
        {

            get
            {

                List<DDL1> lis1 = new List<DDL1>();
                for (int i = 1; i <= 10; i++)
                {
                    lis1.Add(new DDL1
                    {
                        Id = i,
                        Name = "THAI" + i
                    });
                }


                return lis1;

            }
        }
        public IEnumerable<DDL2> modelDDL2
        {

            get
            {

                List<DDL2> lis1 = new List<DDL2>();
                for (int i = 1; i <= 10; i++)
                {
                    lis1.Add(new DDL2
                    {
                        Id = i,
                        DDL1_Id = i % 3,
                        Name = "Bangkok" + (i + 2)
                    });
                }


                return lis1;

            }
        }

        public FileUpload FileUpload { get; set; }
        public List<FileUpload> FileUpload2 { get; set; }
    }

    public class FileUpload
    {
        public HttpPostedFileBase File { get; set; }
        public string Signature { get; set; }
        public int? ROW_ID { get; set; }
        public string NAME { get; set; }
        public string TYPE { get; set; }
        public decimal? SIZE { get; set; }
        public DateTime? UPLOAD_DATE { get; set; }
    }

    public class DDL1
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DDL2
    {
        public int Id { get; set; }
        public int DDL1_Id { get; set; }
        public string Name { get; set; }
    }


    public class DemoModel
    {


    }

    public class WizardStep
    {

        public StepOne step1 { get; set; }

        public StepTwo step2 { get; set; }
    }

    public class StepOne
    {
        [Display(Name = "NameStepOne")]
        [Required]
        [MaxLength(5)]
        public string NameStepOne { get; set; }

    }
    public class StepTwo
    {

        [Display(Name = "NameStepOneTwo")]
        [Required]
        [MaxLength(5)]
        public string NameStepOneTwo { get; set; }

    }
    public class DemoGridModel
    {
        public string data1 { get; set; }
        public DateTime? data2 { get; set; }
        public int? data3 { get; set; }
        public decimal? data4 { get; set; }
        public double? data5 { get; set; }
        public long? data6 { get; set; }

        private bool _RowEdit = false;

        public bool RowEdit
        {
            get { return _RowEdit; }
            set { _RowEdit = value; }
        }


    }
}