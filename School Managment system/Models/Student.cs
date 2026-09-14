namespace School_Managment_system.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string? profile { get; set; } = "";

        public string fullName { get; set; } = "";
        public int OverallRate { get; set; } 

        public string fatherName { get; set; } = "";

        public string email { get; set; } = "";

        public int phone { get; set; }

        public int Age { get; set; }

        public string gender { get; set; } = "";

        public string Class { get; set; } = "";

        public string admissionType { get; set; } = "";

        public string comments { get; set; } = "";

        public string password { get; set; } = "";

        public string feesstatus { get; set; } = "";
    }
}
