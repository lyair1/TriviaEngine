using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TriviaBackend.DataContracts;
using TriviaBackend.TriviaSolvers;

namespace TriviaBackend.Controllers
{
    public class TriviaController : Controller
    {
        private ITriviaSolver triviaSolver;

        public TriviaController()
        {
            triviaSolver = new CountMatchesOnSearchPageSolver();
        }
        // GET: Trivia
        public TriviaQuestion Index()
        {
            return new TriviaQuestion()
            {
                Question = "Question?",
                Answer1 = "answer2",
                Answer2 = "answer3",
                Answer3 = "answer3"
            };
        }

        // GET: Trivia
        public JsonResult Index2()
        {
            return Json(
                new TriviaQuestion()
                {
                    Question = "Question?",
                    Answer1 = "answer2",
                    Answer2 = "answer3",
                    Answer3 = "answer3"
                },
                JsonRequestBehavior.AllowGet);
        }

        // POST: AnswerQuestion
        [HttpPost]
        public async Task<JsonResult> AnswerQuestion(TriviaQuestion triviaQuestion)
        {
            return Json(await this.triviaSolver.SolveQuestion(
                triviaQuestion.Question,
                triviaQuestion.Answer1,
                triviaQuestion.Answer2,
                triviaQuestion.Answer3));
        }
    }
}