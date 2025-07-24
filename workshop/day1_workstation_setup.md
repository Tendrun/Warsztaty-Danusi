# Day 1: Back-end Development - Workstation Setup

## Required Software Installation

1. **Visual Studio 2022 Community Edition**
   - Download from: https://visualstudio.microsoft.com/vs/community/
   - Workloads: ASP.NET and web development, .NET desktop development, Azure development

2. **Docker Desktop for Windows**
   - Download from: https://www.docker.com/products/docker-desktop
   - Requirements: Windows 10 64-bit Pro/Enterprise/Education, WSL 2, hardware virtualization

3. **Postman**
   - Download from: https://www.postman.com/downloads/

4. **Git for Windows**
   - Download from: https://git-scm.com/download/win

5. **.NET 8 SDK (Latest)**
   - Download from: https://dotnet.microsoft.com/download/dotnet/8.0

6. **SQL Server Management Studio (SSMS)**
   - Download from: https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms

7. **Visual Studio Code (Optional)**
   - Download from: https://code.visualstudio.com/
   - Extensions: C#, Docker, REST Client, OpenAPI Editor

## Environment Verification

Run these commands in PowerShell to verify your installation:

```powershell
# Check .NET version
dotnet --version  # Should show 8.x.x

# Check Docker installation
docker --version  # Should show Docker version 20.x.x or higher
docker-compose --version  # Should show docker-compose version 2.x.x or higher

# Check Git installation
git --version  # Should show git version 2.x.x
```

## Resources and References

- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/core/)
- [Docker Documentation](https://docs.docker.com/)
- [Git Documentation](https://git-scm.com/doc)