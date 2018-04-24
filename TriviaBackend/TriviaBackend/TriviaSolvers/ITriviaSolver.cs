using System.Threading.Tasks;
using TriviaBackend.DataContracts;

namespace TriviaBackend.TriviaSolvers
{
    interface ITriviaSolver
    {
        Task<TriviaQuestionResponse> SolveQuestion(string question, string answer1, string answer2, string answer3);
    }
}
