#!/bin/bash

echo "Restoring dotnet tools..."
dotnet tool restore

echo "Installing Husky..."
dotnet husky install

echo "Initialization complete!"