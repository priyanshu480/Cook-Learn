using System.Collections.Generic;

namespace dotnetapp.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public decimal ConsultationFee { get; set; }
        public ICollection<Patient> Patients { get; set; }

        public Doctor()
        {
            Name = string.Empty;
            Specialization = string.Empty;
            Patients = new List<Patient>();
        }

        public Doctor(int doctorId, string name, string specialization, decimal consultationFee)
        {
            DoctorId = doctorId;
            Name = name;
            Specialization = specialization;
            ConsultationFee = consultationFee;
            Patients = new List<Patient>();
        }
    }
}




using System;

namespace dotnetapp.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Condition { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }

        public Patient()
        {
            Name = string.Empty;
            Condition = string.Empty;
        }

        public Patient(int patientId, string name, int age, string condition, DateTime appointmentDate, int doctorId)
        {
            PatientId = patientId;
            Name = name;
            Age = age;
            Condition = condition;
            AppointmentDate = appointmentDate;
            DoctorId = doctorId;
        }
    }
}


namespace dotnetapp.Managers
{
    public interface IDoctorManager
    {
        void AddDoctor();
        void ListDoctors();
        void AddDoctorToDB();
        void ListDoctorsFromDB();
    }
}



namespace dotnetapp.Managers
{
    public interface IPatientManager
    {
        void AddPatient(int doctorId);
        void ListPatients();
        void FindPatient(string patientName);
        void EditPatient();
        void DeletePatient();

        void AddPatientToDB(int doctorId);
        void EditPatientInDB();
        void DeletePatientFromDB();
        void ListPatientsFromDB();
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using dotnetapp.Models;

namespace dotnetapp.Managers
{
    public class DoctorManager : IDoctorManager
    {
        public static List<Doctor> Doctors = new List<Doctor>();
        public static List<Doctor> DoctorsDB = new List<Doctor>();

        private static int doctorIdCounter = 1;
        private static int doctorDbIdCounter = 1;

        public void AddDoctor()
        {
            Console.Write("Enter Doctor Name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter Specialization: ");
            string specialization = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter ConsultationFee: ");
            decimal fee = ReadDecimal();

            Doctor doctor = new Doctor
            {
                DoctorId = doctorIdCounter++,
                Name = name,
                Specialization = specialization,
                ConsultationFee = fee,
                Patients = new List<Patient>()
            };

            Doctors.Add(doctor);

            Console.WriteLine("Doctor added successfully!");
        }

        public void ListDoctors()
        {
            if (Doctors.Count == 0)
            {
                Console.WriteLine("No Doctors");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("List of Doctors:");

            foreach (Doctor doctor in Doctors)
            {
                Console.WriteLine($"Doctor ID: {doctor.DoctorId}, Name: {doctor.Name}, Specialization: {doctor.Specialization}, ConsultationFee: ₹{doctor.ConsultationFee:0.##}");

                Console.WriteLine("Patients:");

                var patients = PatientManager.Patients
                    .Where(p => p.DoctorId == doctor.DoctorId)
                    .ToList();

                if (patients.Count == 0)
                {
                    Console.WriteLine("No patients available for this doctor.");
                }
                else
                {
                    foreach (Patient patient in patients)
                    {
                        Console.WriteLine($"    Patient ID: {patient.PatientId}, Name: {patient.Name}, Age: {patient.Age}, Condition: {patient.Condition}, AppointmentDate: {patient.AppointmentDate}");
                    }
                }
            }
        }

        public void AddDoctorToDB()
        {
            Console.Write("Enter Doctor Name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter Specialization: ");
            string specialization = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter ConsultationFee: ");
            decimal fee = ReadDecimal();

            Doctor doctor = new Doctor
            {
                DoctorId = doctorDbIdCounter++,
                Name = name,
                Specialization = specialization,
                ConsultationFee = fee,
                Patients = new List<Patient>()
            };

            DoctorsDB.Add(doctor);

            Console.WriteLine("Doctor added to the database successfully!");
        }

        public void ListDoctorsFromDB()
        {
            if (DoctorsDB.Count == 0)
            {
                Console.WriteLine("No Doctors available in database.");
                return;
            }

            foreach (Doctor doctor in DoctorsDB)
            {
                Console.WriteLine($"Doctor ID: {doctor.DoctorId}");
                Console.WriteLine($"Name: {doctor.Name}");
                Console.WriteLine($"Specialization: {doctor.Specialization}");
                Console.WriteLine($"ConsultationFee: ₹{doctor.ConsultationFee:0.00}");
                Console.WriteLine("Patients:");

                var patients = PatientManager.PatientsDB
                    .Where(p => p.DoctorId == doctor.DoctorId)
                    .ToList();

                if (patients.Count == 0)
                {
                    Console.WriteLine("  No patients available.");
                }
                else
                {
                    foreach (Patient patient in patients)
                    {
                        Console.WriteLine($"    Patient ID: {patient.PatientId}, Name: {patient.Name}, Age: {patient.Age}, Condition: {patient.Condition}, AppointmentDate: {patient.AppointmentDate}");
                    }
                }
            }
        }

        private decimal ReadDecimal()
        {
            decimal value;
            while (!decimal.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid amount. Enter again: ");
            }
            return value;
        }
    }
}




using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using dotnetapp.Models;

namespace dotnetapp.Managers
{
    public class PatientManager : IPatientManager
    {
        public static List<Patient> Patients = new List<Patient>();
        public static List<Patient> PatientsDB = new List<Patient>();

