<img width="1010" height="562" alt="image" src="https://github.com/user-attachments/assets/8300091a-c148-4035-bad6-3c56fe69d025" />

# ⚡ Smart Energy Meter API

A RESTful API built with **ASP.NET Core 8** to collect, store, and serve energy meter readings from IoT devices such as **ESP8266**. The API supports **CRUD operations**, CSV export, pagination, and filtering by device or date range.

---

## 🌟 Features

- 📥 **POST** energy readings from devices
- 📄 **GET** single or multiple readings with pagination
- 🕒 Retrieve the **latest reading** per device
- 💾 Export readings as **CSV**
- 🌐 Supports **CORS** for IoT devices like ESP8266
- 📊 In-memory or SQL Server database support
- 🛠️ Auto-generated **Swagger UI** for API testing

---

## 🛠️ Project Structure

SmartEnergyMeterAPI/
│
├── Controllers/
│ └── EnergyMeterController.cs # Main API controller
│
├── Models/
│ ├── EnergyReading.cs # Database model
│ └── EnergyReadingDto.cs # DTO for incoming POST requests
│
├── Data/
│ └── ApplicationDbContext.cs # EF Core DbContext
│
├── Program.cs # Application entry point
├── SmartEnergyMeterAPI.csproj # Project file with dependencies
└── README.md

yaml
Copy code

---

## ⚙️ Setup Instructions

### 1. Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 or VS Code
- Optional: SQL Server for persistent storage

### 2. Clone Repository

```bash
git clone https://github.com/yourusername/SmartEnergyMeterAPI.git
cd SmartEnergyMeterAPI
```
### 3. Configure Database
By default, In-Memory Database is used for development.

To use SQL Server, add a connection string in appsettings.json:

json
Copy code
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EnergyMeterDb;Trusted_Connection=True;"
}
The API will automatically switch to SQL Server if the connection string is provided.

### 4. Run the API
bash
Copy code
dotnet run
Swagger UI will be available at https://localhost:{PORT}/swagger

## 📝 API Endpoints
POST /api/EnergyMeter/reading
Adds a new energy reading

Body (JSON)

json
Copy code
{
  "voltage": 230.5,
  "current": 5.2,
  "power": 1200.4,
  "frequency": 50.0,
  "powerFactor": 0.98,
  "deviceId": "ESP8266_01"
}
Response

201 Created with the saved reading

GET /api/EnergyMeter/reading/{id}
Retrieve a specific reading by its ID

GET /api/EnergyMeter/readings
Retrieve multiple readings with optional query parameters:

Parameter	Type	Description
page	int	Page number (default 1)
pageSize	int	Records per page (default 50)
deviceId	string	Filter by device ID
fromDate	DateTime	Start date filter
toDate	DateTime	End date filter

GET /api/EnergyMeter/readings/latest
Retrieve the latest reading for a specific device (optional deviceId query)

GET /api/EnergyMeter/readings/csv
Export readings as CSV

Optional filters: deviceId, fromDate, toDate

Response

CSV file with columns: DateTime,Voltage,Current,Power,Frequency,PowerFactor,DeviceId

## 🧩 Dependencies
Microsoft.EntityFrameworkCore.SqlServer

Microsoft.EntityFrameworkCore.InMemory

Microsoft.EntityFrameworkCore.Tools

Swashbuckle.AspNetCore (Swagger)

## ⚠️ Notes
##🛡️ CORS is enabled for ESP8266 devices

Time is stored in UTC

Suitable for IoT integrations and energy monitoring dashboards

## 📄 License
This project is licensed under the MIT License.
