#!/bin/bash

# Run DomainAskFor API and Client together

echo "🚀 Starting DomainAskFor..."
echo ""

# Function to cleanup on exit
cleanup() {
    echo ""
    echo "🛑 Stopping services..."
    kill $API_PID $CLIENT_PID 2>/dev/null
    exit
}

trap cleanup INT TERM

# Start API in background
echo "📡 Starting API on https://localhost:5001..."
cd src/DomainAskFor.API
dotnet run > ../../logs/api.log 2>&1 &
API_PID=$!
cd ../..

# Wait a moment for API to start
sleep 3

# Start Client in background
echo "💻 Starting Client on https://localhost:5003..."
cd src/DomainAskFor.Client
dotnet run > ../../logs/client.log 2>&1 &
CLIENT_PID=$!
cd ../..

echo ""
echo "✅ Services started!"
echo ""
echo "   API:    https://localhost:5001"
echo "   Client: https://localhost:5003"
echo "   Swagger: https://localhost:5001/swagger"
echo ""
echo "   Logs: ./logs/api.log and ./logs/client.log"
echo ""
echo "Press Ctrl+C to stop all services..."
echo ""

# Wait for both processes
wait
