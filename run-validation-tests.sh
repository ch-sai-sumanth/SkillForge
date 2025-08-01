#!/bin/bash

echo "🚀 Starting SkillForge API Response Validation Tests"
echo "================================================="

# Check if Docker is running (needed for MongoDB test container)
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker is not running. Please start Docker to run the integration tests."
    exit 1
fi

echo "✅ Docker is running"

# Navigate to the test project directory
cd "$(dirname "$0")"

# Restore dependencies
echo "📦 Restoring NuGet packages..."
dotnet restore tests/SkillForge.IntegrationTests/SkillForge.IntegrationTests.csproj

if [ $? -ne 0 ]; then
    echo "❌ Failed to restore packages"
    exit 1
fi

echo "✅ Packages restored successfully"

# Build the test project
echo "🔨 Building test project..."
dotnet build tests/SkillForge.IntegrationTests/SkillForge.IntegrationTests.csproj --no-restore

if [ $? -ne 0 ]; then
    echo "❌ Failed to build test project"
    exit 1
fi

echo "✅ Test project built successfully"

# Run the tests
echo "🧪 Running API Response Validation Tests..."
echo "This will start MongoDB containers and run comprehensive API tests"
echo ""

dotnet test tests/SkillForge.IntegrationTests/SkillForge.IntegrationTests.csproj \
    --no-build \
    --verbosity normal \
    --logger "console;verbosity=detailed" \
    --collect:"XPlat Code Coverage"

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ All API response validation tests passed!"
    echo "🎉 Your existing API contracts are stable and ready for DDD refactoring."
    echo ""
    echo "Next steps:"
    echo "1. Run these tests before any DDD changes: ./run-validation-tests.sh"
    echo "2. After DDD refactoring, run the tests again to ensure compatibility"
    echo "3. If tests fail, check the response format differences and update accordingly"
else
    echo ""
    echo "❌ Some tests failed. Review the output above to understand what needs to be fixed."
    echo "🔍 Common issues:"
    echo "   - API response format changes"
    echo "   - Missing or incorrect JSON schema validation"
    echo "   - Authentication/authorization issues"
    echo "   - Database connectivity problems"
fi

echo ""
echo "💡 Tips:"
echo "   - Run in development environment to see response validation middleware logs"
echo "   - Check the test output for specific schema validation errors"
echo "   - Use the JsonSchemaValidator class in your own tests"