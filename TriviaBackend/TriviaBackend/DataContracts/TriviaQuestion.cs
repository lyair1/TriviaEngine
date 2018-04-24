using System;
using System.Runtime.Serialization;

namespace TriviaBackend.DataContracts
{
    [Serializable]
    [DataContract]
    public class TriviaQuestion
    {
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
    }
}