@echo off & chcp 65001 >nul
echo Publishing 64-bit single exe ...
echo.

dotnet publish src\PromptManager\PromptManager.csproj -c Release -r win-x64 -o publish

echo.
if %ERRORLEVEL% NEQ 0 goto failed
echo Done.
echo publish\PromptManager.exe

set EXE_PATH=publish\PromptManager.exe

echo.
rem 获取 GitHub 上最新 Release 的版本号
set CURRENT_VERSION=未知
gh release list --limit 1 >nul 2>&1
if %ERRORLEVEL% EQU 0 (
  for /f "tokens=1" %%i in ('gh release list --limit 1') do (
    set CURRENT_VERSION=%%i
    goto got_current_version
  )
)

:got_current_version
echo.
set /p VERSION=请输入版本号（当前版本号：%CURRENT_VERSION%），输入后按回车：
if "%VERSION%"=="" goto noversion

echo.
echo 将使用版本号 "%VERSION%" 创建或更新 GitHub Release...
echo.

rem 检查 GitHub CLI 是否可用
gh --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 goto gh_missing

rem 先尝试创建新的 Release（标签不存在时）
gh release create "%VERSION%" "%EXE_PATH%" --title "%VERSION%" --notes "Release %VERSION%"
if %ERRORLEVEL% EQU 0 goto end

echo.
echo Release 已存在，尝试向该 Release 上传/覆盖二进制文件...
gh release upload "%VERSION%" "%EXE_PATH%" --clobber
if %ERRORLEVEL% EQU 0 goto end

echo.
echo 上传到 GitHub Release 失败，请检查上面的错误信息。
goto end

:noversion
echo 未输入版本号，跳过上传到 GitHub Release。
goto end

:gh_missing
echo 未找到 GitHub CLI (gh)，请先安装并配置后再重试。
goto end

:failed
echo Failed. See errors above.
:end
echo.
pause
