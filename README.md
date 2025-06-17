# 📦 OrderManagement  
This is a sample .NET 8 Web API for managing orders, customers, and discounts. It demonstrates clean architecture, proper design patterns, and robust testing.  
________________________________________  
✅ Discounting System  
•	Applies discount rules dynamically based on customer segments (Regular vs. VIP).  
•	Easily extendable for more advanced promotions.  
✅ Order Status Tracking  
•	Tracks order state with clear transitions (Created → Fulfilled).  
•	Designed for future states like Cancelled or OnHold.  
✅ Order Analytics Endpoint  
•	Provides real-time metrics:  
• Average order value  
•	Average fulfillment time (in hours)  
✅ Testing  
•	Thorough unit tests for discount logic.  
•	Integration tests for creating orders with multiple items and verifying totals.  
✅ Documentation & Swagger  
•	All endpoints are documented with XML comments.  
•	Swagger UI enabled for easy testing and exploration.  
✅ Performance Considerations  
•	Uses AsNoTracking() for read-only queries in analytics to reduce EF Core tracking overhead.  
________________________________________  
✅ Assumptions  
•	Customer must pre-exist; only CustomerId is accepted.  
•	Discounts depend only on customer segment for now.  
•	Testing Environment: Uses EF Core InMemory database for simplicity in tests  
•	Analytics endpoint is real-time; caching can be added.  
________________________________________  
🧩 Design Patterns  
•	Repository & Unit of Work (via EF Core DbContext)  
•	Service Layer (OrderService, DiscountService)  
•	Dependency Injection (configured in Program.cs)  
•	DTOs (API models separate from entities)  
________________________________________   
📬 Author  
Onsongo Moseti    
________________________________________  
📜 License  
Open-source for learning and demonstration.  
________________________________________  


