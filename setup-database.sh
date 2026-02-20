#!/bin/bash
# Audit Management Platform - Database Setup Script

set -e

echo "🔧 Audit Management Platform - Database Setup"
echo "=============================================\n"

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check if PostgreSQL is installed
if ! command -v psql &> /dev/null; then
    echo -e "${YELLOW}⚠️  PostgreSQL client not found. Please install PostgreSQL.${NC}"
    exit 1
fi

# Database configuration
DB_HOST="${DB_HOST:-localhost}"
DB_PORT="${DB_PORT:-5432}"
DB_NAME="${DB_NAME:-AuditManagement}"
DB_USER="${DB_USER:-postgres}"

echo -e "${BLUE}Database Configuration:${NC}"
echo "Host: $DB_HOST"
echo "Port: $DB_PORT"
echo "Database: $DB_NAME"
echo "User: $DB_USER"
echo ""

# Check if database exists
echo -e "${BLUE}Checking database...${NC}"
if psql -h "$DB_HOST" -U "$DB_USER" -lqt | cut -d \| -f 1 | grep -qw "$DB_NAME"; then
    echo -e "${YELLOW}Database '$DB_NAME' already exists.${NC}"
    read -p "Drop and recreate? (y/n) " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        echo -e "${BLUE}Dropping database...${NC}"
        dropdb -h "$DB_HOST" -U "$DB_USER" "$DB_NAME"
        echo -e "${GREEN}✓ Database dropped${NC}"
    else
        echo -e "${YELLOW}Skipping database recreation${NC}"
    fi
else
    echo -e "${BLUE}Database not found, creating...${NC}"
fi

# Create database if it doesn't exist
echo -e "${BLUE}Creating database...${NC}"
createdb -h "$DB_HOST" -U "$DB_USER" "$DB_NAME" || true
echo -e "${GREEN}✓ Database created${NC}"

# Run EF Core migrations
echo -e "${BLUE}Running Entity Framework migrations...${NC}"
cd "$(dirname "$0")"

# Check if we're in the right directory
if [ ! -f "global.json" ]; then
    echo -e "${YELLOW}⚠️  global.json not found. Are you in the project root?${NC}"
    exit 1
fi

# Update connection string in appsettings.json
echo -e "${BLUE}Updating connection string...${NC}"
CONN_STRING="Host=$DB_HOST;Port=$DB_PORT;Database=$DB_NAME;Username=$DB_USER;Pooling=true"

# Use sed to update connection string (macOS compatible)
if [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS
    sed -i '' "s|Host=.*;|$CONN_STRING;|g" src/AuditManagement.API/appsettings.json
else
    # Linux
    sed -i "s|Host=.*;|$CONN_STRING;|g" src/AuditManagement.API/appsettings.json
fi

echo -e "${GREEN}✓ Connection string updated${NC}"

# Apply migrations
echo -e "${BLUE}Applying migrations...${NC}"
dotnet ef database update -p src/AuditManagement.Persistence/ -s src/AuditManagement.API/

echo -e "${GREEN}✓ Migrations applied${NC}"

echo ""
echo -e "${GREEN}✅ Database setup completed successfully!${NC}"
echo ""
echo -e "${BLUE}Next steps:${NC}"
echo "1. Update appsettings.json with your specific configuration"
echo "2. Run: dotnet run --project src/AuditManagement.API"
echo "3. API will be available at: https://localhost:5001"
