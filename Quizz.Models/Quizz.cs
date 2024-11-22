using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class Quizz
    {
        public string Question { get; set; }

        public string optionA { get; set; }
        public string optionB { get; set; }

        public string optionC { get; set; }

        public string optionD { get; set; }

        public Type Type { get; set; }

    }
}
