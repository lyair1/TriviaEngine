using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriviaBackend.DataContracts
{
    public class AnswerScore
    {
        public AnswerScore(string answer, double score)
        {
            this.Answer = answer;
            this.Score = score;
        }

        public string Answer { get; set; }
        public double Score { get; set; }
    }
}