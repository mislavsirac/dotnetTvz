using RestoManager.Model.Inventory;
using RestoManager.Model.Menu;
using RestoManager.Model.Orders;
using RestoManager.Model.Restaurants;
using RestoManager.Model.Staff;

namespace RestoManager.Db;

public class Lab1DataStore
{
    public List<Restaurant> Restaurants { get; } = BuildRestaurants();
    public List<Ingredient> Ingredients { get; } = BuildIngredients();

    public Lab1DataStore()
    {
        WireRecipes();
        SeedOrders();
    }

    private void WireRecipes()
    {
        var burgerBun = Ingredients.Single(ingredient => ingredient.Name == "Burger Bun");
        var beefPatty = Ingredients.Single(ingredient => ingredient.Name == "Beef Patty");
        var cheeseSlice = Ingredients.Single(ingredient => ingredient.Name == "Cheese Slice");
        var potato = Ingredients.Single(ingredient => ingredient.Name == "Potato");
        var pizzaDough = Ingredients.Single(ingredient => ingredient.Name == "Pizza Dough");
        var tomatoSauce = Ingredients.Single(ingredient => ingredient.Name == "Tomato Sauce");
        var mozzarella = Ingredients.Single(ingredient => ingredient.Name == "Mozzarella");
        var basil = Ingredients.Single(ingredient => ingredient.Name == "Basil");
        var tortilla = Ingredients.Single(ingredient => ingredient.Name == "Tortilla");
        var chicken = Ingredients.Single(ingredient => ingredient.Name == "Chicken Breast");
        var lettuce = Ingredients.Single(ingredient => ingredient.Name == "Lettuce");

        foreach (var meal in Restaurants.SelectMany(restaurant => restaurant.Meals))
        {
            var recipe = meal.Name switch
            {
                "Classic Burger" => new List<RecipeIngredient>
                {
                    LinkRecipe(meal, burgerBun, 1, "One bun per burger"),
                    LinkRecipe(meal, beefPatty, 150, "Beef patty in grams"),
                    LinkRecipe(meal, cheeseSlice, 1, "Optional cheese slice")
                },
                "Fries" => new List<RecipeIngredient>
                {
                    LinkRecipe(meal, potato, 180, "Fresh cut potato")
                },
                "Margherita Pizza" => new List<RecipeIngredient>
                {
                    LinkRecipe(meal, pizzaDough, 1, "Single pizza base"),
                    LinkRecipe(meal, tomatoSauce, 120, "Sauce in milliliters"),
                    LinkRecipe(meal, mozzarella, 140, "Mozzarella in grams"),
                    LinkRecipe(meal, basil, 6, "Fresh basil leaves")
                },
                "Chicken Wrap" => new List<RecipeIngredient>
                {
                    LinkRecipe(meal, tortilla, 1, "Wrap tortilla"),
                    LinkRecipe(meal, chicken, 130, "Chicken in grams"),
                    LinkRecipe(meal, lettuce, 40, "Lettuce in grams")
                },
                _ => new List<RecipeIngredient>()
            };

            meal.RecipeIngredients = recipe;
        }
    }

