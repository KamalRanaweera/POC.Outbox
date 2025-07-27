@echo off
setlocal

set LOG_DIR1=StoreFront\Logs
set LOG_DIR2=Outbox.SimpleMessageBroker\Logs
set LOG_DIR3=ShipmentProcessor\Logs

start "Console1" powershell -NoExit -Command "Get-Content -Path '%LOG_DIR1%\console.log' -Wait -Tail 10"
start "Console2" powershell -NoExit -Command "Get-Content -Path '%LOG_DIR2%\console.log' -Wait -Tail 10"
start "Console3" powershell -NoExit -Command "Get-Content -Path '%LOG_DIR3%\console.log' -Wait -Tail 10"

endlocal



