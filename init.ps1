Write-Host "Restoring dotnet tools..." -ForegroundColor Cyan
dotnet tool restore

Write-Host "Installing Husky..." -ForegroundColor Cyan
dotnet husky install

Write-Host "Initialization complete!" -ForegroundColor Green