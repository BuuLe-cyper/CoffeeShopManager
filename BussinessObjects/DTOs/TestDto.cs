using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.DTOs
{
    public class TestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        // Constructor
        public TestDTO(int id, string name, DateTime date)
        {
            Id = id;
            Name = name;
            Date = date;
        }

        // Mã thối (Code Smell) bắt đầu từ đây
        public void SaveTest()
        {
            if (Id == 0)
            {
                Console.WriteLine("ID cannot be zero.");
            }
            else
            {
                if (string.IsNullOrEmpty(Name))
                {
                    Console.WriteLine("Name cannot be empty.");
                }
                else
                {
                    if (Date == default)
                    {
                        Console.WriteLine("Date is not valid.");
                    }
                    else
                    {
                        Console.WriteLine("Test saved successfully.");
                    }
                }
            }
        }

        // Mã thối (Code Smell) tiếp tục từ đây
        public void UpdateTest(string newName, DateTime newDate)
        {
            if (string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("New name cannot be empty.");
            }

            if (newDate == default)
            {
                Console.WriteLine("New date is not valid.");
            }
            else
            {
                Name = newName;
                Date = newDate;
            }
        }
    }

}
