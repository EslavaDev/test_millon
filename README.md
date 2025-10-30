# Real Estate Property Management System

A full-stack web application for managing real estate properties, built with .NET 9, React, TypeScript, and MongoDB.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

- **Backend**: .NET 9 Web API with MongoDB
  - Clean Architecture (Domain, Application, Infrastructure, API layers)
  - SOLID principles
  - Comprehensive unit and integration tests
- **Frontend**: React 18 + TypeScript + Vite
  - Component-based architecture
  - State management with Zustand
  - Data fetching with SWR
  - Responsive design with Tailwind CSS 4.x

## 📋 Features

- **Property Management**
  - View paginated list of properties
  - Advanced filtering (name, address, price range, year)
  - Sorting (by name, address, price, year)
  - Detailed property view with images and ownership history

- **Performance Optimized**
  - MongoDB indexes for fast queries
  - SWR caching for efficient data fetching
  - Lazy loading images
  - Responsive design for all devices

- **Developer Experience**
  - Comprehensive API documentation (Swagger/OpenAPI)
  - Type-safe APIs with TypeScript
  - Automated testing (119 frontend tests, 114 backend tests)
  - Code formatting and linting

## 🚀 Technology Stack

### Backend
- **.NET 9**: Latest C# features and performance improvements
- **MongoDB**: NoSQL database with flexible schema
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Validation library
- **Swagger/OpenAPI**: API documentation
- **NUnit**: Testing framework

### Frontend
- **React 18**: Modern React with hooks
- **TypeScript**: Type-safe JavaScript
- **Vite**: Fast build tool
- **Tailwind CSS 4.x**: Utility-first CSS framework
- **React Router**: Client-side routing
- **SWR**: Data fetching and caching
- **Zustand**: Lightweight state management
- **Vitest**: Fast unit testing
- **React Testing Library**: Component testing

## 📦 Project Structure

```
.
├── backend/                 # .NET Backend
│   ├── src/
│   │   ├── RealEstate.Domain/          # Domain entities and interfaces
│   │   ├── RealEstate.Application/     # Business logic and DTOs
│   │   ├── RealEstate.Infrastructure/  # Data access and external services
│   │   └── RealEstate.Api/             # API controllers and middleware
│   └── tests/                          # Unit and integration tests
├── frontend/                # React Frontend
│   ├── src/
│   │   ├── components/      # React components
│   │   ├── pages/           # Page components
│   │   ├── lib/             # Utilities, hooks, types, API client
│   │   └── App.tsx          # Main app component
│   └── tests/               # Component tests
├── docs/                    # Documentation
│   └── swagger.json         # OpenAPI specification
└── specifications/          # Project specifications
```

## 🛠️ Prerequisites

- **.NET 9 SDK**: https://dotnet.microsoft.com/download/dotnet/9.0
- **Node.js 20+**: https://nodejs.org/
- **MongoDB 7.0+**: https://www.mongodb.com/try/download/community
  - Or MongoDB Atlas (cloud): https://www.mongodb.com/cloud/atlas

## 🎯 Quick Start

### 1. Clone the Repository

```bash
git clone <repository-url>
cd test_millon
```

### 2. Set Up MongoDB

**Option A: Local MongoDB**
```bash
# macOS
brew tap mongodb/brew
brew install mongodb-community@7.0
brew services start mongodb-community@7.0

# Linux (Ubuntu/Debian)
# Follow: https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-ubuntu/

# Windows
# Download installer from: https://www.mongodb.com/try/download/community
```

**Option B: MongoDB Atlas (Cloud)**
1. Create free account at https://www.mongodb.com/cloud/atlas
2. Create a cluster
3. Get connection string
4. Update `.env` file with connection string

### 3. Backend Setup

```bash
cd backend

# Install dependencies (automatic with dotnet restore)
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Start the API
dotnet run --project src/RealEstate.Api

# API will be available at http://localhost:5000
# Swagger documentation at http://localhost:5000/swagger
```

**Seed Sample Data**
```bash
curl -X POST http://localhost:5000/api/seed
```

