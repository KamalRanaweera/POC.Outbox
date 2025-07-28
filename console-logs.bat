@echo off
setlocal

set LOG_DIR1=StoreFront\Logs
set LOG_DIR2=Outbox.SimpleMessageBroker\Logs
set LOG_DIR3=ShipmentProcessor\Logs

start "StoreFront Logs" powershell -NoExit -Command " $host.ui.RawUI.WindowTitle = 'StoreFront Logs'; Get-Content -Path '%LOG_DIR1%\console.log' -Wait -Tail 10"
start "Outbox Logs" powershell -NoExit -Command " $host.ui.RawUI.WindowTitle = 'SimpleMessageBroker Logs'; Get-Content -Path '%LOG_DIR2%\console.log' -Wait -Tail 10"
start "Shipment Logs" powershell -NoExit -Command " $host.ui.RawUI.WindowTitle = 'Shipment Logs'; Get-Content -Path '%LOG_DIR3%\console.log' -Wait -Tail 10"

endlocal
