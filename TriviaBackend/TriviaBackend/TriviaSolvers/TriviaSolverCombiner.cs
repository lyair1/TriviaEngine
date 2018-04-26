using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TriviaBackend.DataContracts;

namespace TriviaBackend.TriviaSolvers
{
    public class TriviaSolverCombiner
    {
        private List<Tuple<ITriviaSolver, int>> TriviaSolvers { set; get; }

        public TriviaSolverCombiner(List<Tuple<ITriviaSolver, int>> triviaSolvers)
        {
            this.TriviaSolvers = triviaSolvers;
        }

        public async Task<TriviaQuestionResponse> SolveQuestions(string question, string answer1, string answer2, string answer3)
        {
            List<TriviaQuestionResponse> responses = new List<TriviaQuestionResponse>();

            foreach(var solver in this.TriviaSolvers)
            {
                responses.Add(await solver.Item1.SolveQuestion(question, answer1, answer2, answer3));
            }

            Dictionary<string, double> answers = new Dictionary<string, double>();
            answers.Add(responses.First().Answers[0].Answer, 0);
            answers.Add(responses.First().Answers[1].Answer, 0);
            answers.Add(responses.First().Answers[2].Answer, 0);

            int i = 0;
            foreach(var response in responses)
            {
                answers[response.Answers[0].Answer] += response.Answers[0].Score * this.TriviaSolvers[i].Item2;
                answers[response.Answers[1].Answer] += response.Answers[1].Score * this.TriviaSolvers[i].Item2;
                answers[response.Answers[2].Answer] += response.Answers[2].Score * this.TriviaSolvers[i].Item2;
            }

            var finalAnswers = new List<AnswerScore>();

            foreach(var answer in answers.Keys)
            {
                finalAnswers.Add(new AnswerScore(answer, answers[answer]));
            }

            return new TriviaQuestionResponse()
            {
                Question = responses.First().Question,
                Answers = finalAnswers.OrderByDescending(a => a.Score).ToList()
            };
        }
    }
}