using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriviaBackend.DataContracts;

namespace TriviaBackend.TriviaSolvers
{
    class QuestionAnswerMatchPercentSolver : ITriviaSolver
    {
        GoogleSearchClient gsc;

        public QuestionAnswerMatchPercentSolver()
        {
            this.gsc = new GoogleSearchClient();
        }

        public async Task<TriviaQuestionResponse> SolveQuestion(string question, string answer1, string answer2, string answer3)
        {
            List<AnswerScore> answers = new List<AnswerScore>();
            answers.Add(new AnswerScore(
                answer1,
                1.0 * await this.gsc.SearchAndCountResults(string.Format("({0}) AND \"{1}\"", question, answer1)) /
                    await this.gsc.SearchAndCountResults(answer1)));
            answers.Add(new AnswerScore(
                answer2,
                1.0 * await this.gsc.SearchAndCountResults(string.Format("({0}) AND \"{1}\"", question, answer2)) /
                    await this.gsc.SearchAndCountResults(answer2)));
            answers.Add(new AnswerScore(
                answer3,
                1.0 * await this.gsc.SearchAndCountResults(string.Format("({0}) AND \"{1}\"", question, answer3)) /
                    await this.gsc.SearchAndCountResults(answer3)));
            return new TriviaQuestionResponse()
            {
                Question = question,
                Answers = answers.OrderByDescending(a => a.Score).ToList()
            };
        }
    }
}
