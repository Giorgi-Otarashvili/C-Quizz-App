using Quiz.Models;
using System.Diagnostics;
using System.Text.Json;

namespace QC_Quizz_App
{
    public class UserRepository
    {
        private readonly string _filePath;
        private readonly QuizzRepository _quizzRepository;
        private List<User> _users;
        private List<Quizz> _quiz;

        public UserRepository(string filepath, QuizzRepository quizzRepository = null)
        {
            _filePath = filepath;
            _users = LoadUsers();
            _quizzRepository = quizzRepository;
        }



        public void Register()
        {
            Console.WriteLine("---REGISTRATION---");
            Console.WriteLine("Enter Your Username");
            string username = Console.ReadLine();



            if (_users.Any(x => x.UserName.Trim().ToLower() == username.Trim().ToLower()))
            {
                Console.WriteLine("This Username already exist, try another");
                return;
            }
            Console.WriteLine("Enter Password");
            string password = Console.ReadLine();

            User newUser = new User
            {
                UserName = username,
                Password = password,
                HighScore = 0
                
            };
            

            CreateUser(newUser);

            Console.WriteLine($"Registration Succesful, Welcome to quizz Game {username} ");

        }

        public bool  Login()
        {
            Console.WriteLine("---Login---");
            Console.WriteLine("Enter your Username\n");
            string username = Console.ReadLine();
            Console.WriteLine("Enter your Password\n");
            string password = Console.ReadLine();

            var user = _users.FirstOrDefault(x => x.UserName.Trim().ToLower() == username.Trim().ToLower() && x.Password == password);

            if(user == null)
            {
                Console.WriteLine("Username or Password is incorrect\nPlease try again.");
                return false;
            }
            else
            {
                Console.WriteLine($"Welcome back, {username}!");
                return true;
            }
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
                switch(afterLoginChoice){
                    case "1":
                        Console.WriteLine("play func");
                             break;
                        case "2":
                        _quizzRepository.CreateQuiz(user.Id);
                        break;
                        case "3":
                        Console.WriteLine("logout func");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break ;


                    }
                }

        }



        public void CreateUser(User user)
        {
            user.Id = _users.Any() ? _users.Max(x => x.Id) + 1 : 1;
            _users.Add(user);
            SaveData();
        }

       

        public void SaveData()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true});
            File.WriteAllText(_filePath, json);
        }

        public  List<User> LoadUsers()
        {
            if(!File.Exists(_filePath))
            {
                return new List<User>();
            }

            string result = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(result);

        }
    }
}
