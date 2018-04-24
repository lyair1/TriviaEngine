using System.Threading.Tasks;

namespace BETest.TriviaSolvers
{
    interface ITriviaSolver
    {
        Task<string> SolveQuestion(string question, string answer1, string answer2, string answer3);
    }
}
