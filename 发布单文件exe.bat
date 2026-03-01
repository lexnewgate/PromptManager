@echo off
echo Publishing 64-bit single exe ...
echo.

dotnet publish src\PromptManager\PromptManager.csproj -c Release -r win-x64 -o publish

echo.
if %ERRORLEVEL% NEQ 0 goto failed
echo Done.
echo publish\PromptManager.exe
goto end
:failed
echo Failed. See errors above.
:end
echo.
pause
