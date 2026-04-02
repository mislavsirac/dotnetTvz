# RestoManager Project Plan

## Overview

RestoManager is a restaurant management application focused on improving day-to-day restaurant operations. Its primary goal is to enable smooth communication between the cashier and kitchen by allowing orders to be created at the cashier desk and displayed to kitchen staff in real time.

The platform is designed to support both small restaurants with a single location and larger restaurant businesses with multiple branches. Each restaurant acts as a separate business entity, with one or more locations and location-specific staff.

## Core Goals

- Simplify order management between cashier and kitchen staff
- Improve operational speed and reduce communication delays
- Support multiple restaurants and multiple locations per restaurant
- Provide role-based access for managers, cashiers, and chefs
- Track ingredients and stock automatically based on placed orders
- Create a foundation for future nutrition and food-data integrations

## Business Structure

### Restaurant Hierarchy

- One system can support multiple restaurants
- Each restaurant is an independent entity
- Each restaurant can have multiple locations
- Each location has its own staff and operational data

### Staff Roles

- Manager
  - Oversees restaurant and location operations
  - Manages staff, menu items, and inventory settings
- Cashier
  - Creates and manages customer orders
  - Can mark orders as completed when needed
- Chef
  - Views incoming orders in real time
  - Prepares meals and can mark orders as completed

## Functional Requirements

### 1. Order Management

- Cashier can create customer orders
- Orders are instantly visible to kitchen staff
- Orders move through statuses such as:
  - New
  - In Preparation
  - Completed
  - Cancelled
- Orders can be completed by either chef or cashier
- Order history should be stored for reporting and review

### 2. Real-Time Kitchen Display

- Kitchen staff should receive new orders in real time
- The kitchen view should clearly show:
  - Order items
  - Time created
  - Current status
  - Special notes or instructions

### 3. Multi-Restaurant and Multi-Location Support

- Data must be separated by restaurant
- Each location should manage its own:
  - Staff
  - Orders
  - Inventory
  - Menu availability
- Managers may eventually need access across multiple locations within the same restaurant

### 4. Inventory Management

- Ingredients and stock levels should be tracked
- Meals should define recipes or ingredient usage per portion
- When an order is placed, inventory should automatically decrease based on meal composition
- Inventory should be managed at the location level

### 5. Meal and Recipe Definitions

- Each meal should include:
  - Name
  - Price
  - Category
  - Portion definition
  - Ingredient list
- Portion measurements should be explicit so stock updates are predictable and accurate

## Future Enhancements

### Low-Stock Alerts

- Notify staff or managers when ingredient levels fall below a threshold

### USDA Food Database Integration

- Connect ingredients with USDA food data
- Calculate calories and nutritional values for each meal
- Support recipe-based nutrition summaries

### Reporting and Analytics

- Sales reports by location and restaurant
- Inventory usage trends
- Most ordered meals
- Staff activity summaries

## Suggested Domain Model

### Main Entities

- Restaurant
- Location
- User
- Role
- Order
- OrderItem
- Meal
- Recipe
- RecipeIngredient
- Ingredient
- InventoryItem
- StockTransaction

### Relationship Summary

- A `Restaurant` has many `Locations`
- A `Location` has many `Users`
- A `Location` has many `Orders`
- A `Location` has many `InventoryItems`
- A `Meal` has one recipe definition
- A `Recipe` has many `RecipeIngredients`
- An `Order` has many `OrderItems`

## Suggested Phased Delivery

### Phase 1 - Foundation

- Set up core project architecture
- Define database schema and domain entities
- Implement restaurant, location, and user management
- Add role-based authentication and authorization

### Phase 2 - Order Flow

- Implement order creation by cashier
- Build kitchen order display
- Add real-time order updates
- Support order status changes and completion

### Phase 3 - Inventory

- Implement ingredient and inventory management
- Define meal recipes and portion logic
- Reduce stock automatically when orders are placed

### Phase 4 - Operations and Insights

- Add reporting features
- Add low-stock alerts
- Improve manager tools and dashboards

### Phase 5 - Nutrition Integration

- Integrate USDA food data
- Calculate nutritional values per meal
- Display calorie and nutrition summaries

## Non-Functional Considerations

- Real-time communication should be reliable and responsive
- Data isolation between restaurants must be strict
- Inventory calculations must be accurate and auditable
- The system should be scalable enough to support multiple locations and concurrent users
- UI should be simple and optimized for fast use in a restaurant environment

## Initial MVP Recommendation

The first MVP should focus on the features that provide the biggest operational value:

- Multi-location-ready architecture
- User roles for manager, cashier, and chef
- Order creation and kitchen visibility
- Real-time order status updates
- Basic inventory deduction from orders

This would provide a strong working base while leaving room for advanced analytics, alerts, and nutrition features later.

## Project Vision Statement

RestoManager aims to become a reliable digital operations hub for restaurants by connecting front-desk order entry, kitchen workflow, and inventory tracking into one practical system. The long-term vision is to help restaurants operate faster, reduce errors, and gain better visibility into both service and stock usage across all locations.
