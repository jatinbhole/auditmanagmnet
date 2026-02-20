#!/bin/bash
# Audit Management Platform - Development Environment Setup

set -e

GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

echo -e "${BLUE}"
echo "╔════════════════════════════════════════════════╗"
echo "║  Audit Management Platform - Dev Setup        ║"
echo "║  Version: 1.0.0                               ║"
echo "╚════════════════════════════════════════════════╝"
echo -e "${NC}\n"

# Check .NET installation
echo -e "${BLUE}1. Checking .NET installation...${NC}"
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}✗ .NET SDK not found${NC}"
    echo "Please install .NET 10 SDK from https://dotnet.microsoft.com/download"
    exit 1
fi
DOTNET_VERSION=$(dotnet --version)
echo -e "${GREEN}✓ .NET $DOTNET_VERSION found${NC}\n"

# Restore NuGet packages
echo -e "${BLUE}2. Restoring NuGet packages...${NC}"
dotnet restore
echo -e "${GREEN}✓ Packages restored${NC}\n"

# Build solution
echo -e "${BLUE}3. Building solution...${NC}"
dotnet build
echo -e "${GREEN}✓ Solution built successfully${NC}\n"

# Check PostgreSQL
echo -e "${BLUE}4. Checking PostgreSQL...${NC}"
if ! command -v psql &> /dev/null; then
    echo -e "${YELLOW}⚠ PostgreSQL client not found${NC}"
    echo "Please install PostgreSQL to continue"
    echo "Ubuntu/Debian: apt-get install postgresql-client"
    echo "macOS: brew install postgresql"
else
    echo -e "${GREEN}✓ PostgreSQL client found${NC}\n"
fi

# Create default appsettings.Development.json
echo -e "${BLUE}5. Creating development settings...${NC}"
if [ ! -f "src/AuditManagement.API/appsettings.Development.json" ]; then
    cat > src/AuditManagement.API/appsettings.Development.json <<EOF
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "JwtSettings": {
    "SecretKey": "dev-super-secret-key-change-in-production",
    "Issuer": "AuditManagement-Dev",
    "Audience": "AuditManagementUsers-Dev",
    "ExpirationMinutes": 60
  }
}
EOF
    echo -e "${GREEN}✓ Development settings created${NC}"
else
    echo -e "${YELLOW}⚠ Development settings already exist${NC}"
fi

echo -e "\n${GREEN}✅ Setup completed successfully!${NC}\n"

echo -e "${BLUE}Next steps:${NC}"
echo "1. Configure database connection:"
echo "   - Edit: src/AuditManagement.API/appsettings.json"
echo "   - Update ConnectionStrings section"
echo ""
echo "2. Run database setup:"
echo "   - ./setup-database.sh"
echo ""
echo "3. Start development server:"
echo "   - dotnet run --project src/AuditManagement.API"
echo ""
echo "4. API will be available at:"
echo "   - https://localhost:5001 (HTTPS)"
echo "   - http://localhost:5000 (HTTP)"
echo ""
echo -e "${BLUE}Project Structure:${NC}"
echo "- src/AuditManagement.Domain        - Domain entities"
echo "- src/AuditManagement.Application   - Business logic"
echo "- src/AuditManagement.Infrastructure - Repository implementation"
echo "- src/AuditManagement.Persistence   - Database context"
echo "- src/AuditManagement.API           - REST API"
echo ""
echo -e "${BLUE}Useful Commands:${NC}"
echo "dotnet build                                      # Build solution"
echo "dotnet run --project src/AuditManagement.API     # Run API"
echo "dotnet test                                       # Run tests (when available)"
echo "dotnet ef migrations add <Name> -p src/AuditManagement.Persistence  # Add migration"
echo "dotnet ef database update -p src/AuditManagement.Persistence         # Apply migrations"
echo ""
echo -e "${BLUE}Documentation:${NC}"
echo "- ARCHITECTURE.md  - Project architecture overview"
echo "- requarments.md   - Business requirements"
echo ""
