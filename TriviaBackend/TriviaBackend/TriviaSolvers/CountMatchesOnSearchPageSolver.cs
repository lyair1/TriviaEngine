using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriviaBackend.DataContracts;

namespace TriviaBackend.TriviaSolvers
{
    class CountMatchesOnSearchPageSolver : ITriviaSolver
    {
        GoogleSearchClient gsc;

        public CountMatchesOnSearchPageSolver()
        {
            this.gsc = new GoogleSearchClient();
        }

        public async Task<TriviaQuestionResponse> SolveQuestion(string question, string answer1, string answer2, string answer3)
        {
            List<AnswerScore> answers = new List<AnswerScore>();
            answers.Add(new AnswerScore(answer1, await this.gsc.SearchAndCountMatchesOnPage(question, answer1)));
            answers.Add(new AnswerScore(answer2, await this.gsc.SearchAndCountMatchesOnPage(question, answer2)));
            answers.Add(new AnswerScore(answer3, await this.gsc.SearchAndCountMatchesOnPage(question, answer3)));
            return new TriviaQuestionResponse()
            {
                Question = question,
                Answers = answers.OrderByDescending(a => a.Score).ToList()
            };
        }
    }
}
