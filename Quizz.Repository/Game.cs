using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QC_Quizz_App 
{
    public class Game
    {
        private readonly QuizzRepository _quizzRepository;
        private readonly UserRepository _userRepository;



        public Game(QuizzRepository quizzRepository, UserRepository userRepository)
        {
            _quizzRepository = quizzRepository;
            _userRepository = userRepository;
        }



        public void AfterLogin(User user)
        {
            while (true)
            {
                Console.WriteLine("\n--- Post Login Menu ---");
                Console.WriteLine("1. Play Quiz");
                Console.WriteLine("2. Haiscorres");
                Console.WriteLine("3. Create Quiz");
                Console.WriteLine("4. Edit your qvizz");
                Console.WriteLine("5. Delete your qvizz");
                Console.WriteLine("6. Logout");
                Console.Write("Select an option: ");

                string afterLoginChoice = Console.ReadLine();
                switch (afterLoginChoice)
                {
                    case "1":
                        PlayGame(user);
                        break;
                        case "2":
                        _userRepository.GetTop10Haiscores();
                        break;
                    case "3":
                        _quizzRepository.CreateQuiz(user.Id);
                        break;
                        case "4":
                           _quizzRepository.EditQuiz(user.Id);
                        break;
                        case "5":
                            _quizzRepository.DeleteQuiz(user.Id);
                        break;
                    case "6":
                        Console.WriteLine("logout func");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;


                }
            }

        }
        public void PlayGame(User user)
        {
            if (user == null)
            {
                Console.WriteLine("Error: user is null. Exiting game.");
                return;
            }

            var quizzes = _quizzRepository.LoadQuizzes();
            if (!quizzes.Any())
            {
                Console.WriteLine("No quizzes available for you to play.");
                return;
            }

            Console.WriteLine("Select a quiz to play:");
            for (int i = 0; i < quizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {quizzes[i].Title}");
            }

            Console.Write("Enter the number of your choice: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > quizzes.Count)
            {
                Console.WriteLine("Invalid selection. Exiting game.");
                return;
            }

            var selectedQuiz = quizzes[choice - 1];
            Console.WriteLine($"\nQuiz Title: {selectedQuiz.Title}");
            int score = 0;

            Console.WriteLine("Starting the quiz... You have 2 minutes to answer 5 questions!");

            var timer = DateTime.Now.AddMinutes(2);

            foreach (var question in selectedQuiz.Questions)
            {
                if (DateTime.Now > timer)
                {
                    Console.WriteLine("Time's up!");
                    break;
                }

                Console.WriteLine($"\nQuestion: {question.Text}");
                for (int i = 0; i < question.Options.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Options[i]}");
                }

                Console.Write("Your Answer: ");
                if (int.TryParse(Console.ReadLine(), out int answer) &&
                    answer - 1 == question.CorrectOptionIndex)
                {
                    Console.WriteLine("Correct!");
                    score += 20;
                }
                else
                {
                    Console.WriteLine("Incorrect.");
                    score -= 20;
                }
            }

            Console.WriteLine($"\nQuiz Over! Your final score is: {score}");

            if (score > user.HighScore)
            {
                Console.WriteLine("New high score!");
                user.HighScore = score;

                try
                {
                    _userRepository.SaveData();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving data: {ex.Message}");
                }
            }
        }
    }
}
