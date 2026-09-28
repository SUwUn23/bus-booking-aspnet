using BusBookingSystem.Data;
using BusBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!context.Routes.Any())
        {
            var routes = new List<Route>
            {
                new Route { FromCity = "Hà Nội", ToCity = "Đà Nẵng", BasePrice = 250000 },
                new Route { FromCity = "Hà Nội", ToCity = "TP. Hồ Chí Minh", BasePrice = 350000 },
                new Route { FromCity = "Đà Nẵng", ToCity = "TP. Hồ Chí Minh", BasePrice = 260000 },
                new Route { FromCity = "TP. Hồ Chí Minh", ToCity = "Cần Thơ", BasePrice = 120000 },
                new Route { FromCity = "Hà Nội", ToCity = "Hải Phòng", BasePrice = 100000 }
            };

            context.Routes.AddRange(routes);
            await context.SaveChangesAsync();
        }

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            await roleManager.CreateAsync(new IdentityRole("Customer"));
        }

        var admin = await userManager.FindByEmailAsync("admin@busbooking.local");
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = "admin@busbooking.local",
                Email = "admin@busbooking.local",
                FullName = "Administrator",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        var customer = await userManager.FindByEmailAsync("customer@busbooking.local");
        if (customer == null)
        {
            customer = new ApplicationUser
            {
                UserName = "customer@busbooking.local",
                Email = "customer@busbooking.local",
                FullName = "Customer Test",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(customer, "Customer123!");
            await userManager.AddToRoleAsync(customer, "Customer");
        }

        if (!context.Trips.Any())
        {
            var routes = context.Routes.ToList();
            var trips = new List<Trip>
            {
                new Trip { RouteId = routes[0].Id, BusName = "Tanna Travel", LicensePlate = "29A-101.12", DepartureDate = DateTime.Today.AddDays(1), DepartureTime = new TimeSpan(6,0,0), ArrivalTime = new TimeSpan(14,0,0), TotalSeats = 40, Fare = 260000 },
                new Trip { RouteId = routes[1].Id, BusName = "Luxury Bus", LicensePlate = "29A-101.13", DepartureDate = DateTime.Today.AddDays(1), DepartureTime = new TimeSpan(8,30,0), ArrivalTime = new TimeSpan(18,0,0), TotalSeats = 40, Fare = 360000 },
                new Trip { RouteId = routes[2].Id, BusName = "Comfort Coach", LicensePlate = "29A-101.14", DepartureDate = DateTime.Today.AddDays(2), DepartureTime = new TimeSpan(7,0,0), ArrivalTime = new TimeSpan(15,30,0), TotalSeats = 36, Fare = 280000 },
                new Trip { RouteId = routes[3].Id, BusName = "Local Express", LicensePlate = "29A-101.15", DepartureDate = DateTime.Today.AddDays(2), DepartureTime = new TimeSpan(5,0,0), ArrivalTime = new TimeSpan(9,30,0), TotalSeats = 40, Fare = 120000 }
            };

            context.Trips.AddRange(trips);
            await context.SaveChangesAsync();

            var createdTrips = context.Trips.Include(t => t.Route).ToList();
            foreach (var trip in createdTrips)
            {
                var seatList = new List<Seat>();
                var rows = new[] { "A", "B", "C", "D" };
                var index = 1;
                for (int i = 1; i <= trip.TotalSeats; i++)
                {
                    var row = rows[(i - 1) % rows.Length];
                    var code = row + index;
                    if ((i - 1) % rows.Length == rows.Length - 1) index++;
                    seatList.Add(new Seat { TripId = trip.Id, SeatCode = code, IsBooked = false });
                }

                context.Seats.AddRange(seatList);
            }

            await context.SaveChangesAsync();
        }
    }
}
