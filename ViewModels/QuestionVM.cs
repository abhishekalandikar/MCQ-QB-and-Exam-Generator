using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuestionBanks.ViewModels
{
    public class QuestionVM
    {
        public long ID { get; set; }

        [Column("Question")]
        public string Question1 { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public short CorrectAnswer { get; set; }

        public int? SelectedAnswer { get; set; }


        public HttpPostedFileBase Image { get; set; }
        public string AnswerHint{ get; set; }
    }
    
}