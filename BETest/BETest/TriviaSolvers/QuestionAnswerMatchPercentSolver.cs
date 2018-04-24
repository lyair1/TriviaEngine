using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BETest.TriviaSolvers
{
    class QuestionAnswerMatchPercentSolver : ITriviaSolver
    {
        GoogleSearchClient gsc;

        public QuestionAnswerMatchPercentSolver()
        {
            this.gsc = new GoogleSearchClient();
        }

        public async Task<string> SolveQuestion(string question, string answer1, string answer2, string answer3)
        {
            List<Tuple<string, double>> answers = new List<Tuple<string, double>>();
            answers.Add(new Tuple<string, double>(
                answer1,
                1.0 * await this.gsc.SearchAndCountResults(string.Format("({0}) AND \"{1}\"", question, answer1)) /
                    await this.gsc.SearchAndCountResults(answer1)));
            answers.Add(new Tuple<string, double>(
                answer2,
                1.0 * await this.gsc.SearchAndCountResults(string.Format("({0}) AND \"{1}\"", question, answer2)) /
                    await this.gsc.SearchAndCountResults(answer2)));
            answers.Add(new Tuple<string, double>(
                answer3,
                1.0 * await this.gsc.SearchAndCountResults(string.Format("({0}) AND \"{1}\"", question, answer3)) /
                    await this.gsc.SearchAndCountResults(answer3)));
            return answers.OrderByDescending(a => a.Item2).First().Item1;
        }
    }
}
