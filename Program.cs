// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

Console.WriteLine("Hello, World!");
using System;
using System.ComponentModel.DataAnnotations;

namespace StudentDatabaseCodeFirst
{
    public class Student
    {
        [Key]  // Primary Key
        public int Id { get; set; }

        [Required]  // Makes this field mandatory
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
using System;
using System.Data.Entity;

namespace StudentDatabaseCodeFirst
{
    public class StudentContext : DbContext
    {
        public StudentContext() : base("StudentDB")
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
using System;
using System.Linq;

namespace StudentDatabaseCodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new StudentContext())
            {
                // Create a new student
                var student = new Student
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Age = 22
                };

                // Add to database
                db.Students.Add(student);
                db.SaveChanges();

                // Retrieve and display students
                var students = db.Students.ToList();
                Console.WriteLine("Students in database:");
                foreach (var s in students)
                {
                    Console.WriteLine($"ID: {s.Id}, Name: {s.FirstName} {s.LastName}, Age: {s.Age}");
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
