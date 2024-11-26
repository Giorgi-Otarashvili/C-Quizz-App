using Quiz.Models;
using System.Text.Json;

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
        Console.WriteLine("Enter your question:");
        quiz.Question = Console.ReadLine();

        quiz.Options = new string[4];
        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine($"Enter option {i + 1}:");
            quiz.Options[i] = Console.ReadLine();
        }

        while (true)
        {
            Console.WriteLine("Enter the correct option (1-4):");
            if (int.TryParse(Console.ReadLine(), out int correctOptionIndex))
            {
                quiz.CorrectOpinionIndex = correctOptionIndex - 1;
                if (quiz.CorrectOpinionIndex >= 0 && quiz.CorrectOpinionIndex <= 3)
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

        quiz.AuthorId = authorId;
        quiz.Id = _quiz.Any() ? _quiz.Max(x => x.Id) + 1 : 1;
        CreateQuiz(quiz);
        Console.WriteLine("Quiz created successfully!");
    }

    private void SaveData()
    {
        var json = JsonSerializer.Serialize(_quiz, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    private List<Quizz> LoadQuizzes()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Quizz>();
        }

        var result = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Quizz>>(result);
    }
}


