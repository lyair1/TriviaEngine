using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BETest.TriviaSolvers
{
    class CountMatchesOnSearchPageSolver : ITriviaSolver
    {
        GoogleSearchClient gsc;

        public CountMatchesOnSearchPageSolver()
        {
            this.gsc = new GoogleSearchClient();
        }

        public async Task<string> SolveQuestion(string question, string answer1, string answer2, string answer3)
        {
            List<Tuple<string, int>> answers = new List<Tuple<string, int>>();
            answers.Add(new Tuple<string, int>(answer1, await this.gsc.SearchAndCountMatchesOnPage(question, answer1)));
            answers.Add(new Tuple<string, int>(answer2, await this.gsc.SearchAndCountMatchesOnPage(question, answer2)));
            answers.Add(new Tuple<string, int>(answer3, await this.gsc.SearchAndCountMatchesOnPage(question, answer3)));
            return answers.OrderByDescending(a => a.Item2).First().Item1;
        }
    }
}
