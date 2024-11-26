﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class Quizz
    {
        public int Id { get; set; }
        public string Question { get; set; }

        public string[] Options { get; set; }

        public int CorrectOpinionIndex { get; set; }

        public int AuthorId { get; set; }
    }
}
