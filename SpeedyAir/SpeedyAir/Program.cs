using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AirFreightApp
{
    class Program
    {
        public class Flight
        {
            public int FlightNumber { get; set; }
            public string Departure { get; set; }
            public string Arrival { get; set; }
            public int Day { get; set; }
            public int Capacity { get; set; } = 20;
        }

        public class Order
        {
            public string OrderId { get; set; }
            public string Departure { get; set; }
            public string Destination { get; set; }
            public bool IsScheduled { get; set; } = false;
            public int FlightNumber { get; set; } = -1;
        }

        static void Main(string[] args)
        {
            #region User story 1: Load flight schedule

            List<Flight> flights = new List<Flight>
            {
                new Flight { FlightNumber = 1, Departure = "YUL", Arrival = "YYZ", Day = 1 },
                new Flight { FlightNumber = 2, Departure = "YUL", Arrival = "YYC", Day = 1 },
                new Flight { FlightNumber = 3, Departure = "YUL", Arrival = "YVR", Day = 1 },
                new Flight { FlightNumber = 4, Departure = "YUL", Arrival = "YYZ", Day = 2 },
                new Flight { FlightNumber = 5, Departure = "YUL", Arrival = "YYC", Day = 2 },
                new Flight { FlightNumber = 6, Departure = "YUL", Arrival = "YVR", Day = 2 }
            };
            Console.WriteLine("Flight Schedule:");
            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight: {flight.FlightNumber}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
            }
            Console.WriteLine();

            #endregion


            #region User story 2: Generate flight itineraries

            // Load orders and assign to flights
            List<Order> orders = GetOrders("coding-assigment-orders.json");
            AssignOrdersToFlights(orders, flights);

            // Show order assignments
            ShowOrderAssignments(orders);

            #endregion
        }


        // Load orders from json file
        static List<Order> GetOrders(string filePath)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, filePath);
            var json = File.ReadAllText(fullPath);
            var ordersDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            var orders = new List<Order>();
            foreach (var kvp in ordersDict)
            {
                orders.Add(new Order { OrderId = kvp.Key, Destination = kvp.Value["destination"] });
            }
            return orders;
        }

        // Assigning orders to flights
        static void AssignOrdersToFlights(List<Order> orders, List<Flight> flights)
        {
            foreach (var order in orders)
            {
                foreach (var flight in flights)
                {
                    // If the order destination matches the flight and the flight is not full
                    if (order.Destination == flight.Arrival && flight.Capacity > 0)
                    {
                        order.Departure = flight.Departure;
                        order.IsScheduled = true;
                        order.FlightNumber = flight.FlightNumber;
                        flight.Capacity--; // Reduced flight capacity
                        break;
                    }
                }
            }
        }

        // Show order assignments
        static void ShowOrderAssignments(List<Order> orders)
        {
            Console.WriteLine("Order Assignments:");
            foreach (var order in orders)
            {
                if (order.IsScheduled)
                {
                    Console.WriteLine($"order: {order.OrderId}, flightNumber: {order.FlightNumber}, departure: {order.Departure}, arrival: {order.Destination}");
                }
                else
                {
                    Console.WriteLine($"order: {order.OrderId}, flightNumber: not scheduled");
                }
            }
        }
    }
}


