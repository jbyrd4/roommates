using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT rm.FirstName, rm.RentPortion, r.Name, r.MaxOccupancy FROM Roommate rm JOIN Room r on rm.RoomId = r.Id WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        if (reader.Read())
                        {
                            roommate = new Roommate()
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room()
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                                }

                            };
                        }
                        return roommate;
                    }
                }
            }
        }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT rm.Id, rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, rm.RoomId, r.Name FROM Roommate rm JOIN Room r on rm.RoomId = r.Id";
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommates = new List<Roommate>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                            string firstNameValue = reader.GetString(firstNameColumnPosition);

                            int lastNameColumnPosition = reader.GetOrdinal("LastName");
                            string lastNameValue = reader.GetString(lastNameColumnPosition);

                            int rentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                            int rentPortionValue = reader.GetInt32(rentPortionColumnPosition);

                            int moveInColumnPosition = reader.GetOrdinal("MoveInDate");
                            DateTime moveInValue = reader.GetDateTime(moveInColumnPosition);

                            int roomIdColumnPosition = reader.GetOrdinal("RoomId");
                            int roomIdValue = reader.GetInt32(roomIdColumnPosition);

                            int roomColumnPosition = reader.GetOrdinal("Name");
                            string roomValue = reader.GetString(roomColumnPosition);

                            Roommate roommate = new Roommate()
                            {
                                Id = idValue,
                                FirstName = firstNameValue,
                                LastName = lastNameValue,
                                RentPortion = rentPortionValue,
                                MovedInDate = moveInValue,
                                Room = new Room()
                                {
                                    Id = roomIdValue,
                                    Name = roomValue
                                }
                            };

                            roommates.Add(roommate);
                        }
                        return roommates;
                    }
                }
            }
        }
    }
}