    private void SeedOrders()
    {
        var cashierMap = Restaurants
            .SelectMany(restaurant => restaurant.Locations)
            .SelectMany(location => location.StaffMembers)
            .Where(staff => staff.Role == StaffRole.Cashier)
            .ToDictionary(staff => staff.LocationId, staff => staff);

        int orderId = 1;
        int orderItemId = 1;

        foreach (var location in Restaurants.SelectMany(restaurant => restaurant.Locations))
        {
            var availableMeals = Restaurants
                .Single(restaurant => restaurant.Id == location.RestaurantId)
                .Meals
                .Where(meal => meal.IsAvailable)
                .ToList();

            var cashier = cashierMap[location.Id];

            for (int i = 0; i < 3; i++)
            {
                var primaryMeal = availableMeals[i % availableMeals.Count];
                var secondaryMeal = availableMeals[(i + 1) % availableMeals.Count];
                var createdAt = DateTime.Today.AddDays(-i).AddHours(10 + i);

                location.Orders.Add(new Order
                {
                    Id = orderId,
                    LocationId = location.Id,
                    CashierId = cashier.Id,
                    OrderNumber = $"ORD-{location.Id:D2}-{orderId:D4}",
                    CustomerName = $"Customer {location.Id}-{i + 1}",
                    CreatedAt = createdAt,
                    CompletedAt = i == 2 ? null : createdAt.AddMinutes(18 + i * 4),
                    Status = i switch
                    {
                        0 => OrderStatus.Completed,
                        1 => OrderStatus.InPreparation,
                        _ => OrderStatus.New
                    },
                    Notes = i == 1 ? "Extra sauce requested" : "No special notes",
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = orderItemId++,
                            OrderId = orderId,
                            MealId = primaryMeal.Id,
                            MealName = primaryMeal.Name,
                            Quantity = 1 + i,
                            UnitPrice = primaryMeal.Price,
                            SpecialInstructions = "Prepare as standard"
                        },
                        new OrderItem
                        {
                            Id = orderItemId++,
                            OrderId = orderId,
                            MealId = secondaryMeal.Id,
                            MealName = secondaryMeal.Name,
                            Quantity = 1,
                            UnitPrice = secondaryMeal.Price,
                            SpecialInstructions = i == 2 ? "Priority order" : "No onions"
                        }
                    }
                });

                orderId++;
            }
        }

        ApplyInventoryUsage();
    }

    private void ApplyInventoryUsage()
    {
        var usageByIngredientId = Restaurants
            .SelectMany(restaurant => restaurant.Locations)
            .SelectMany(location => location.Orders)
            .Where(order => order.Status != OrderStatus.Cancelled)
            .SelectMany(order => order.Items)
            .Join(
                Restaurants.SelectMany(restaurant => restaurant.Meals),
                item => item.MealId,
                meal => meal.Id,
                (item, meal) => new { item, meal })
            .SelectMany(result => result.meal.RecipeIngredients.Select(recipe => new
            {
                recipe.IngredientId,
                UsedQuantity = recipe.Quantity * result.item.Quantity
            }))
            .GroupBy(result => result.IngredientId)
            .ToDictionary(group => group.Key, group => group.Sum(item => item.UsedQuantity));

        foreach (var ingredient in Ingredients)
        {
            if (usageByIngredientId.TryGetValue(ingredient.Id, out var usedQuantity))
            {
                ingredient.CurrentStock = Math.Max(0, ingredient.CurrentStock - usedQuantity);
            }
        }
    }

    private static RecipeIngredient LinkRecipe(Meal meal, Ingredient ingredient, decimal quantity, string notes)
    {
        var link = new RecipeIngredient
        {
            MealId = meal.Id,
            IngredientId = ingredient.Id,
            Quantity = quantity,
            Notes = notes,
            Meal = meal,
            Ingredient = ingredient
        };

        ingredient.RecipeIngredients.Add(link);
        return link;
    }

    private static List<Restaurant> BuildRestaurants()
    {
        return new List<Restaurant>
        {
            new Restaurant
            {
                Id = 1,
                Name = "RestoManager Central",
                BrandCode = "RMC",
                TaxNumber = "HR10001",
                HeadquartersCity = "Zagreb",
                CreatedAt = new DateTime(2022, 5, 3),
                IsActive = true,
                Locations = new List<Location>
                {
                    BuildLocation(1, 1, "Zagreb Center", "Zagreb", "Ilica 10", true, new DateTime(2022, 6, 1)),
                    BuildLocation(2, 1, "Zagreb East", "Zagreb", "Maksimirska 22", true, new DateTime(2023, 2, 12))
                },
                Meals = new List<Meal>
                {
                    new Meal
                    {
                        Id = 1,
                        RestaurantId = 1,
                        Name = "Classic Burger",
                        Category = "Main",
                        Price = 10.50m,
                        PreparationTimeMinutes = 15,
                        IsAvailable = true,
                        AddedToMenuAt = new DateTime(2022, 6, 1)
                    },
                    new Meal
                    {
                        Id = 2,
                        RestaurantId = 1,
                        Name = "Fries",
                        Category = "Side",
                        Price = 4.20m,
                        PreparationTimeMinutes = 8,
                        IsAvailable = true,
                        AddedToMenuAt = new DateTime(2022, 6, 1)
                    }
                }
            },
            new Restaurant
            {
                Id = 2,
                Name = "RestoManager Adriatic",
                BrandCode = "RMA",
                TaxNumber = "HR10002",
                HeadquartersCity = "Split",
                CreatedAt = new DateTime(2021, 9, 15),
                IsActive = true,
                Locations = new List<Location>
                {
                    BuildLocation(3, 2, "Split Riva", "Split", "Obala 5", true, new DateTime(2021, 10, 4)),
                    BuildLocation(4, 2, "Trogir Port", "Trogir", "Riva 2", false, new DateTime(2024, 1, 8))
                },
                Meals = new List<Meal>
                {
                    new Meal
                    {
                        Id = 3,
                        RestaurantId = 2,
                        Name = "Margherita Pizza",
                        Category = "Main",
                        Price = 12.90m,
                        PreparationTimeMinutes = 14,
                        IsAvailable = true,
                        AddedToMenuAt = new DateTime(2021, 10, 4)
                    },
                    new Meal
                    {
                        Id = 4,
                        RestaurantId = 2,
                        Name = "Fries",
                        Category = "Side",
                        Price = 4.00m,
                        PreparationTimeMinutes = 8,
                        IsAvailable = true,
                        AddedToMenuAt = new DateTime(2021, 10, 4)
                    }
                }
            },
            new Restaurant
            {
                Id = 3,
                Name = "RestoManager North",
                BrandCode = "RMN",
                TaxNumber = "HR10003",
                HeadquartersCity = "Varazdin",
                CreatedAt = new DateTime(2023, 1, 5),
                IsActive = true,
                Locations = new List<Location>
                {
                    BuildLocation(5, 3, "Varazdin Old Town", "Varazdin", "Kapucinski trg 3", true, new DateTime(2023, 1, 20)),
                    BuildLocation(6, 3, "Cakovec Square", "Cakovec", "Trg Republike 7", true, new DateTime(2023, 4, 10))
                },
                Meals = new List<Meal>
                {
                    new Meal
                    {
                        Id = 5,
                        RestaurantId = 3,
                        Name = "Chicken Wrap",
                        Category = "Main",
                        Price = 12.40m,
                        PreparationTimeMinutes = 12,
                        IsAvailable = true,
                        AddedToMenuAt = new DateTime(2023, 1, 20)
                    },
                    new Meal
                    {
                        Id = 6,
                        RestaurantId = 3,
                        Name = "Fries",
                        Category = "Side",
                        Price = 4.10m,
                        PreparationTimeMinutes = 8,
                        IsAvailable = true,
                        AddedToMenuAt = new DateTime(2023, 1, 20)
                    }
                }
            }
        };
    }

    private static Location BuildLocation(int id, int restaurantId, string name, string city, string address, bool isOpen, DateTime openedAt)
    {
        return new Location
        {
            Id = id,
            RestaurantId = restaurantId,
            Name = name,
            City = city,
            Address = address,
            PhoneNumber = $"+385-1-555-00{id}",
            IsOpen = isOpen,
            OpenedAt = openedAt,
            StaffMembers = new List<StaffMember>
            {
                new StaffMember
                {
                    Id = id * 10 + 1,
                    LocationId = id,
                    FirstName = "Ana",
                    LastName = $"Manager{id}",
                    Email = $"manager{id}@restomanager.hr",
                    PhoneNumber = $"+385-91-100-00{id}",
                    Role = StaffRole.Manager,
                    EmploymentStartDate = openedAt,
                    IsActive = true
                },
                new StaffMember
                {
                    Id = id * 10 + 2,
                    LocationId = id,
                    FirstName = "Ivan",
                    LastName = $"Cashier{id}",
                    Email = $"cashier{id}@restomanager.hr",
                    PhoneNumber = $"+385-91-200-00{id}",
                    Role = StaffRole.Cashier,
                    EmploymentStartDate = openedAt.AddDays(10),
                    IsActive = true
                },
                new StaffMember
                {
                    Id = id * 10 + 3,
                    LocationId = id,
                    FirstName = "Marko",
                    LastName = $"Chef{id}",
                    Email = $"chef{id}@restomanager.hr",
                    PhoneNumber = $"+385-91-300-00{id}",
                    Role = StaffRole.Chef,
                    EmploymentStartDate = openedAt.AddDays(20),
                    IsActive = true
                }
            }
        };
    }

    private static List<Ingredient> BuildIngredients()
    {
        return new List<Ingredient>
        {
            new Ingredient
            {
                Id = 1,
                Name = "Burger Bun",
                SupplierName = "Bakery Plus",
                Unit = IngredientUnit.Piece,
                CurrentStock = 24,
                ReorderLevel = 12,
                CaloriesPer100Units = 270,
                LastDeliveryAt = DateTime.Today.AddDays(-2),
                IsAllergen = true
            },
            new Ingredient
            {
                Id = 2,
                Name = "Beef Patty",
                SupplierName = "Meat House",
                Unit = IngredientUnit.Gram,
                CurrentStock = 2600,
                ReorderLevel = 900,
                CaloriesPer100Units = 250,
                LastDeliveryAt = DateTime.Today.AddDays(-1),
                IsAllergen = false
            },
            new Ingredient
            {
                Id = 3,
                Name = "Cheese Slice",
                SupplierName = "Dairy Land",
                Unit = IngredientUnit.Piece,
                CurrentStock = 18,
                ReorderLevel = 10,
                CaloriesPer100Units = 330,
                LastDeliveryAt = DateTime.Today.AddDays(-2),
                IsAllergen = true
            },
            new Ingredient
            {
                Id = 4,
                Name = "Potato",
                SupplierName = "Farm Fresh",
                Unit = IngredientUnit.Gram,
                CurrentStock = 2200,
                ReorderLevel = 1600,
                CaloriesPer100Units = 77,
                LastDeliveryAt = DateTime.Today.AddDays(-3),
                IsAllergen = false
            },
            new Ingredient
            {
                Id = 5,
                Name = "Pizza Dough",
                SupplierName = "Bakery Plus",
                Unit = IngredientUnit.Piece,
                CurrentStock = 8,
                ReorderLevel = 10,
                CaloriesPer100Units = 265,
                LastDeliveryAt = DateTime.Today.AddDays(-1),
                IsAllergen = true
            },
            new Ingredient
            {
                Id = 6,
                Name = "Tomato Sauce",
                SupplierName = "Mediterraneo",
                Unit = IngredientUnit.Milliliter,
                CurrentStock = 900,
                ReorderLevel = 500,
                CaloriesPer100Units = 29,
                LastDeliveryAt = DateTime.Today.AddDays(-4),
                IsAllergen = false
            },
            new Ingredient
            {
                Id = 7,
                Name = "Mozzarella",
                SupplierName = "Dairy Land",
                Unit = IngredientUnit.Gram,
                CurrentStock = 950,
                ReorderLevel = 600,
                CaloriesPer100Units = 280,
                LastDeliveryAt = DateTime.Today.AddDays(-2),
                IsAllergen = true
            },
            new Ingredient
            {
                Id = 8,
                Name = "Basil",
                SupplierName = "Green Market",
                Unit = IngredientUnit.Gram,
                CurrentStock = 120,
                ReorderLevel = 40,
                CaloriesPer100Units = 23,
                LastDeliveryAt = DateTime.Today.AddDays(-1),
                IsAllergen = false
            },
            new Ingredient
            {
                Id = 9,
                Name = "Tortilla",
                SupplierName = "Bakery Plus",
                Unit = IngredientUnit.Piece,
                CurrentStock = 11,
                ReorderLevel = 10,
                CaloriesPer100Units = 310,
                LastDeliveryAt = DateTime.Today.AddDays(-2),
                IsAllergen = true
            },
            new Ingredient
            {
                Id = 10,
                Name = "Chicken Breast",
                SupplierName = "Meat House",
                Unit = IngredientUnit.Gram,
                CurrentStock = 1200,
                ReorderLevel = 700,
                CaloriesPer100Units = 165,
                LastDeliveryAt = DateTime.Today.AddDays(-1),
                IsAllergen = false
            },
            new Ingredient
            {
                Id = 11,
                Name = "Lettuce",
                SupplierName = "Green Market",
                Unit = IngredientUnit.Gram,
                CurrentStock = 400,
                ReorderLevel = 180,
                CaloriesPer100Units = 15,
                LastDeliveryAt = DateTime.Today.AddDays(-1),
                IsAllergen = false
            }
        };
    }
}