        private static int patientIdCounter = 1;
        private static int patientDbIdCounter = 1;

        public void AddPatient(int doctorId)
        {
            Console.Write("Enter Patient Name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter Patient Age: ");
            int age = ReadInt();

            Console.Write("Enter Patient Condition: ");
            string condition = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter Appointment Date: ");
            DateTime appointmentDate = ReadDate();

            Patient patient = new Patient
            {
                PatientId = patientIdCounter++,
                Name = name,
                Age = age,
                Condition = condition,
                AppointmentDate = appointmentDate,
                DoctorId = doctorId
            };

            Patients.Add(patient);

            Console.WriteLine("Patient added successfully!");
        }

        public void ListPatients()
        {
            if (Patients.Count == 0)
            {
                Console.WriteLine("No Patients");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("List of Patients:");

            foreach (Patient patient in Patients)
            {
                string doctorName = GetDoctorName(patient.DoctorId, false);

                Console.WriteLine($"Patient ID: {patient.PatientId}, Name: {patient.Name}, Age: {patient.Age}, Condition: {patient.Condition}, Appointment Date: {patient.AppointmentDate}, Doctor Name: {doctorName}");
            }
        }

        public void FindPatient(string patientName)
        {
            Patient? patient = Patients.FirstOrDefault(p =>
                p.Name.Equals(patientName, StringComparison.OrdinalIgnoreCase));

            if (patient == null)
            {
                Console.WriteLine($"Patient with name '{patientName}' not found.");
                return;
            }

            string doctorName = GetDoctorName(patient.DoctorId, false);

            Console.WriteLine($"Patient ID: {patient.PatientId}, Name: {patient.Name}, Age: {patient.Age}, Condition: {patient.Condition}, Appointment Date: {patient.AppointmentDate}, Doctor Name: {doctorName}");
        }

        public void EditPatient()
        {
            Console.Write("Enter Patient ID to edit: ");
            int id = ReadInt();

            Patient? patient = Patients.FirstOrDefault(p => p.PatientId == id);

            if (patient == null)
            {
                Console.WriteLine($"Patient with ID {id} not found.");
                return;
            }

            Console.Write("Enter new Patient Name (leave empty to keep current): ");
            string name = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(name))
            {
                patient.Name = name;
            }

            Console.Write("Enter new Patient Age (leave empty to keep current): ");
            string ageInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(ageInput) && int.TryParse(ageInput, out int age))
            {
                patient.Age = age;
            }

            Console.Write("Enter new Patient Condition (leave empty to keep current): ");
            string condition = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(condition))
            {
                patient.Condition = condition;
            }

            Console.Write("Enter new Patient AppointmentDate (leave empty to keep current): ");
            string dateInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(dateInput) && DateTime.TryParse(dateInput, out DateTime date))
            {
                patient.AppointmentDate = date;
            }

            Console.WriteLine("Patient information updated successfully!");
        }

        public void DeletePatient()
        {
            Console.Write("Enter Patient ID to delete: ");
            int id = ReadInt();

            Patient? patient = Patients.FirstOrDefault(p => p.PatientId == id);

            if (patient == null)
            {
                Console.WriteLine($"Patient with ID {id} not found.");
                return;
            }

            Patients.Remove(patient);

            Console.WriteLine("Patient deleted successfully!");
        }

