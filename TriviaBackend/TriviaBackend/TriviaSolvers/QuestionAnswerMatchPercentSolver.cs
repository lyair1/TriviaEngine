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
            try
            {
                var qAnswer1 = await this.gsc.SearchAndCountResults(string.Format("\"{0}\"", answer1));
                var qAnswer2 = await this.gsc.SearchAndCountResults(string.Format("\"{0}\"", answer2));
                var qAnswer3 = await this.gsc.SearchAndCountResults(string.Format("\"{0}\"", answer3));
                var qQuestion = await this.gsc.SearchAndCountResults(string.Format("\"{0}\"", question));

                var resultCount1 = 1.0 * await this.gsc.SearchAndCountResults(string.Format("({0}) AND \"{1}\"", question, answer1)) / (qAnswer1 + qQuestion);
                var resultCount2 = 1.0 * await this.gsc.SearchAndCountResults(string.Format("({0}) AND \"{1}\"", question, answer2)) / (qAnswer2 + qQuestion);
                var resultCount3 = 1.0 * await this.gsc.SearchAndCountResults(string.Format("({0}) AND \"{1}\"", question, answer3)) / (qAnswer3 + qQuestion);

                var totalCount = resultCount1 + resultCount2 + resultCount3;

                List<AnswerScore> answers = new List<AnswerScore>();
                answers.Add(new AnswerScore(
                    answer1, totalCount == 0 ? 0 :
                    1.0 * resultCount1 / totalCount));
                answers.Add(new AnswerScore(
                    answer2, totalCount == 0 ? 0 :
                    1.0 * resultCount2 / totalCount));
                answers.Add(new AnswerScore(
                    answer3, totalCount == 0 ? 0 :
                    1.0 * resultCount3 / totalCount));

                return new TriviaQuestionResponse()
                {
                    Question = question,
                    Answers = answers.OrderByDescending(a => a.Score).ToList()
                };
            }
            catch (Exception e)
            {
                var i = 0;
                throw;
            }

        }
    }
}
