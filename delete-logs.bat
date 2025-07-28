@echo off
setlocal

echo Deleting log files...

set LOG_DIR1=StoreFront\Logs
set LOG_DIR2=Outbox.SimpleMessageBroker\Logs
set LOG_DIR3=ShipmentProcessor\Logs

del /q "%LOG_DIR1%\console.log"
del /q "%LOG_DIR2%\console.log"
del /q "%LOG_DIR3%\console.log"

echo Log files deleted.
endlocal
pause