### 4. Frontend Setup

```bash
cd frontend

# Install dependencies
npm install

# Run tests
npm test

# Start development server
npm run dev

# Frontend will be available at http://localhost:3000
```

### 5. Build for Production

**Backend**
```bash
cd backend
dotnet publish -c Release -o ./publish
```

**Frontend**
```bash
cd frontend
npm run build
# Output will be in dist/ directory
```

## 🔧 Configuration

### Backend Environment Variables

Create `backend/.env` file:

```env
MongoDb__ConnectionString=mongodb://localhost:27017
MongoDb__DatabaseName=RealEstateDB
```

### Frontend Environment Variables

Create `frontend/.env` file:

```env
VITE_API_URL=http://localhost:5000/api
VITE_IMAGES_URL=http://localhost:5000/images
```

## 📚 API Documentation

Once the backend is running, access the Swagger documentation at:
- **Swagger UI**: http://localhost:5000/swagger
- **OpenAPI JSON**: http://localhost:5000/swagger/v1/swagger.json
- **Exported Schema**: `docs/swagger.json`

### Key Endpoints

- `GET /api/properties` - Get paginated list of properties with filtering
- `GET /api/properties/{id}` - Get property details by ID
- `POST /api/seed` - Seed database with sample data (Development only)
- `GET /health` - Health check endpoint

## 🧪 Testing

### Backend Tests

```bash
cd backend

# Run all tests
dotnet test

# Run specific test project
dotnet test tests/RealEstate.Domain.Tests
dotnet test tests/RealEstate.Application.Tests
dotnet test tests/RealEstate.Api.Tests

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

**Test Results**: 114/114 tests passing (100%)
- Domain Tests: 30 tests
- Application Tests: 55 tests
- API Tests: 29 tests

### Frontend Tests

```bash
cd frontend

# Run all tests
npm test

# Run tests in watch mode
npm test -- --watch

# Run tests with coverage
npm test -- --coverage
```

**Test Results**: 119/119 tests passing (100%)
- UI Components: 48 tests
- Property Components: 41 tests
- Utility Functions: 28 tests
- Additional Tests: 2 tests

## 🎨 Code Quality

### Backend

```bash
cd backend

# Format code
dotnet format

# Analyze code
dotnet build /p:TreatWarningsAsErrors=true
```

### Frontend

```bash
cd frontend

# Run linter
npm run lint

# Fix linting issues
npm run lint -- --fix
```

## 📊 Performance

- **API Response Time**: < 200ms (95th percentile)
- **Frontend Load Time**: < 3s (Time to Interactive)
- **Lighthouse Score**: > 90
- **Database Queries**: Optimized with indexes

## 🔐 Security

- **Input Validation**: FluentValidation on all API inputs
- **CORS**: Configured for specific origins
- **HTTPS**: Enforced in production
- **Environment Variables**: Sensitive data not hardcoded
- **MongoDB**: No SQL injection vulnerabilities

## 🤝 Contributing

1. Create a feature branch
2. Make your changes
3. Write/update tests
4. Ensure all tests pass
5. Run linter and formatter
6. Submit pull request

## 📝 Development Workflow

1. **Backend Development**
   - Write domain models and interfaces
   - Implement business logic in Application layer
   - Write tests alongside implementation
   - Add API endpoints
   - Update Swagger documentation

2. **Frontend Development**
   - Create TypeScript types
   - Build reusable UI components
   - Implement page components
   - Write component tests
   - Ensure responsive design

## 🐛 Troubleshooting

### MongoDB Connection Issues

```bash
# Check if MongoDB is running
mongo --eval "db.adminCommand('ping')"

# Check connection string in .env file
# Ensure firewall allows connection
```

### Backend Build Errors

```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Frontend Build Errors

```bash
# Clear node_modules and reinstall
rm -rf node_modules package-lock.json
npm install

# Clear Vite cache
rm -rf node_modules/.vite
```



## 👥 Authors

EslavaDev

---

**Built with ❤️ using Clean Architecture and Modern Web Technologies**