        public void AddPatientToDB(int doctorId)
        {
            Console.Write("Enter Patient Name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter Age Name: ");
            int age = ReadInt();

            Console.Write("Enter Patient Condition: ");
            string condition = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter Appointment Date: ");
            DateTime appointmentDate = ReadDate();

            Patient patient = new Patient
            {
                PatientId = patientDbIdCounter++,
                Name = name,
                Age = age,
                Condition = condition,
                AppointmentDate = appointmentDate,
                DoctorId = doctorId
            };

            PatientsDB.Add(patient);

            Console.WriteLine("Patient added to the database successfully!");
        }

        public void ListPatientsFromDB()
        {
            if (PatientsDB.Count == 0)
            {
                Console.WriteLine("No Patients");
                return;
            }

            foreach (Patient patient in PatientsDB)
            {
                string doctorName = GetDoctorName(patient.DoctorId, true);

                Console.WriteLine($"Patient ID: {patient.PatientId}");
                Console.WriteLine($"Name: {patient.Name}");
                Console.WriteLine($"Age: {patient.Age}");
                Console.WriteLine($"Condition: {patient.Condition}");
                Console.WriteLine($"AppointmentDate: {patient.AppointmentDate}");
                Console.WriteLine($"Doctor Name: {doctorName}");
            }
        }

        public void EditPatientInDB()
        {
            Console.Write("Enter Patient ID to edit: ");
            int id = ReadInt();

            Patient? patient = PatientsDB.FirstOrDefault(p => p.PatientId == id);

            if (patient == null)
            {
                Console.WriteLine($"Patient with ID {id} not found.");
                return;
            }

            Console.WriteLine($"Editing Patient with ID {id}:");

            Console.Write("Enter new Patient Name (leave empty to keep current): ");
            string name = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(name))
            {
                patient.Name = name;
            }

            Console.Write("Enter new Patient Age (leave empty to keep current): ");
            string ageInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(ageInput) && int.TryParse(ageInput, out int age))
            {
                patient.Age = age;
            }

            Console.Write("Enter new Patient Condition (leave empty to keep current): ");
            string condition = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(condition))
            {
                patient.Condition = condition;
            }

            Console.Write("Enter new Patient AppointmentDate (leave empty to keep current): ");
            string dateInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(dateInput) && DateTime.TryParse(dateInput, out DateTime date))
            {
                patient.AppointmentDate = date;
            }

            Console.WriteLine("Patient information updated successfully!");
        }

        public void DeletePatientFromDB()
        {
            Console.Write("Enter Patient ID to delete: ");
            int id = ReadInt();

            Patient? patient = PatientsDB.FirstOrDefault(p => p.PatientId == id);

            if (patient == null)
            {
                Console.WriteLine($"Patient with ID {id} not found.");
                return;
            }

            PatientsDB.Remove(patient);

            Console.WriteLine("Patient deleted successfully!");
        }

        private string GetDoctorName(int doctorId, bool db)
        {
            if (db)
            {
                Doctor? doctor = DoctorManager.DoctorsDB.FirstOrDefault(d => d.DoctorId == doctorId);
                return doctor != null ? doctor.Name : "Unknown";
            }
            else
            {
                Doctor? doctor = DoctorManager.Doctors.FirstOrDefault(d => d.DoctorId == doctorId);
                return doctor != null ? doctor.Name : "Unknown";
            }
        }

        private int ReadInt()
        {
            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid number. Enter again: ");
            }
            return value;
        }

        private DateTime ReadDate()
        {
            string input = Console.ReadLine() ?? string.Empty;

            DateTime date;

            string[] formats =
            {
                "dd/MM/yyyy",
                "d/M/yyyy",
                "MM/dd/yyyy",
                "M/d/yyyy",
                "yyyy-MM-dd"
            };

            if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }

            if (DateTime.TryParse(input, out date))
            {
                return date;
            }

            return DateTime.Now;
        }
    }
}



using dotnetapp.Managers;

namespace dotnetapp.BLL
{
    public class DoctorsBLL
    {
        private readonly IDoctorManager doctorManager;

        public DoctorsBLL(IDoctorManager doctorManager)
        {
            this.doctorManager = doctorManager;
        }

        public void AddDoctor()
        {
            doctorManager.AddDoctor();
        }

        public void ListDoctors()
        {
            doctorManager.ListDoctors();
        }

