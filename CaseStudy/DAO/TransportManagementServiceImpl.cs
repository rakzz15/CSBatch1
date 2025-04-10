using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportManagementSystem.Entity;
using TransportManagementSystem.Util;
using TransportManagementSystem.Exceptions;

namespace TransportManagementSystem.DAO
{
    public class TransportManagementServiceImpl : ITransportService
    {
        public bool AddVehicle(Vehicle vehicle)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "INSERT INTO Vehicles (Model, Capacity, Type, Status) VALUES (@Model, @Capacity, @Type, @Status)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Model", vehicle.Model);
                    cmd.Parameters.AddWithValue("@Capacity", vehicle.Capacity);
                    cmd.Parameters.AddWithValue("@Type", vehicle.Type);
                    cmd.Parameters.AddWithValue("@Status", vehicle.Status);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding vehicle: " + ex.Message);
                return false;
            }
        }
        public bool UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "UPDATE Vehicles SET Model=@Model, Capacity=@Capacity, Type=@Type, Status=@Status WHERE VehicleID=@VehicleID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Model", vehicle.Model);
                    cmd.Parameters.AddWithValue("@Capacity", vehicle.Capacity);
                    cmd.Parameters.AddWithValue("@Type", vehicle.Type);
                    cmd.Parameters.AddWithValue("@Status", vehicle.Status);
                    cmd.Parameters.AddWithValue("@VehicleID", vehicle.VehicleID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating vehicle: " + ex.Message);
                return false;
            }
        }
        public bool DeleteVehicle(int vehicleId)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "DELETE FROM Vehicles WHERE VehicleID=@VehicleID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@VehicleID", vehicleId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting vehicle: " + ex.Message);
                return false;
            }
        }
        public bool ScheduleTrip(int vehicleId, int routeId, DateTime departureDate, DateTime arrivalDate, int maxPassengers)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "INSERT INTO Trips (VehicleID, RouteID, DepartureDate, ArrivalDate, Status, TripType, MaxPassengers) VALUES (@VehicleID, @RouteID, @DepartureDate, @ArrivalDate, 'Scheduled', 'Freight', @MaxPassengers)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@VehicleID", vehicleId);
                    cmd.Parameters.AddWithValue("@RouteID", routeId);
                    cmd.Parameters.AddWithValue("@DepartureDate", departureDate);
                    cmd.Parameters.AddWithValue("@ArrivalDate", arrivalDate);
                    cmd.Parameters.AddWithValue("@MaxPassengers", maxPassengers);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error scheduling trip: " + ex.Message);
                return false;
            }
        }
        public bool CancelTrip(int tripId)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "UPDATE Trips SET Status='Cancelled' WHERE TripID=@TripID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TripID", tripId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error cancelling trip: " + ex.Message);
                return false;
            }
        }
        public bool BookTrip(int tripId, int passengerId, DateTime bookingDate)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "INSERT INTO Bookings (TripID, PassengerID, BookingDate, Status) VALUES (@TripID, @PassengerID, @BookingDate, 'Confirmed')";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TripID", tripId);
                    cmd.Parameters.AddWithValue("@PassengerID", passengerId);
                    cmd.Parameters.AddWithValue("@BookingDate", bookingDate);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error booking trip: " + ex.Message);
                return false;
            }
        }

        public bool CancelBooking(int bookingId)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "UPDATE Bookings SET Status='Cancelled' WHERE BookingID=@BookingID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error cancelling booking: " + ex.Message);
                return false;
            }
        }

        public bool AllocateDriver(int tripId, int driverId)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string updateTrip = "UPDATE Trips SET DriverID=@DriverID WHERE TripID=@TripID";
                    SqlCommand cmd = new SqlCommand(updateTrip, conn);
                    cmd.Parameters.AddWithValue("@DriverID", driverId);
                    cmd.Parameters.AddWithValue("@TripID", tripId);
                    int result = cmd.ExecuteNonQuery();

                    string updateDriver = "UPDATE Drivers SET Status='On Trip' WHERE DriverID=@DriverID";
                    SqlCommand cmd2 = new SqlCommand(updateDriver, conn);
                    cmd2.Parameters.AddWithValue("@DriverID", driverId);
                    cmd2.ExecuteNonQuery();

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error allocating driver: " + ex.Message);
                return false;
            }
        }

        public bool DeallocateDriver(int tripId)
        {
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string selectDriver = "SELECT DriverID FROM Trips WHERE TripID=@TripID";
                    SqlCommand selectCmd = new SqlCommand(selectDriver, conn);
                    selectCmd.Parameters.AddWithValue("@TripID", tripId);
                    object driverIdObj = selectCmd.ExecuteScalar();

                    if (driverIdObj != null && driverIdObj != DBNull.Value)
                    {
                        int driverId = Convert.ToInt32(driverIdObj);

                        string updateTrip = "UPDATE Trips SET DriverID=NULL WHERE TripID=@TripID";
                        SqlCommand updateCmd = new SqlCommand(updateTrip, conn);
                        updateCmd.Parameters.AddWithValue("@TripID", tripId);
                        updateCmd.ExecuteNonQuery();

                        string updateDriver = "UPDATE Drivers SET Status='Available' WHERE DriverID=@DriverID";
                        SqlCommand updateDriverCmd = new SqlCommand(updateDriver, conn);
                        updateDriverCmd.Parameters.AddWithValue("@DriverID", driverId);
                        updateDriverCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deallocating driver: " + ex.Message);
                return false;
            }
        }

        public List<Booking> GetBookingsByPassenger(int passengerId)
        {
            List<Booking> bookings = new List<Booking>();
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "SELECT * FROM Bookings WHERE PassengerID=@PassengerID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PassengerID", passengerId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        bookings.Add(new Booking
                        {
                            BookingID = (int)reader["BookingID"],
                            TripID = (int)reader["TripID"],
                            PassengerID = (int)reader["PassengerID"],
                            BookingDate = (DateTime)reader["BookingDate"],
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving bookings: " + ex.Message);
            }
            return bookings;
        }

        public List<Booking> GetBookingsByTrip(int tripId)
        {
            List<Booking> bookings = new List<Booking>();
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "SELECT * FROM Bookings WHERE TripID=@TripID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TripID", tripId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        bookings.Add(new Booking
                        {
                            BookingID = (int)reader["BookingID"],
                            TripID = (int)reader["TripID"],
                            PassengerID = (int)reader["PassengerID"],
                            BookingDate = (DateTime)reader["BookingDate"],
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving trip bookings: " + ex.Message);
            }
            return bookings;
        }

        public List<Driver> GetAvailableDrivers()
        {
            List<Driver> drivers = new List<Driver>();
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "SELECT * FROM Drivers WHERE Status='Available'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        drivers.Add(new Driver
                        {
                            DriverID = (int)reader["DriverID"],
                            Name = reader["Name"].ToString(),
                            LicenseNumber = reader["LicenseNumber"].ToString(),
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving available drivers: " + ex.Message);
            }
            return drivers;
        }

        public List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "SELECT * FROM Vehicles";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        vehicles.Add(new Vehicle
                        {
                            VehicleID = (int)reader["VehicleID"],
                            Model = reader["Model"].ToString(),
                            Capacity = Convert.ToDouble(reader["Capacity"]),
                            Type = reader["Type"].ToString(),
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving vehicles: " + ex.Message);
            }
            return vehicles;
        }

        public List<Trip> GetAllTrips()
        {
            List<Trip> trips = new List<Trip>();
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "SELECT * FROM Trips";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        trips.Add(new Trip
                        {
                            TripID = (int)reader["TripID"],
                            VehicleID = (int)reader["VehicleID"],
                            RouteID = (int)reader["RouteID"],
                            DepartureDate = (DateTime)reader["DepartureDate"],
                            ArrivalDate = (DateTime)reader["ArrivalDate"],
                            Status = reader["Status"].ToString(),
                            TripType = reader["TripType"].ToString(),
                            MaxPassengers = (int)reader["MaxPassengers"],
                            DriverID = reader["DriverID"] != DBNull.Value ? (int?)reader["DriverID"] : null
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving trips: " + ex.Message);
            }
            return trips;
        }

        public List<Route> GetAllRoutes()
        {
            List<Route> routes = new List<Route>();
            try
            {
                using (SqlConnection conn = DBConnection.GetConnection())
                {
                    string query = "SELECT * FROM Routes";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        routes.Add(new Route
                        {
                            RouteID = (int)reader["RouteID"],
                            StartDestination = reader["StartDestination"].ToString(),
                            EndDestination = reader["EndDestination"].ToString(),
                            Distance = Convert.ToDouble(reader["Distance"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving routes: " + ex.Message);
            }
            return routes;
        }
    }

}
