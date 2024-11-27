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
                Console.WriteLine("2. Create Quiz");
                Console.WriteLine("3. Logout");
                Console.Write("Select an option: ");

                string afterLoginChoice = Console.ReadLine();
                switch (afterLoginChoice)
                {
                    case "1":
                        PlayGame(user);
                        break;
                    case "2":
                        _quizzRepository.CreateQuiz(user.Id);
                        break;
                    case "3":
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

            if (_userRepository == null)
            {
                Console.WriteLine("Error: UserRepository is not initialized.");
                return;
            }

            Console.WriteLine("Starting the quiz... You have 2 minutes to answer 5 questions!");

            var questions = _quizzRepository.GetRandomQuestions(5, user.Id);
            int score = 0;
            var timer = DateTime.Now.AddMinutes(2);

            foreach (var question in questions)
            {
                if (DateTime.Now > timer)
                {
                    Console.WriteLine("Time's up!");
                    break;
                }
                Console.WriteLine($"\nQuestion: {question.Question}");
                for (int i = 0; i < question.Options.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Options[i]}");
                }
                Console.Write("Your Answer: ");
                if (int.TryParse(Console.ReadLine(), out int answer) &&
                        answer - 1 == question.CorrectOpinionIndex)
                {
                    Console.WriteLine("Correct!");
                    score += 20;
                }
                else
                {
                    Console.WriteLine("Incorrect.");
                    score -= 20;
                }

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
            Console.WriteLine($"\nGame over! Your final score is: {score}");
        }


    }
}
