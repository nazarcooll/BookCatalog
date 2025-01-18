
# Books Catalog

This project is a Books Catalog application built with .NET 8 and Docker Compose. The application provides an interface to manage a collection of books, including functionalities such as listing, adding, editing, and deleting book entries.

## Minimum Requirements

To run this application on your machine, ensure the following prerequisites are met:

### 1. Operating System
- **Windows 10/11** (with WSL 2 enabled for Docker)
- **macOS** (latest version recommended)
- **Linux** (Ubuntu 20.04 or higher)

### 2. Software Requirements
- **.NET SDK**: Version 8.0 or later
  - Download from the official [.NET downloads page](https://dotnet.microsoft.com/download/dotnet).
- **Docker**: Latest version (Docker Desktop for Windows/Mac or Docker Engine for Linux)
  - Install Docker from the [Docker website](https://www.docker.com/products/docker-desktop).
- **Docker Compose**: Included with Docker Desktop or standalone installation for Linux.
- **Git**: To clone the repository.

### 3. Hardware Requirements
- **Processor**: Dual-core CPU or higher
- **Memory**: 8 GB RAM minimum (16 GB recommended)
- **Disk Space**: At least 10 GB free

## Setup and Running the Application

### 1. Clone the Repository

```bash
git clone https://github.com/your-repo/books-catalog.git
cd books-catalog
```

### 2. Build and Run with Docker Compose

Ensure Docker is running before proceeding.

#### Steps:

1. Build and start the application using Docker Compose:

   ```bash
   docker-compose up --build
   ```

2. Open your browser and navigate to:
   
   - **Client Application**: `http://localhost:57209`
   - **API Endpoints**: `http://localhost:57210` and `http://localhost:57211`

### 3. Stop the Application

To stop the application, run:

```bash
docker-compose down
```

### 4. Clean Up (Optional)

To remove all containers, networks, and volumes created by Docker Compose:

```bash
docker-compose down --volumes
```

## Development Environment

If you want to run the application locally without Docker:

1. Install the required .NET 8 SDK.
2. Navigate to the project folder:

   ```bash
   cd src/BooksCatalog
   ```

3. Restore dependencies:

   ```bash
   dotnet restore
   ```

4. Run the application:

   ```bash
   dotnet run
   ```

5. Open your browser and go to `http://localhost:57209` for the client application.

## Additional Notes

- **Configuration**: Application settings are stored in `appsettings.json`.
- **Database**: The application uses a seeded database for demonstration purposes. Data persistence is managed through Docker volumes when running via Docker Compose.
- **Ports**: Ensure ports 57209 (client app), 57210, and 57211 (API) are not in use by other applications.

## Support

For any issues or questions, please open an issue on the [GitHub repository](https://github.com/your-repo/books-catalog/issues).
