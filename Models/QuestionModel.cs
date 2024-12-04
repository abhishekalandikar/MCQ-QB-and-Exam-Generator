using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuestionBanks.Models
{
    public class QuestionModel
    {       
            public long ID { get; set; }  
            public int InstituteID { get; set; }
            public int ImportedFileID { get; set; }

            [Required]  // Ensures Question is mandatory
            [Column("Question")]
            public string Question1 { get; set; }        

       
            public string Option1 { get; set; }

            [Required]
            public string Option2 { get; set; }

            [Required]
            public string Option3 { get; set; }

            [Required]
            public string Option4 { get; set; }
        [Required]
        public string Option5 { get; set; }  
        [Required]
        public string Option6 { get; set; }  

            public short? OptionCount { get; set; }

            [Required]
            public string AnswerHint { get; set; }

            public string OptionHint1 { get; set; }
            public string OptionHint2 { get; set; }
            public string OptionHint3 { get; set; }
            public string OptionHint4 { get; set; }
            public string OptionHint5 { get; set; }
            public string OptionHint6 { get; set; }

            public string QuestionInfo { get; set; }

            public bool IsMultichoise { get; set; }  
            public short CorrectAnswer { get; set; }  

            public bool IsCorrectAnswer1 { get; set; }
            public bool IsCorrectAnswer2 { get; set; }
            public bool IsCorrectAnswer3 { get; set; }
            public bool IsCorrectAnswer4 { get; set; }
            public bool IsCorrectAnswer5 { get; set; }
            public bool IsCorrectAnswer6 { get; set; }

            
            public int Marks { get; set; }

           
            public int Time { get; set; }

            
            public short Complexity { get; set; }

            public int CertificationID { get; set; }
            public int StandardID { get; set; }
            public int SubjectID { get; set; }
            public int TopicID { get; set; }

            public HttpPostedFileBase Image { get; set; }
    }

    }
