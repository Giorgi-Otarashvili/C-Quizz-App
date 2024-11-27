using QC_Quizz_App;
using System.Linq.Expressions;

namespace C_Quizz_App
{
    public class Program
    {
        static void Main(string[] args)
        {

            var userRepository = new UserRepository("D:\\C-Quizz-App\\Quizz.Repository\\Data\\Users.json");
            var quizzRepository = new QuizzRepository("D:\\C-Quizz-App\\Quizz.Repository\\Data\\Quiz.json");

            // Step 2: Pass repositories to the Game class
            var game = new Game(quizzRepository, userRepository);


            while (true)
            {
                Console.WriteLine("Welcome to quizz App");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3.Exit");
                Console.Write("Select An Option\n");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        userRepository.Register();
                        return;

                    case "2":
                        var isLogin = userRepository.Login();
                        if (isLogin != null)
                        {
                            game.AfterLogin(isLogin);
                        }
                        break;
                    case "3":
                        Console.WriteLine("GoodBye");
                        return;
                    default:
                        Console.WriteLine("Invalid Option. Please Try Again");
                        break;

                }

            }


        }

    }
}
