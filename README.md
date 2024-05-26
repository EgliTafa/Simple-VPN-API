# Simple VPN App Frontend

## Description
The Simple VPN App Frontend is a user-friendly WPF application designed for secure login, registration, and VPN management, seamlessly interacting with an ASP.NET Core backend. This project utilizes technologies such as WPF, C#, .NET, and integrates with a backend built on ASP.NET Core.

## Technologies Used
- **WPF (Windows Presentation Foundation)**: For creating a rich desktop application interface.
- **C#**: The primary programming language for the application logic.
- **.NET**: The framework for building and running the application.
- **ASP.NET Core**: The backend service providing authentication and VPN management APIs.
- **Newtonsoft.Json**: For JSON serialization and deserialization.
- **HttpClient**: For making HTTP requests to the backend APIs.

## Features
### User Registration
- **Form-based Registration**: Users can create a new account by providing a username, first name, last name, email, and password.
- **Validation**: Client-side validation ensures all required fields are filled out correctly before submission.

### User Login
- **Secure Login**: Users can log in with their credentials to access the VPN management features.
- **Token-Based Authentication**: Upon successful login, a JWT token is retrieved and stored for authenticated requests.

### ~~VPN Management~~ WIP
- **Connect to VPN**: Users can connect to the VPN using their credentials.
- **Disconnect from VPN**: Users can disconnect from the VPN when it is no longer needed.
- **Status Monitoring**: Users can check the status of their VPN connection.

### JWT Authentication
- **Secure API Requests**: All API requests are authenticated using JWT tokens, ensuring secure communication between the client and server.
- **Token Handling**: The application automatically manages token storage and includes the token in the headers of all authenticated requests.

## How to Use
1. **Clone the Repository**:
    ```sh
    git clone https://github.com/yourusername/simple-vpn-app-frontend.git
    cd simple-vpn-app-frontend
    ```

2. **Open the Solution**:
   - Open the `Simple-VPN-App-Frontend.sln` file in Visual Studio.

3. **Restore NuGet Packages**:
   - In Visual Studio, go to `Tools` > `NuGet Package Manager` > `Package Manager Console` and run:
     ```sh
     Update-Package -reinstall
     ```

4. **Update API URLs**:
   - Ensure the `apiURL` in `ApiService.cs` is pointing to your backend URL:
     ```csharp
     private readonly string apiURL = "https://localhost:7214";
     ```

5. **Build and Run the Application**:
   - Build the solution in Visual Studio by selecting `Build` > `Build Solution`.
   - Run the application by selecting `Debug` > `Start Debugging`.


