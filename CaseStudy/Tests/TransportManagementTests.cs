using System;
using System.Collections.Generic;
using NUnit.Framework;
using TransportManagementSystem.DAO;
using TransportManagementSystem.Entity;
using TransportManagementSystem.Exceptions;


namespace TransportManagementSystem.Tests
{

    [TestFixture]
    public class TransportManagementTests
    {
        private ITransportService service;

        [SetUp]
        public void Setup()
        {
            service = new TransportManagementServiceImpl();
        }

        [Test]
        public void AddVehicle_ShouldReturnTrue()
        {
            Vehicle vehicle = new Vehicle
            {
                Model = "Tata Ace",
                Capacity = 2.5,
                Type = "Mini Truck",
                Status = "Available"
            };

            bool result = service.AddVehicle(vehicle);
            Assert.IsTrue(result);
        }

        [Test]
        public void BookTrip_ShouldReturnTrue()
        {
            bool result = service.BookTrip(1, 1, DateTime.Now);
            Assert.IsTrue(result);
        }

        [Test]
        public void AllocateDriver_ShouldReturnTrue()
        {
            bool result = service.AllocateDriver(1, 1); // assumes Trip ID 1 and Driver ID 1 exist
            Assert.IsTrue(result);
        }

        [Test]
        public void VehicleNotFoundException_ShouldBeThrown()
        {
            try
            {
                bool result = service.DeleteVehicle(99999); // unlikely to exist
                Assert.IsFalse(result); // should return false
            }
            catch (VehicleNotFoundException ex)
            {
                StringAssert.Contains("not found", ex.Message);
            }
        }

        [Test]
        public void BookingNotFoundException_ShouldBeHandled()
        {
            try
            {
                List<Booking> result = service.GetBookingsByPassenger(99999); // unlikely to exist
                Assert.IsEmpty(result); // should return empty
            }
            catch (BookingNotFoundException ex)
            {
                StringAssert.Contains("not found", ex.Message);
            }
        }
    }
}
