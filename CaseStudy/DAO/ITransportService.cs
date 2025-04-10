using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportManagementSystem.Entity;
using TransportManagementSystem.Util;

namespace TransportManagementSystem.DAO
{
    public interface ITransportService
    {

        // Vehicle Management
        bool AddVehicle(Vehicle vehicle);
        bool UpdateVehicle(Vehicle vehicle);
        bool DeleteVehicle(int vehicleId);

        // Trip Management
        bool ScheduleTrip(int vehicleId, int routeId, DateTime departureDate, DateTime arrivalDate, int maxPassengers);
        bool CancelTrip(int tripId);
        List<Route> GetAllRoutes();


        // Booking Management
        bool BookTrip(int tripId, int passengerId, DateTime bookingDate);
        bool CancelBooking(int bookingId);

        // Driver Management
        bool AllocateDriver(int tripId, int DriverId);
        bool DeallocateDriver(int tripId);

        // Booking Retrieval
        List<Booking> GetBookingsByPassenger(int passengerId);
        List<Booking> GetBookingsByTrip(int tripId);

        // Driver Availability
        List<Driver> GetAvailableDrivers();

        List<Vehicle> GetAllVehicles();
        List<Trip> GetAllTrips();
    }
}
