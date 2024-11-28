using QC_Quizz_App;
using Quiz.Models;
using System.Text.Json;

namespace QC_Quizz_App
{
    public class QuizzRepository
    {
        private readonly string _filePath;
        private List<Quizz> _quiz;


        public QuizzRepository(string filepath)
        {
            _filePath = filepath;
            _quiz = LoadQuizzes();
        }

        public void CreateQuiz(Quizz quiz)
        {
            quiz.Id = _quiz.Any() ? _quiz.Max(x => x.Id) + 1 : 1;
            _quiz.Add(quiz);
            SaveData();
        }

        public void CreateQuiz(int authorId)
        {
            var quiz = new Quizz();

            Console.WriteLine("Enter the title for your quiz:");
            quiz.Title = Console.ReadLine();

            quiz.Questions = new List<Question>();

            for (int i = 0; i < 1; i++)
            {
                var question = new Question();

                Console.WriteLine($"Enter question {i + 1}:");
                question.Text = Console.ReadLine();

                question.Options = new string[4];
                for (int j = 0; j < 4; j++)
                {
                    Console.WriteLine($"Enter option {j + 1} for question {i + 1}:");
                    question.Options[j] = Console.ReadLine();
                }

                while (true)
                {
                    Console.WriteLine($"Enter the correct option (1-4) for question {i + 1}:");
                    if (int.TryParse(Console.ReadLine(), out int correctOptionIndex))
                    {
                        question.CorrectOptionIndex = correctOptionIndex - 1;
                        if (question.CorrectOptionIndex >= 0 && question.CorrectOptionIndex <= 3)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid option. Please enter a number between 1 and 4.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect value. Please enter a number (1-4).");
                    }
                }

                quiz.Questions.Add(question);
            }

            quiz.AuthorId = authorId;
            CreateQuiz(quiz);
            Console.WriteLine("Quiz created successfully!");
        }

        public Quizz SelectQuiz(int authorId)
        {
            var quizzes = _quiz.Where(x => x.AuthorId == authorId).ToList();

            if (!quizzes.Any())
            {
                Console.WriteLine("You are not the owner of any quizzes.");
                return null;
            }

            Console.WriteLine("Your quizzes:");
            for (int i = 0; i < quizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {quizzes[i].Title}");
            }

            Console.WriteLine("Choose a quiz by number:");
            if (!int.TryParse(Console.ReadLine(), out int quizIndex) || quizIndex < 1 || quizIndex > quizzes.Count)
            {
                Console.WriteLine("Invalid choice.");
                return null;
            }

            return quizzes[quizIndex - 1];
        }


        public void EditQuiz(int authorId)
        {
            var selectedQuiz = SelectQuiz(authorId);
            if (selectedQuiz == null)
            {
                Console.WriteLine("Invalid Operation");
                return;
            }


            Console.WriteLine($"Editing Quiz: {selectedQuiz.Title}");
            Console.WriteLine("1) Edit Quiz Title");
            Console.WriteLine("2) Edit Questions");

            var choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.WriteLine("Enter new title:");
                selectedQuiz.Title = Console.ReadLine();
            }
            else if (choice == "2")
            {
                Console.WriteLine("Questions:");
                for (int i = 0; i < selectedQuiz.Questions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {selectedQuiz.Questions[i].Text}");
                }

                Console.WriteLine("Select a question to edit by entering its number:");
                if (!int.TryParse(Console.ReadLine(), out int questionIndex) || questionIndex < 1 || questionIndex > selectedQuiz.Questions.Count)
                {
                    Console.WriteLine("Invalid choice.");
                    return;
                }

                var selectedQuestion = selectedQuiz.Questions[questionIndex - 1];

                Console.WriteLine($"Editing Question: {selectedQuestion.Text}");
                Console.WriteLine("Enter new question text:");
                selectedQuestion.Text = Console.ReadLine();

                for (int i = 0; i < selectedQuestion.Options.Length; i++)
                {
                    Console.WriteLine($"Current Option {i + 1}: {selectedQuestion.Options[i]}");
                    Console.WriteLine($"Enter new Option {i + 1} (or press Enter to keep current):");
                    var newOption = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newOption))
                    {
                        selectedQuestion.Options[i] = newOption;
                    }
                }

                Console.WriteLine("Enter the correct option (1-4):");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out int correctOptionIndex) && correctOptionIndex >= 1 && correctOptionIndex <= 4)
                    {
                        selectedQuestion.CorrectOptionIndex = correctOptionIndex - 1;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            SaveData();
            Console.WriteLine("Quiz updated successfully!");
        }

        public void DeleteQuiz(int id)
        {
            var selectedQuiz = SelectQuiz(id);

            _quiz.Remove(selectedQuiz);
            SaveData();
            Console.WriteLine("Quiz deleted successfully!");

        }



        public void SaveData()
        {
            var json = JsonSerializer.Serialize(_quiz, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public List<Quizz> LoadQuizzes()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Quizz>();
            }

            var result = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Quizz>>(result);
        }
    }
}
       