        public void AddDoctorToDB()
        {
            doctorManager.AddDoctorToDB();
        }

        public void ListDoctorsFromDB()
        {
            doctorManager.ListDoctorsFromDB();
        }
    }
}



using dotnetapp.Managers;

namespace dotnetapp.BLL
{
    public class PatientsBLL
    {
        private readonly IPatientManager patientManager;

        public PatientsBLL(IPatientManager patientManager)
        {
            this.patientManager = patientManager;
        }

        public void AddPatient(int doctorId)
        {
            patientManager.AddPatient(doctorId);
        }

        public void ListPatients()
        {
            patientManager.ListPatients();
        }

        public void FindPatient(string patientName)
        {
            patientManager.FindPatient(patientName);
        }

        public void EditPatient()
        {
            patientManager.EditPatient();
        }

        public void DeletePatient()
        {
            patientManager.DeletePatient();
        }

        public void AddPatientToDB(int doctorId)
        {
            patientManager.AddPatientToDB(doctorId);
        }

        public void ListPatientsFromDB()
        {
            patientManager.ListPatientsFromDB();
        }

        public void EditPatientInDB()
        {
            patientManager.EditPatientInDB();
        }

        public void DeletePatientFromDB()
        {
            patientManager.DeletePatientFromDB();
        }
    }
}




using System;
using dotnetapp.BLL;
using dotnetapp.Managers;

namespace dotnetapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IDoctorManager doctorManager = new DoctorManager();
            IPatientManager patientManager = new PatientManager();

            DoctorsBLL doctorsBLL = new DoctorsBLL(doctorManager);
            PatientsBLL patientsBLL = new PatientsBLL(patientManager);

            bool running = true;

            while (running)
            {
                DisplayMenu();

                Console.Write("Enter your choice: ");
                string input = Console.ReadLine() ?? string.Empty;

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid choice");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        doctorsBLL.AddDoctor();
                        break;

                    case 2:
                        doctorsBLL.ListDoctors();
                        break;

                    case 3:
                        Console.Write("Enter Doctor ID for the Patient: ");
                        int doctorId = ReadInt();
                        patientsBLL.AddPatient(doctorId);
                        break;

                    case 4:
                        patientsBLL.ListPatients();
                        break;

                    case 5:
                        Console.Write("Enter Patient Name to find: ");
                        string patientName = Console.ReadLine() ?? string.Empty;
                        patientsBLL.FindPatient(patientName);
                        break;

                    case 6:
                        patientsBLL.EditPatient();
                        break;

                    case 7:
                        patientsBLL.DeletePatient();
                        break;

                    case 8:
                        doctorsBLL.AddDoctorToDB();
                        break;

                    case 9:
                        doctorsBLL.ListDoctorsFromDB();
                        break;

                    case 10:
                        Console.Write("Enter Doctor ID for the Patient: ");
                        int dbDoctorId = ReadInt();
                        patientsBLL.AddPatientToDB(dbDoctorId);
                        break;

                    case 11:
                        patientsBLL.ListPatientsFromDB();
                        break;

                    case 12:
                        patientsBLL.EditPatientInDB();
                        break;

                    case 13:
                        patientsBLL.DeletePatientFromDB();
                        break;

                    case 14:
                        Console.WriteLine("Exiting from Hospital Management Console App...");
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }

                if (running)
                {
                    Console.WriteLine();
                }
            }
        }

        public static void DisplayMenu()
        {
            Console.WriteLine("Hospital Management Console App Menu:");
            Console.WriteLine("1. Add Doctor to list");
            Console.WriteLine("2. List Doctors from list");
            Console.WriteLine("3. Add Patient to list");
            Console.WriteLine("4. List Patients from list");
            Console.WriteLine("5. Find Patient from list");
            Console.WriteLine("6. Edit Patient in list");
            Console.WriteLine("7. Delete Patient from list");
            Console.WriteLine("8. Add Doctor to DB");
            Console.WriteLine("9. List Doctors from DB");
            Console.WriteLine("10. Add Patient to DB");
            Console.WriteLine("11. List Patients from DB");
            Console.WriteLine("12. Edit Patient in DB");
            Console.WriteLine("13. Delete Patient from DB");
            Console.WriteLine("14. Exit");
        }

        private static int ReadInt()
        {
            int value;

            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid number. Enter again: ");
            }

            return value;
        }
    }
}







