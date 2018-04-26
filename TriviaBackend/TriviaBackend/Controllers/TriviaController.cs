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
        private TriviaSolverCombiner triviaSolverCombiner;

        public TriviaController()
        {
            triviaSolverCombiner = new TriviaSolverCombiner(new List<Tuple<ITriviaSolver, int>>()
                                                                {
                                                                    new Tuple<ITriviaSolver, int>(new CountMatchesOnSearchPageSolver(), 5),
                                                                    new Tuple<ITriviaSolver, int>(new QuestionAnswerMatchPercentSolver(), 10),
                                                                });
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
            var json = Json(await this.triviaSolverCombiner.SolveQuestions(
                triviaQuestion.Question,
                triviaQuestion.Answer1,
                triviaQuestion.Answer2,
                triviaQuestion.Answer3));

            return json;
        }
    }
}