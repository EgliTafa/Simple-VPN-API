# Simple VPN App ASP.NET Core API

## Description
The Simple VPN App ASP.NET Core API is a robust backend service designed to handle user registration, authentication, and VPN management. It leverages modern technologies such as JWT for secure token-based authentication, OpenVPN.NET for VPN management, and ASP.NET Core Identity for managing user accounts and roles.

## Technologies Used
- **ASP.NET Core**: The framework for building the backend API.
- **JWT (JSON Web Tokens)**: For secure token-based authentication.
- **OpenVPN.NET**: For managing VPN connections.
- **ASP.NET Core Identity**: For handling user authentication and authorization.
- **Entity Framework Core**: For database interactions.
- **Microsoft.Extensions.Configuration**: For handling application settings and configuration.

## Features
### User Registration and Login
- **Secure Registration**: Allows users to create accounts with username, password, and additional details.
- **Token-Based Authentication**: Issues JWT tokens upon successful login for secure API access.

### VPN Management
- **Connect to VPN**: Endpoint to initiate a VPN connection using OpenVPN.NET.
- **Disconnect from VPN**: Endpoint to terminate the VPN connection.
- **Status Monitoring**: Provides the current status of the VPN connection.

### User Management
- **Role-Based Access Control**: Manages user roles and permissions using ASP.NET Core Identity.

## How to Use
1. **Clone the Repository**:
    ```sh
    git clone https://github.com/yourusername/simple-vpn-app-api.git
    cd simple-vpn-app-api
    ```

2. **Update the Configuration**:
   - Update `appsettings.json` with your database connection string and JWT settings.
   - Ensure the OpenVPN configuration files are correctly referenced in your application.

3. **Apply Migrations**:
   - In Visual Studio, go to `Tools` > `NuGet Package Manager` > `Package Manager Console` and run:
     ```sh
     Update-Database
     ```

4. **Run the Application**:
   - Build and run the solution in Visual Studio by selecting `Build` > `Build Solution` and then `Debug` > `Start Debugging`.

5. **Testing the API**:
   - Use tools like Postman or curl to interact with the API endpoints for registration, login, and VPN management.

## Endpoints
- **POST /Register**: Register a new user.
- **POST /Login**: Authenticate a user and issue a JWT token.
- **POST /Logout**: Log out the current user.
- **POST /Vpn/Connect**: Connect to the VPN.
- **POST /Vpn/Disconnect**: Disconnect from the VPN.
- **GET /Vpn/Status**: Get the current VPN connection status.

