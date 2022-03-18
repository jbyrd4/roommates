using System;
using System.Collections.Generic;
using Roommates.Models;
using Roommates.Repositories;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);


            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a room"):
                        List<Room> roomDelete = roomRepo.GetAll();
                        foreach (Room r in roomDelete)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} - Max Occupancy({r.MaxOccupancy}");
                        }
                        Console.WriteLine("Which room would you like to delete?");
                        int selectedDeleteRoomId = int.Parse(Console.ReadLine());
                        roomRepo.Delete(selectedDeleteRoomId);
                        Console.WriteLine("You successfully delete the room");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.WriteLine("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());
                        Chore chore = choreRepo.GetById(choreId);
                        Console.WriteLine($"Id: {chore.Id}, Name: {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.WriteLine("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };
                        choreRepo.Insert(choreToAdd);
                        break;
                    case ("Search for roommate"):
                        Console.WriteLine("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());
                        Roommate roommate = roommateRepo.GetById(roommateId);
                        Console.WriteLine($"Name: {roommate.FirstName}, Rent Portion: {roommate.RentPortion}, Room: {roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("View unnassigned chores"):
                        List<Chore> unnassignedChores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in unnassignedChores)
                        {
                            Console.WriteLine($"{c.Name} is currently unnassigned.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign chore to roommate"):
                        List<Chore> allChores = choreRepo.GetAll();
                        List<Roommate> allRoomates = roommateRepo.GetAll();

                        Console.WriteLine("Select the chore you want to assign");

                        foreach (Chore c in allChores)
                        {
                            Console.WriteLine($"{c.Id}: {c.Name}");
                        }

                        int selectedChoreId = int.Parse(Console.ReadLine());

                        Console.WriteLine("Select roommate to assign chore");

                        foreach (Roommate r in allRoomates)
                        {
                            Console.WriteLine($"{r.Id}: {r.FirstName} {r.LastName}");
                        }

                        int selectedRoomateId = int.Parse(Console.ReadLine());
                        choreRepo.AssignChore(selectedRoomateId, selectedChoreId);
                        Console.WriteLine("You did it!");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a chore"):
                        List<Chore> choreList = choreRepo.GetAll();
                        foreach (Chore c in choreList)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.WriteLine("Which chore would you like to update?");
                        int updateChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreList.FirstOrDefault(c => c.Id == updateChoreId);
                        Console.WriteLine("New chore name?");
                        selectedChore.Name = Console.ReadLine();
                        choreRepo.Update(selectedChore);
                        Console.WriteLine("You successfully updated the chore");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a chore"):
                        List<Chore> choreDeleteList = choreRepo.GetAll();
                        foreach (Chore c in choreDeleteList)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.WriteLine("Select the chore you would like to delete");
                        int deleteChoreId = int.Parse(Console.ReadLine());
                        choreRepo.Delete(deleteChoreId);
                        Console.WriteLine("You successfully deleted the chore");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Update a room",
                "Delete a room",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Search for roommate",
                "View unnassigned chores",
                "Assign chore to roommate",
                "Update a chore",
                "Delete a chore",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}