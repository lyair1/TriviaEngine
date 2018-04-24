using System.Collections.Generic;

namespace TriviaBackend.DataContracts
{
    public class TriviaQuestionResponse
    {
        public string Question { get; set; }

        public List<AnswerScore> Answers { get; set; }
    }
}