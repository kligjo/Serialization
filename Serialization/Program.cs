using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Xml.Serialization;

namespace SerializationBasics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SERIALIZATION: CREATE AND READ FILES ===\n");

            // Generate sample files and read them back
            GenerateAndReadJsonFile();
            GenerateAndReadXmlFile();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void GenerateAndReadJsonFile()
        {
            Console.WriteLine("=== JSON FILE EXAMPLE ===");

            // Step 1: Create sample data
            List<Person> person = new List<Person>()
            {
                new Person
            {
                Name = "John Doe",
                Age = 30,
                Email = "john@example.com",
                Hobbies = new List<string> { "Reading", "Gaming", "Cooking" }
            },
                new Person
            {
                Name = "Klime Doe",
                Age = 30,
                Email = "klime@example.com",
                Hobbies = new List<string> { "Gaming", "Cooking" }
            }
            }; ;

            // Step 2: Generate JSON file
            string fileName = "person.json";
            string json = JsonSerializer.Serialize(person, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(fileName, json);
            Console.WriteLine($"✓ Generated file: {fileName}");
            Console.WriteLine("File contents:");
            Console.WriteLine(json);

            // Step 3: Read from the generated file
            Console.WriteLine("\n--- Reading from file ---");

            if (File.Exists(fileName))
            {
                string fileContent = File.ReadAllText(fileName);
                List<Person> loadedPersons = JsonSerializer.Deserialize<List<Person>>(fileContent);
                foreach (var loadedPerson in loadedPersons)
                {
                    Console.WriteLine($"Name: {loadedPerson.Name}");
                    Console.WriteLine($"Age: {loadedPerson.Age}");
                    Console.WriteLine($"Email: {loadedPerson.Email}");
                    Console.WriteLine($"Hobbies: {string.Join(", ", loadedPerson.Hobbies)}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("File not found!");
            }

            Console.WriteLine();
        }

        static void GenerateAndReadXmlFile()
        {
            Console.WriteLine("=== XML FILE EXAMPLE ===");

            // Step 1: Create sample data
            List<Book> book = new List<Book>()
            {
                new Book{
                Title = "C# Programming Guide",
                Author = "Jane Smith",
                Year = 2024,
                Price = 49.99m,
                Categories = new List<string> { "Programming", "Technology", "Education" }
            },
               new Book {
                Title = "C# Programming Guide",
                Author = "Jane Smith",
                    Year = 2024,
                Price = 49.99m,
                Categories = new List<string> { "Programming", "Technology", "Education" }
            }
            };

            // Step 2: Generate XML file
            string fileName = "book.xml";
            var xmlSerializer = new XmlSerializer(typeof(List<Book>));

            using (var writer = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(writer, book);
            }

            Console.WriteLine($"✓ Generated file: {fileName}");

            // Show file contents
            string xmlContent = File.ReadAllText(fileName);
            Console.WriteLine("File contents:");
            Console.WriteLine(xmlContent);

            // Step 3: Read from the generated file
            Console.WriteLine("\n--- Reading from file ---");

            if (File.Exists(fileName))
            {
                using (var reader = new StreamReader(fileName))
                {
                    List<Book> loadedBooks = (List<Book>)xmlSerializer.Deserialize(reader);
                    foreach (var loadedBook in loadedBooks)
                    {

                        Console.WriteLine($"Title: {loadedBook.Title}");
                        Console.WriteLine($"Author: {loadedBook.Author}");
                        Console.WriteLine($"Year: {loadedBook.Year}");
                        Console.WriteLine($"Price: ${loadedBook.Price}");
                        Console.WriteLine($"Categories: {string.Join(", ", loadedBook.Categories)}");
                    }
                }
            }
            else
            {
                Console.WriteLine("File not found!");
            }

            Console.WriteLine();
        }
    }

    // Simple classes for examples
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public List<string> Hobbies { get; set; } = new List<string>();
    }

    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public List<string> Categories { get; set; } = new List<string>();

    }

    // ===== BONUS: MULTIPLE RECORDS EXAMPLE =====

    public static class MultipleRecordsExample
    {
        public static void GenerateAndReadStudentsList()
        {
            Console.WriteLine("=== MULTIPLE STUDENTS EXAMPLE ===");

            // Step 1: Create list of students
            var students = new List<Student>
            {
                new Student { Name = "Alice", Grade = "A", Subject = "Math", Score = 95 },
                new Student { Name = "Bob", Grade = "B", Subject = "Science", Score = 87 },
                new Student { Name = "Charlie", Grade = "A", Subject = "History", Score = 92 }
            };

            // Step 2: Generate JSON file with multiple records
            string fileName = "students.json";
            string json = JsonSerializer.Serialize(students, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(fileName, json);
            Console.WriteLine($"✓ Generated file: {fileName} with {students.Count} students");

            // Step 3: Read and display all students
            Console.WriteLine("\n--- Reading all students from file ---");

            if (File.Exists(fileName))
            {
                string fileContent = File.ReadAllText(fileName);
                List<Student> loadedStudents = JsonSerializer.Deserialize<List<Student>>(fileContent);

                foreach (var student in loadedStudents)
                {
                    Console.WriteLine($"{student.Name}: {student.Grade} in {student.Subject} (Score: {student.Score})");
                }
            }

            Console.WriteLine();
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public string Grade { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
    }

    // ===== UTILITY CLASS FOR FILE OPERATIONS =====

    public static class FileSerializer
    {
        // Generate JSON file from any object
        public static void GenerateJsonFile<T>(T obj, string fileName)
        {
            try
            {
                string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(fileName, json);
                Console.WriteLine($"✓ JSON file generated: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating file: {ex.Message}");
            }
        }

        // Read JSON file and convert to object
        public static T ReadJsonFile<T>(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"File not found: {fileName}");
                    return default(T);
                }

                string json = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return default(T);
            }
        }

        // Generate XML file from any object
        public static void GenerateXmlFile<T>(T obj, string fileName)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var writer = new StreamWriter(fileName))
                {
                    xmlSerializer.Serialize(writer, obj);
                }
                Console.WriteLine($"✓ XML file generated: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating XML file: {ex.Message}");
            }
        }

        // Read XML file and convert to object
        public static T ReadXmlFile<T>(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"File not found: {fileName}");
                    return default(T);
                }

                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var reader = new StreamReader(fileName))
                {
                    return (T)xmlSerializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading XML file: {ex.Message}");
                return default(T);
            }
        }
    }

    // ===== EXAMPLE USAGE OF UTILITY CLASS =====

    public static class UtilityExample
    {
        public static void RunUtilityExample()
        {
            Console.WriteLine("=== USING UTILITY CLASS ===");

            // Create sample product
            var product = new Product
            {
                Id = 101,
                Name = "Laptop",
                Price = 999.99m,
                InStock = true
            };

            // Generate files using utility
            FileSerializer.GenerateJsonFile(product, "product.json");
            FileSerializer.GenerateXmlFile(product, "product.xml");

            // Read files using utility
            Product jsonProduct = FileSerializer.ReadJsonFile<Product>("product.json");
            Product xmlProduct = FileSerializer.ReadXmlFile<Product>("product.xml");

            if (jsonProduct != null)
                Console.WriteLine($"From JSON: {jsonProduct.Name} - ${jsonProduct.Price}");

            if (xmlProduct != null)
                Console.WriteLine($"From XML: {xmlProduct.Name} - ${xmlProduct.Price}");
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
    }
}