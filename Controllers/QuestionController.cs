using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuestionBanks.Models;
using QuestionBanks.ViewModels;
using QuestionBanks.Entities;
using System.Data.Entity.Validation;
using System.IO;

namespace QuestionBanks.Controllers
{
    public class QuestionController : Controller
    {
        private institute_parikshaEntities db = new institute_parikshaEntities();

        
        public ActionResult Index()
        {
            return View(db.Questions.ToList());
        }

        public ActionResult CreateExam()
        {
            
            List<QuestionVM> questions = db.Questions
                                            .OrderBy(q => Guid.NewGuid()) 
                                            .Take(5)                      
                                            .Select(q => new QuestionVM
                                            {
                                                ID = q.ID,
                                                Question1 = q.Question1,
                                             
                                                Option1 = q.Option1,
                                                Option2 = q.Option2,
                                                Option3 = q.Option3,
                                                Option4 = q.Option4,
                                                Option5 = q.Option5,
                                                Option6 = q.Option6
                                            })
                                            .ToList();

            return View(questions); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateExam(List<int> selectedAnswers, List<long> questionIds)
        {
            if (selectedAnswers == null || questionIds == null)
            {
                TempData["Error"] = "Please answer all questions.";
                return RedirectToAction("CreateExam");
            }

            try
            {
                // Initialize variables for storing results
                int correctAnswers = 0;
                int totalQuestions = questionIds.Count;

                // Process each question and compare the answers
                for (int i = 0; i < totalQuestions; i++)
                {
                    var questionId = questionIds[i];
                    var selectedAnswer = selectedAnswers[i];

                    // Get the correct answer from the database
                    var correctAnswer = db.Questions
                        .Where(q => q.ID == questionId)
                        .Select(q => q.CorrectAnswer)
                        .FirstOrDefault();

                    // Increment correct answers if the selected answer matches
                    if (selectedAnswer.ToString() == correctAnswer.ToString())
                    {
                        correctAnswers++;
                    }
                }

             
                TempData["CorrectAnswers"] = correctAnswers;
                TempData["TotalQuestions"] = totalQuestions;

                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("An error occurred: " + ex.Message);
                TempData["Error"] = "An error occurred while submitting your exam. Please try again later.";
                return RedirectToAction("CreateExam");
            }
        }

        // GET: Question/Result
        public ActionResult Result()
        {
            ViewBag.CorrectAnswers = TempData["CorrectAnswers"];
            ViewBag.TotalQuestions = TempData["TotalQuestions"];
            return View();
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }


        
        public ActionResult Create()
        {
            return View();
        }

         [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionVM model)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var question = new Question
                    {
                        ID = model.ID,  
                        Question1 = model.Question1,
                        Option1 = model.Option1,
                        Option2 = model.Option2,
                        Option3 = model.Option3,
                        Option4 = model.Option4,
                        Option5 = model.Option5,
                        Option6 = model.Option6,
                        CorrectAnswer = model.CorrectAnswer,
                        AnswerHint = model.AnswerHint
                    };

                   
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        var imageDirectory = Server.MapPath("~/Images/");

                        
                        if (!Directory.Exists(imageDirectory))
                        {
                            Directory.CreateDirectory(imageDirectory);
                        }

                        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Image.FileName)}";

                        var imagePath = Path.Combine(imageDirectory, uniqueFileName);
                        model.Image.SaveAs(imagePath);

                       question.Image = System.IO.File.ReadAllBytes(imagePath);
                    }

                    
                    db.Questions.Add(question);

                    
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }

                    
                    ModelState.AddModelError("", "There was a validation error while saving the question.");
                }
                catch (Exception ex)
                {
                   
                    System.Diagnostics.Debug.WriteLine("An error occurred: " + ex.Message);
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                }
            }

           
            return View(model);
        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            var model = new QuestionVM
            {
                ID = question.ID,  
                Question1 = question.Question1,
                Option1 = question.Option1,
                Option2 = question.Option2,
                Option3 = question.Option3,
                Option4 = question.Option4,
                Option5 = question.Option5,
               Option6 = question.Option6,
                CorrectAnswer = question.CorrectAnswer,
                AnswerHint = question.AnswerHint,
                
            };

            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var question = db.Questions.Find(model.ID);
                    if (question == null)
                    {
                        return HttpNotFound(); 
                    }

                    
                    question.Question1 = model.Question1;
                    question.Option1 = model.Option1;
                    question.Option2 = model.Option2;
                    question.Option3 = model.Option3;
                    question.Option4 = model.Option4;
                    question.Option5 = model.Option5;
                    question.Option6 = model.Option6;
                    question.CorrectAnswer = model.CorrectAnswer;
                    question.AnswerHint = model.AnswerHint;

                  
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        var imageDirectory = Server.MapPath("~/Images/");

                       
                        if (!Directory.Exists(imageDirectory))
                        {
                            Directory.CreateDirectory(imageDirectory);
                        }

                        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Image.FileName)}";

                      
                        var imagePath = Path.Combine(imageDirectory, uniqueFileName);
                        model.Image.SaveAs(imagePath);

                 
                        question.Image = System.IO.File.ReadAllBytes(imagePath);

                    }
                    
                 
                    db.SaveChanges();

                    
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                 
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                    ModelState.AddModelError("", "There was a validation error while updating the question.");
                }
                catch (Exception ex)
                {
                 
                    System.Diagnostics.Debug.WriteLine("An error occurred: " + ex.Message);
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                }
            }

         
            return View(model);
        }


        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
