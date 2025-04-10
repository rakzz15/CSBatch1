using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportManagementSystem.DAO;
using TransportManagementSystem.Entity;
using TransportManagementSystem.Exceptions;
using TransportManagementSystem.Util;

namespace TransportManagementSystem.App
{
    public class TransportManagementApp
    {
        public static void Main(string[] args)
        {
            ITransportService service = new TransportManagementServiceImpl();
            int choice;

            do
            {
                Console.WriteLine("\n==== Transport Management Menu ====");
                Console.WriteLine("1. Add Vehicle");
                Console.WriteLine("2. Update Vehicle");
                Console.WriteLine("3. Delete Vehicle");
                Console.WriteLine("4. Schedule Trip");
                Console.WriteLine("5. Cancel Trip");
                Console.WriteLine("6. Book Trip");
                Console.WriteLine("7. Cancel Booking");
                Console.WriteLine("8. Allocate Driver");
                Console.WriteLine("9. Deallocate Driver");
                Console.WriteLine("10. Get Bookings by Passenger");
                Console.WriteLine("11. Get Bookings by Trip");
                Console.WriteLine("12. Get Available Drivers");
                Console.WriteLine("13. View All Routes");
                Console.WriteLine("0. Exit");

                Console.Write("Enter your choice: ");
                choice = int.Parse(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            Vehicle v = new Vehicle();
                            Console.Write("Model: ");
                            v.Model = Console.ReadLine();
                            Console.Write("Capacity: ");
                            v.Capacity = double.Parse(Console.ReadLine());
                            Console.Write("Type: ");
                            v.Type = Console.ReadLine();
                            Console.Write("Status: ");
                            v.Status = Console.ReadLine();
                            Console.WriteLine(service.AddVehicle(v) ? "Vehicle Added." : "Failed.");
                            break;

                        case 2:
                            Vehicle v2 = new Vehicle();
                            Console.Write("Vehicle ID: ");
                            v2.VehicleID = int.Parse(Console.ReadLine());
                            Console.Write("Model: ");
                            v2.Model = Console.ReadLine();
                            Console.Write("Capacity: ");
                            v2.Capacity = double.Parse(Console.ReadLine());
                            Console.Write("Type: ");
                            v2.Type = Console.ReadLine();
                            Console.Write("Status: ");
                            v2.Status = Console.ReadLine();
                            Console.WriteLine(service.UpdateVehicle(v2) ? "Vehicle Updated." : "Failed.");
                            break;

                        case 3:
                            Console.Write("Vehicle ID: ");
                            int vehicleId = int.Parse(Console.ReadLine());
                            Console.WriteLine(service.DeleteVehicle(vehicleId) ? "Vehicle Deleted." : "Failed.");
                            break;

                        case 4:
                            Console.WriteLine("Available Routes:");
                            List<Route> routes = service.GetAllRoutes();
                            foreach (var r in routes)
                            {
                                Console.WriteLine($"RouteID: {r.RouteID}, From: {r.StartDestination}, To: {r.EndDestination}, Distance: {r.Distance} km");
                            }
                            Console.Write("Vehicle ID: "); int vid = int.Parse(Console.ReadLine());
                            Console.Write("Route ID: "); int rid = int.Parse(Console.ReadLine());
                            Console.Write("Departure Date (yyyy-MM-dd): "); DateTime dep = DateTime.Parse(Console.ReadLine());
                            Console.Write("Arrival Date (yyyy-MM-dd): "); DateTime arr = DateTime.Parse(Console.ReadLine());
                            Console.Write("Max Passengers: "); int max = int.Parse(Console.ReadLine());
                            Console.WriteLine(service.ScheduleTrip(vid, rid, dep, arr, max) ? "Trip Scheduled." : "Failed.");
                            break;

                        case 5:
                            Console.Write("Trip ID: ");
                            int tid = int.Parse(Console.ReadLine());
                            Console.WriteLine(service.CancelTrip(tid) ? "Trip Cancelled." : "Failed.");
                            break;

                        case 6:
                            Console.Write("Trip ID: ");
                            int tripId = int.Parse(Console.ReadLine());
                            Console.Write("Passenger ID: ");
                            int passId = int.Parse(Console.ReadLine());
                            Console.Write("Booking Date (yyyy-MM-dd): ");
                            DateTime bdate = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine(service.BookTrip(tripId, passId, bdate) ? "Booking Confirmed." : "Failed.");
                            break;

                        case 7:
                            Console.Write("Booking ID: "); int bookId = int.Parse(Console.ReadLine());
                            Console.WriteLine(service.CancelBooking(bookId) ? "Booking Cancelled." : "Failed.");
                            break;

                        case 8:
                            Console.Write("Trip ID: ");
                            int trId = int.Parse(Console.ReadLine());
                            Console.Write("Driver ID: ");
                            int drId = int.Parse(Console.ReadLine());
                            Console.WriteLine(service.AllocateDriver(trId, drId) ? "Driver Allocated." : "Failed.");
                            break;

                        case 9:
                            Console.Write("Trip ID: ");
                            int dtripId = int.Parse(Console.ReadLine());
                            Console.WriteLine(service.DeallocateDriver(dtripId) ? "Driver Deallocated." : "Failed.");
                            break;

                        case 10:
                            Console.Write("Passenger ID: ");
                            int pid = int.Parse(Console.ReadLine());
                            List<Booking> bookings = service.GetBookingsByPassenger(pid);
                            foreach (var b in bookings)
                                Console.WriteLine($"BookingID: {b.BookingID}, TripID: {b.TripID}, Status: {b.Status}");
                            break;

                        case 11:
                            Console.Write("Trip ID: ");
                            int trip = int.Parse(Console.ReadLine());
                            List<Booking> tripBookings = service.GetBookingsByTrip(trip);
                            foreach (var b in tripBookings)
                                Console.WriteLine($"BookingID: {b.BookingID}, PassengerID: {b.PassengerID}, Status: {b.Status}");
                            break;

                        case 12:
                            List<Driver> drivers = service.GetAvailableDrivers();
                            foreach (var d in drivers)
                                Console.WriteLine($"DriverID: {d.DriverID}, Name: {d.Name}, License: {d.LicenseNumber}");
                            break;
                        case 13:
                            List<Route> allRoutes = service.GetAllRoutes();
                            foreach (var route in allRoutes)
                            {
                                Console.WriteLine($"RouteID: {route.RouteID}, From: {route.StartDestination}, To: {route.EndDestination}, Distance: {route.Distance} km");
                            }
                            break;
                    }
                }
                catch (VehicleNotFoundException ex)
                {
                    Console.WriteLine("Vehicle Error: " + ex.Message);
                }
                catch (BookingNotFoundException ex)
                {
                    Console.WriteLine("Booking Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

            } while (choice != 0);
        }
    }
}
